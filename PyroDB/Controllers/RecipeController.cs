using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using PyroDB.Data;
using PyroDB.Helpers;
using PyroDB.Models.Base;
using PyroDB.Models.Database;
using System.Drawing;
using System.Net;
//using System.Web.Mvc;

namespace PyroDB.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? filter, string? order, int? page, int? size)
        {
            var model = await GetRecipeModelsAsync(filter, order, page, size);
            return View(model);
        }


        public async Task<List<RecipeInfo>> GetRecipeModelsAsync(string? filter, string? order, int? page, int? size)
        {
            page ??= 0;
            size ??= 5;

            var user = await _userManager.GetUserAsync(User);
            var query = _context.Recipes.Select(recipe => new RecipeInfo { 
                Id = recipe.Id,
                Name = recipe.Name,
                Ingredients = recipe.Ingredients.Select(ingredient=> new IngredientInfo { 
                    Id= ingredient.Id,
                    Name = ingredient.Name,
                    Quantity= ingredient.Quantity,
                    Chemical = ingredient.Chemical == null ? null : 
                    new ChemicalInfo { 
                        Id = ingredient.Chemical.Id,
                        Name = ingredient.Chemical.Name,
                        Formula = ingredient.Chemical.Formula,
                        Owned = ingredient.Chemical.OwnedBy.Contains(user)
                    }
                }),
            });

            return await query.CustomQuery(filter, order, page, size).ToListAsync();
        }

        public async Task<IActionResult> GetRecipes(string? filter, string? order, int? page, int? size)
        {
            List<string> recipes = new List<string>();
            var models = await GetRecipeModelsAsync(filter, order, page, size);

            foreach(var model in models)
            {
                recipes.Add(await Rendering.RenderPartialViewToString(this.ControllerContext, "_Recipe", model));
            }

            return Json(recipes);
        }
    }
}
