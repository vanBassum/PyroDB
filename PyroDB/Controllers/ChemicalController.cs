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
            //List of all chems, with bool wheter its owned by current user
            List<ChemicalInfo> chemicalInfos = new List<ChemicalInfo>();
            var chemicals = await _context.Chemicals.ToListAsync();
            var user = await _userManager.GetUserAsync(User);

            foreach (var chem in chemicals)
            {
                var chemInfo = ChemicalInfo.Create(chem);
                chemInfo.Owned = user?.OwnedChems?.Contains(chem) ?? false;
                chemicalInfos.Add(chemInfo);
            }

            return View(chemicalInfos);
        }
    }
}
