using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PyroDB.Application.Jobs.PyroData.Models;
using PyroDB.Data;
using PyroDB.Models;
using PyroDB.Models.Base;
using PyroDB.Models.Database;
using PyroDB.Services;
using System.Diagnostics;

namespace PyroDB.Controllers
{
    public class ChemicalController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChemicalController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> Owned([FromBody] KeyValuePair<int, bool> data)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var chem = await _context.Chemicals.FindAsync(data.Key);
            if (chem == null)
                return NotFound();

            if(data.Value)
            {
                if(!user.OwnedChems.Contains(chem))
                    user.OwnedChems.Add(chem);
            }
            else
            {
                user.OwnedChems.Remove(chem);
            }


            await _context.SaveChangesAsync();
            return Ok();
        }



        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var query = _context.Chemicals.Select(chemical=>
                new ChemicalInfo
                {
                    Id = chemical.Id,
                    Name = chemical.Name,
                    Formula = chemical.Formula,
                    Owned = chemical.OwnedBy.Contains(user)
                }
            );


            var model = await query.ToListAsync();
            return View(model);
        }
    }
}
