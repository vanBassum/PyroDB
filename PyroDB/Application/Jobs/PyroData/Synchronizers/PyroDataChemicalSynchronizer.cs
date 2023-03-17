using Microsoft.EntityFrameworkCore;
using PyroDB.Application.Jobs.PyroData.Models;
using PyroDB.Data;
using PyroDB.Models.Database;
using System.Diagnostics;

namespace PyroDB.Application.Jobs.PyroData.Synchronizers
{
    public class PyroDataChemicalSynchronizer
    {
        private readonly PyroDataCrawler _crawler;
        private readonly ApplicationDbContext _context;

        public PyroDataChemicalSynchronizer(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _crawler = new PyroDataCrawler(httpClientFactory);
        }

        public async Task SyncAllAsync(IProgress<double> progress, CancellationToken token = default)
        {
            await _crawler.IterateAllChemicalUrls(HandleChemical, progress, token);
        }

        public async Task HandleChemical(string chemicalUrl, CancellationToken token = default)
        {
            var dbChemical = await GetChemicalFromDbAsync(chemicalUrl, token);
            if (!CheckIfChemicalIsComplete(dbChemical))
            {
                var pdChemical = await _crawler.FetchChemicalAsync(chemicalUrl, token);
                if (pdChemical != null)
                {
                    await SyncChemicalsAsync(pdChemical, dbChemical, token);
                }
            }
        }

        private async Task SyncChemicalsAsync(ChemicalPD pdChemical, Chemical? dbChemical, CancellationToken token = default)
        {
            //Create chem if it doens't yet exist
            if (dbChemical == null)
            {
                dbChemical = new Chemical();
                dbChemical.DataSourceInfo = new DataSourceInfo
                {
                    DataSource = DataSources.PyroDataSynchronizer,
                    SourceId = pdChemical.Uri
                };
                await _context.AddAsync(dbChemical, token);
            }

            //Sync chem properties
            if (dbChemical.Name == null)
                dbChemical.Name = pdChemical.Name;

            if (dbChemical.Formula == null)
                dbChemical.Formula = pdChemical.Formula;
            await _context.SaveChangesAsync();
        }

        private bool CheckIfChemicalIsComplete(Chemical? chemical)
        {
            if (chemical == null)
                return false;

            if (chemical.Formula == null)
                return false;

            //Add more checks to determine if any of the properties need updating.

            return true;
        }

        private async Task<Chemical?> GetChemicalFromDbAsync(string chemicalUrl, CancellationToken token = default)
        {
            var foundByDataSource = from r in _context.Chemicals
                                    where r.DataSourceInfo != null
                                    && r.DataSourceInfo.DataSource == DataSources.PyroDataSynchronizer
                                    && r.DataSourceInfo.SourceId == chemicalUrl
                                    select r;
            return await foundByDataSource.FirstOrDefaultAsync(token);
        }
    }
}
