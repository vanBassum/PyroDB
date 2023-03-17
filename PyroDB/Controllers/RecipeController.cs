using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PyroDB.Data;
using PyroDB.Models;
using PyroDB.Models.Base;
using PyroDB.Models.Database;
using PyroDB.Services;
using System.Diagnostics;

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

        bool HasChems(Recipe recipe, ApplicationUser user)
        {
            foreach(var ingredient in recipe.Ingredients)
            {
                bool contains = ingredient.Chemical?.OwnedBy.Contains(user) ?? false;
                if (contains == false)
                    return false;
            }
            return true;
        }


        public async Task<IActionResult> Index()
        {
            //List of all chems, with bool wheter its owned by current user
            List<RecipeInfo> model = new List<RecipeInfo>();
            var recipes = await _context.Recipes.ToListAsync();
            var user = await _userManager.GetUserAsync(User);

            foreach (var recipe in recipes)
            {
                bool hasChemicals = false;
                if(user != null)
                    hasChemicals = HasChems(recipe, user);
                model.Add(new RecipeInfo(recipe, hasChemicals));
            }

            return View(model);
        }
    }
}
