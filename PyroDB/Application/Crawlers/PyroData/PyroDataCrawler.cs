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

namespace PyroDB.Application.Crawlers.PyroData
{
    public class PyroDataCrawler
    {
        private readonly string baseUrl = "https://pyrodata.com";
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;


        public PyroDataCrawler(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
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
                var recipe = GetRecipe(recipeLinks[i]);
                token.ThrowIfCancellationRequested();
                progress.Report((double)i / count);
            }
        }


        private async Task<Recipe?> GetRecipe(string url)
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
                    Ingredients = await FindIngredients(document)
                };
                return result;
            }
            return null;
        }

        private async Task<List<Ingredient>> FindIngredients(HtmlDocument document)
        {
            List<Ingredient> result = new List<Ingredient>();
            var nodes = document.DocumentNode.Descendants("div").Where(a => a.Attributes["typeof"]?.Value?.Contains("schema:Ingredient") == true);
            foreach(var node in nodes)
            {
                var nameNode = node.Descendants("span").Where(a => a.Attributes["class"].Value.Contains("ingredient-name")).FirstOrDefault();
                var quantityNode = node.Descendants("div").Where(a => a.Attributes["class"].Value.Contains("quantity-unit")).FirstOrDefault();
                var linkNode = nameNode?.Descendants("a").FirstOrDefault();
                Ingredient ingredient = new Ingredient();
                
                if(int.TryParse(GetNumbers(quantityNode?.InnerText), out int quantity))
                    ingredient.Amount = quantity;

                ingredient.Name = nameNode?.InnerText;
                var link = linkNode?.Attributes["href"]?.Value;
                if (link != null)
                {
                    ingredient.Chemical = await GetChemical(link);
                }
                result.Add(ingredient);
            }
            return result;
        }


        private async Task<Chemical?> GetChemical(string url)
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
                    try
                    {
                        var existing = await _context.Chemicals.FirstOrDefaultAsync(a => a.Formula == formula);
                        if (existing != null)
                            return existing;
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
                        return chemical;
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
            return null;
        }



        private static string? GetNumbers(string? input)
        {
            if (input == null)
                return null;
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
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
