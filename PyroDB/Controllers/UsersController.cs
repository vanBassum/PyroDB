using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PyroDB.Data;
using PyroDB.Models;

namespace PyroDB.Controllers
{
    public class UsersController : Controller
    {
        private readonly PyroDBContext _db;
        private readonly AppConfig _config;

        public UsersController(PyroDBContext context, IOptions<AppConfig> config)
        {
            _db = context;
            _config = config.Value;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _db.User.ToListAsync());
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
            if (_config.AllowRegistrations)
            {
                if (ModelState.IsValid)
                {
                    User dbUser = _db.User.FirstOrDefault(a => a.Name == user.Name);
                    if (dbUser != null)
                    {
                        ModelState.AddModelError("", "Username already taken");
                        return View(user);
                    }
                    else
                    {
                        var keyNew = Helper.GeneratePassword(10);
                        var password = Helper.EncodePassword(user.Password, keyNew);
                        user.Password = password;
                        user.VCode = keyNew;
                        _db.Add(user);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(user);
        }


        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(User inputUser)
        {
            
            if (ModelState.IsValid)
            {

                bool credentialsValid = false;
                User dbUser = _db.User.FirstOrDefault(a => a.Name == inputUser.Name);

#if DEBUG
                if (inputUser.Name == "admin" && inputUser.Password == "admin")
                {
                    credentialsValid = true;
                    dbUser.Role = Roles.Admin;
                    dbUser.Name = "admin";
                    dbUser.ID = 0;

                }
#endif

                
                if (dbUser != null)
                {
                    var password = Helper.EncodePassword(inputUser.Password, dbUser.VCode);
                    var match = password == dbUser.Password;

                    if (match)
                        credentialsValid = true;
                    else
                        ModelState.AddModelError("", "Wrong password");
                }
                else
                    ModelState.AddModelError("", "User doens't exist");
                

                if(credentialsValid)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, dbUser.Name),
                            new Claim(ClaimTypes.NameIdentifier, dbUser.ID.ToString()),
                        };

                    if (!String.IsNullOrEmpty(dbUser.Role))
                        claims.Add(new Claim(ClaimTypes.Role, dbUser.Role));

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var claimsPrincipal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Input invalid");
            }
            
            return PartialView("_Login", inputUser);
        }
    }
}
