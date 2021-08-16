using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PyroDB.Data;
using PyroDB.Models;

namespace PyroDB.Controllers
{
    public class ChemicalsController : Controller
    {
        private readonly PyroDBContext _context;

        public ChemicalsController(PyroDBContext context)
        {
            _context = context;
        }

        // GET: Chemicals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chemical.ToListAsync());
        }

        // GET: Chemicals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemical = await _context.Chemical
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chemical == null)
            {
                return NotFound();
            }

            return View(chemical);
        }

        // GET: Chemicals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chemicals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Symbol,Notes,GHS")] Chemical chemical)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chemical);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chemical);
        }

        // GET: Chemicals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemical = await _context.Chemical.FindAsync(id);
            if (chemical == null)
            {
                return NotFound();
            }
            return View(chemical);
        }

        // POST: Chemicals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Symbol,Notes,GHS")] Chemical chemical)
        {
            if (id != chemical.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chemical);
                    await _context.SaveChangesAsync();
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

        // GET: Chemicals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemical = await _context.Chemical
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chemical == null)
            {
                return NotFound();
            }

            return View(chemical);
        }

        // POST: Chemicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chemical = await _context.Chemical.FindAsync(id);
            _context.Chemical.Remove(chemical);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChemicalExists(int id)
        {
            return _context.Chemical.Any(e => e.ID == id);
        }
    }
}
