using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MySqlX.XDevAPI.Common;
using PyroDB.Data;
using PyroDB.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace PyroDB.Application.Synchronizers.PyroData
{
    public class PyroDataSynchronizer
    {
        private readonly string baseUrl = "https://pyrodata.com";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;

        public PyroDataSynchronizer(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
        {
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseUrl);


        }

        public async Task SyncAll(IProgress<double> progress, CancellationToken token = default)
        {
            await SyncRecipes(progress, token);
        }

        private async Task SyncRecipes(IProgress<double> progress, CancellationToken token = default)
        {
            string prefix = @"/composition/";
            List<string> recipeLinks = new List<string>();
            foreach (var c in "A") // ABCDEFGHIJKLMNOPQRSTUVWXYZ
            {
                var url = prefix + c;
                recipeLinks.AddRange(await FindRecipeLinks(url));
            }

            int count = recipeLinks.Count;
            for(int i =0; i<count; i++)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = _serviceProvider.GetService<ApplicationDbContext>();
                    if (context != null)
                    {
                        var recipe = await GetRecipe(context, recipeLinks[i]);
                        if (recipe != null)
                        { 
                            if (!await SyncRecipe(context, recipe))
                            {
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                //Duplicate
                            }
                        }
                    }
                }

                token.ThrowIfCancellationRequested();
                progress.Report((double)i / count);
            }
        }

        async Task<bool> SyncRecipe(ApplicationDbContext context, Recipe recipe)
        {
            var dbRecipe = await FindByDataSource(context, recipe);
            if (dbRecipe == null)
            { 
                
            }

            if ( != null)
                return true;
            return false;
        }


        async Task<Recipe?> FindByDataSource(ApplicationDbContext context, Recipe recipe)
        {
            DataSources? s = recipe.DataSourceInfo?.DataSource;
            string? sid = recipe.DataSourceInfo?.SourceId;
            if (s != null && sid != null)
            {
                var foundByDataSource = from r in context.Recipes
                                        where r.DataSourceInfo != null
                                        && r.DataSourceInfo.DataSource == s
                                        && r.DataSourceInfo.SourceId == sid
                                        select r;
                return await foundByDataSource.FirstOrDefaultAsync();
            }
            return null;
        }



        private async Task<Recipe?> GetRecipe(ApplicationDbContext context, string url)
        {
            var document = await GetDocument(url);
            if (document != null)
            { 
                var titleNode = document.GetElementbyId("page-title");
                Recipe result = new Recipe()
                {
                    DataSourceInfo = new DataSourceInfo()
                    {
                        DataSource = DataSources.PyroData,
                        SourceId = url,
                    },
                    Name = titleNode.InnerText,
                    Ingredients = await FindIngredients(context, document)
                };
                context.Recipes.Add(result);
                return result;
            }
            return null;
        }

        private async Task<List<Ingredient>> FindIngredients(ApplicationDbContext context, HtmlDocument document)
        {
            List<Ingredient> result = new List<Ingredient>();
            var nodes = document.DocumentNode.Descendants("div").Where(a => a.Attributes["typeof"]?.Value?.Contains("schema:Ingredient") == true);
            foreach(var node in nodes)
            {
                var nameNode = node.Descendants("span").Where(a => a.Attributes["class"].Value.Contains("ingredient-name")).FirstOrDefault();
                var quantityNode = node.Descendants("div").Where(a => a.Attributes["class"].Value.Contains("quantity-unit")).FirstOrDefault();
                var linkNode = nameNode?.Descendants("a").FirstOrDefault();
                Ingredient ingredient = new Ingredient()
                {
                    Name = nameNode?.InnerText,
                    Quantity = quantityNode?.InnerText,
                };
                
                var link = linkNode?.Attributes["href"]?.Value;
                if (link != null)
                {
                    ingredient.Chemical = await GetChemical(context, link);
                }
                context.Ingredients.Add(ingredient);
                result.Add(ingredient);
            }
            return result;
        }

        private static string? GetNumbers(string? input)
        {
            if (input == null)
                return null;
            return new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray());
        }

        private async Task<Chemical?> GetChemical(ApplicationDbContext context, string url)
        {
            //first try to find if the chemical already exists, otherwise create new one. (Match by formula)
            var document = await GetDocument(url);
            if (document != null)
            {
                var titleNode = document.GetElementbyId("page-title");
                var formulaNode1 = document.DocumentNode.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value?.Contains("field-name-field-symbol") == true);
                var formulaNode2 = formulaNode1?.Descendants("p").FirstOrDefault();
                string? formula = formulaNode2?.InnerText;
                if (formula != null)
                {
              
                    var existing = await context.Chemicals.FirstOrDefaultAsync(a => a.Formula == formula);
                    if (existing != null)
                        return existing;

                    var existing2 = context.ChangeTracker.Entries<Chemical>().FirstOrDefault(a => a.Entity.Formula == formula);
                    if (existing2 != null)
                        return existing2.Entity;
                    //Create new 
                    Chemical chemical = new Chemical()
                    {
                        Formula = formula,
                        Name = titleNode?.InnerText,
                        DataSourceInfo = new DataSourceInfo()
                        {
                            DataSource = DataSources.PyroData,
                            SourceId = url,
                        }
                    };
                    context.Chemicals.Add(chemical);
                    return chemical;

                }
            }
            return null;
        }





        private async Task<List<string>> FindRecipeLinks(string url)
        {
            var result = new List<string>();
            var document = await GetDocument(url);
            if (document != null)
            {
                var nodes = document.DocumentNode.Descendants("article");
                foreach(var node in nodes)
                {
                    var test = node.Descendants("h2");
                    var test2 = test?.FirstOrDefault()?.Descendants("a");
                    var test3 = test2?.FirstOrDefault()?.Attributes["href"];
                    if(test3 != null)
                        result.Add(test3.Value);
                }
            }
            return result;
        }


        private async Task<HtmlDocument?> GetDocument(string url)
        {
            var result = new List<string>();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                HtmlDocument document = new HtmlDocument();
                document.Load(contentStream);
                return document;
            }
            return null;
        }
    }
}
