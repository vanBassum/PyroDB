using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PyroDB.Data;
using PyroDB.Models;

namespace PyroDB.Controllers
{
    public class ChemicalsController : Controller
    {
        private readonly PyroDBContext _db;

        public ChemicalsController(PyroDBContext context)
        {
            _db = context;
        }

        // GET: Chemicals

        public async Task<IActionResult> Index()
        {
            return View(await _db.Chemical.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemical = await _db.Chemical
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chemical == null)
            {
                return NotFound();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                var userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName

                if(int.TryParse(userId, out int userIdInt))
                {
                    chemical.Labels = await _db.Label.Where(a => a.UserID == userIdInt).ToListAsync();
                }
                
            }
            return View(chemical);
        }


        [Authorize(Roles = Roles.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Chemical chemical)
        {
            if (ModelState.IsValid)
            {
                chemical.GHS = 0;

                if (chemical.CheckedSymbols != null)
                    foreach (var v in chemical.CheckedSymbols)
                        chemical.GHS += (int)v;

                _db.Add(chemical);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chemical);
        }



        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemical = await _db.Chemical.FindAsync(id);
            if (chemical == null)
            {
                return NotFound();
            }

            return View(chemical);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Chemical chemical)
        {
            if (id != chemical.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                chemical.GHS = 0;

                if(chemical.CheckedSymbols != null)
                    foreach (var v in chemical.CheckedSymbols)
                        chemical.GHS += (int)v;


                try
                {
                    _db.Update(chemical);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChemicalExists(chemical.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chemical);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemical = await _db.Chemical
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chemical == null)
            {
                return NotFound();
            }

            return View(chemical);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chemical = await _db.Chemical.FindAsync(id);
            _db.Chemical.Remove(chemical);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChemicalExists(int id)
        {
            return _db.Chemical.Any(e => e.ID == id);
        }

    }
}
