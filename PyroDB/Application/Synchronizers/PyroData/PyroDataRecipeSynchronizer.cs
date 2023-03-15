﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MySqlX.XDevAPI.Common;
using PyroDB.Data;
using PyroDB.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace PyroDB.Application.Synchronizers.PyroData
{
    public class PyroDataRecipeSynchronizer
    {
        private readonly PyroDataCrawler _crawler;
        private readonly ApplicationDbContext _context;

        public PyroDataRecipeSynchronizer(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _crawler = new PyroDataCrawler(httpClientFactory);
        }

        public async Task SyncAllAsync(IProgress<double> progress, CancellationToken token = default)
        {
            await _crawler.IterateAllRecipeUrls(HandleRecipe, progress, token);
        }

        public async Task HandleRecipe(string recipeUrl, CancellationToken token = default)
        {
            var dbRecipe = await GetRecipeFromDbAsync(recipeUrl, token);
            if(!CheckIfRecipeIsComplete(dbRecipe))
            {
                var pdRecipe = await _crawler.FetchRecipeAsync(recipeUrl, token);
                if (pdRecipe != null)
                {
                    await SyncRecipesAsync(pdRecipe, dbRecipe, token);
                }
            }
        }

        private async Task SyncRecipesAsync(RecipePD pdRecipe, Recipe? dbRecipe, CancellationToken token = default)
        {
            //Create recipe if it doens't yet exist
            if(dbRecipe == null)
            {
                dbRecipe = new Recipe();
                dbRecipe.DataSourceInfo = new DataSourceInfo
                {
                    DataSource = DataSources.PyroData,
                    SourceId = pdRecipe.Uri
                };
                await _context.AddAsync(dbRecipe, token);
            }

            //Sync dbRecipe properties
            if (dbRecipe.Name == null)
                dbRecipe.Name = pdRecipe.Name;

            foreach (var pdIngredient in pdRecipe.Ingredients)
            {
                //Create new ingredient if not exists
                var dbIngredient = dbRecipe.Ingredients.FirstOrDefault(i=>i.Name == pdIngredient.Name);
                if(dbIngredient == null)
                {
                    dbIngredient = new Ingredient();
                    await _context.AddAsync(dbIngredient, token);
                    dbRecipe.Ingredients.Add(dbIngredient);
                }

                //Sync ingredient properties
                if (dbIngredient.Name == null)
                    dbIngredient.Name = pdIngredient.Name;

                if(dbIngredient.Quantity == null)
                {
                    if (int.TryParse(pdIngredient.Quantity, out int quantity))
                        dbIngredient.Quantity = quantity;
                    else
                        Debug.WriteLine($"Quantity counl't be parsed '{pdIngredient.Quantity}'");
                }

                if (dbIngredient.Chemical == null)
                {
                    if (pdIngredient.ChemicalLink != null)
                    {
                        //await HandleChemical(pdIngredient.ChemicalLink, token);
                        dbIngredient.Chemical = await GetChemicalFromDbAsync(pdIngredient.ChemicalLink, token);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        private bool CheckIfRecipeIsComplete(Recipe? recipe)
        {
            if(recipe == null)
                return false;

            if (recipe.Ingredients.Any(i => i.Quantity == null || i.Chemical == null))
                return false;

            //Add more checks to determine if any of the properties need updating.

            return true;
        }

        private async Task<Recipe?> GetRecipeFromDbAsync(string recipeUrl, CancellationToken token = default)
        {
            var foundByDataSource = from r in _context.Recipes
                                    where r.DataSourceInfo != null
                                    && r.DataSourceInfo.DataSource == DataSources.PyroData
                                    && r.DataSourceInfo.SourceId == recipeUrl
                                    select r;
            return await foundByDataSource.FirstOrDefaultAsync(token);
        }


        private async Task<Chemical?> GetChemicalFromDbAsync(string chemicalUrl, CancellationToken token = default)
        {
            var foundByDataSource = from r in _context.Chemicals
                                    where r.DataSourceInfo != null
                                    && r.DataSourceInfo.DataSource == DataSources.PyroData
                                    && r.DataSourceInfo.SourceId == chemicalUrl
                                    select r;
            return await foundByDataSource.FirstOrDefaultAsync(token);
        }
    }
}
