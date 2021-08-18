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
    }
}
