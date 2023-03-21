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

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var hasChemsRecipes = await _context.Recipes.Where(a => a.Ingredients.All(i => i.Chemical.OwnedBy.Contains(user))).ToListAsync();

            //List of all chems, with bool wheter its owned by current user
            List<RecipeInfo> model = new List<RecipeInfo>();
            var recipes = await _context.Recipes.ToListAsync();
            

            foreach (var recipe in recipes)
            {
                bool hasChemicals = hasChemsRecipes.Contains(recipe);
                model.Add(new RecipeInfo(recipe, hasChemicals));
            }

            return View(model);
        }
    }
}
