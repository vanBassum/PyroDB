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
    public class UsersController : Controller
    {
        private readonly PyroDBContext _context;

        public UsersController(PyroDBContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }


        // GET: Users/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var keyNew = Helper.GeneratePassword(10);
                var password = Helper.EncodePassword(user.Password, keyNew);
                user.Password = password;
                user.VCode = keyNew;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        // GET: Users/Create
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                User dbUser = _context.User.FirstOrDefault(a => a.Name == user.Name);
                if(dbUser != null)
                {
                    var password = Helper.EncodePassword(user.Password, dbUser.VCode);
                    var match = password == dbUser.Password;

                    if(match)
                    {
                        Session["UserID"] = obj.UserId.ToString();
                        Session["UserName"] = obj.UserName.ToString();
                    }
                }
                else
                {
                    //Not found!
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
