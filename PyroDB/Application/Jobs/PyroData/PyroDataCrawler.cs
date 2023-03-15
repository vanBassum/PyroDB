using HtmlAgilityPack;
using PyroDB.Application.Jobs.PyroData.Models;

namespace PyroDB.Application.Jobs.PyroData
{
    public class PyroDataCrawler
    {
        private readonly string baseUrl = "https://pyrodata.com";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public PyroDataCrawler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
        }


        public async Task IterateAllRecipeUrls(Func<string, CancellationToken, Task> recipeHandler, IProgress<double> progress, CancellationToken token = default)
        {
            string prefix = @"/composition/";
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int letterNo = 0; letterNo < letters.Length; letterNo++)
            {
                char letter = letters[letterNo];
                var recipeUrls = await FindRecipeLinksAsync(prefix + letter, token);
                foreach (var recipeUrl in recipeUrls)
                {
                    await recipeHandler.Invoke(recipeUrl, token);
                    token.ThrowIfCancellationRequested();
                }
                progress.Report((double)letterNo / letters.Length);
            }
        }

        public async Task IterateAllChemicalUrls(Func<string, CancellationToken, Task> chemicalHandler, IProgress<double> progress, CancellationToken token = default)
        {
            string prefix = @"/chemicals";
            var chemUrls = await FindChemicalLinksAsync(prefix, token);

            for(int i=0; i<chemUrls.Count; i++)
            {
                var chemUrl = chemUrls[i];
                await chemicalHandler.Invoke(chemUrl, token);
                token.ThrowIfCancellationRequested();
                progress.Report((double)i / chemUrls.Count);
            }
        }

        public async Task<RecipePD?> FetchRecipeAsync(string url, CancellationToken token = default)
        {
            var document = await GetDocumentAsync(url, token);
            if (document != null)
            {
                RecipePD result = new RecipePD();
                result.Uri = url;
                result.FromNode(document.DocumentNode);
                return result;
            }
            return null;
        }



        private async Task<List<string>> FindRecipeLinksAsync(string url, CancellationToken token = default)
        {
            var result = new List<string>();
            var document = await GetDocumentAsync(url, token);
            if (document != null)
            {
                var nodes = document.DocumentNode.Descendants("article");
                foreach (var node in nodes)
                {
                    var test = node.Descendants("h2");
                    var test2 = test?.FirstOrDefault()?.Descendants("a");
                    var test3 = test2?.FirstOrDefault()?.Attributes["href"];
                    if (test3 != null)
                        result.Add(test3.Value);
                }
            }
            return result;
        }


        public async Task<ChemicalPD?> FetchChemicalAsync(string url, CancellationToken token = default)
        {
            var document = await GetDocumentAsync(url, token);
            if (document != null)
            {
                ChemicalPD result = new ChemicalPD();
                result.Uri = url;
                result.FromNode(document.DocumentNode);
                return result;
            }
            return null;
        }
        private async Task<List<string>> FindChemicalLinksAsync(string url, CancellationToken token = default)
        {
            var result = new List<string>();
            var document = await GetDocumentAsync(url, token);
            if (document != null)
            {
                var nodes = document.DocumentNode.Descendants("td").Where(a => a.Attributes["class"]?.Value?.Contains("chemi-table") == true); 
                foreach (var node in nodes)
                {
                    var d = node.Descendants("a").FirstOrDefault(n => n.Attributes["href"]?.Value?.Contains("/chemicals/") == true);
                    var test = d?.Attributes["href"]?.Value;
                    if (test != null)
                        result.Add(test);
                }
            }
            return result;
        }

        private async Task<HtmlDocument?> GetDocumentAsync(string url, CancellationToken token = default)
        {
            var result = new List<string>();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, token);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync(token);
                HtmlDocument document = new HtmlDocument();
                document.Load(contentStream);
                return document;
            }
            return null;
        }
    }

}
