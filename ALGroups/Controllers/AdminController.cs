using ALGroups.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ALGroups.Controllers
{
    public class AdminController : BaseController
    {
        [Authorize(Roles = Startup.ADMIN_ROLE)]
        public ActionResult Users()
        {
            var users = (
                from u in _db.Users
                select u
                );
            return View(model: users);
        }

        [Authorize(Roles = Startup.ADMIN_ROLE)]
        public ActionResult Categories()
        {
            var categories = (
                from c in _db.Categories
                select c
                ).ToList();
            return View(model: categories);
        }

        [Authorize(Roles = Startup.ADMIN_ROLE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Categories(string categoryName)
        {
            Category c = new Category
            {
                Name = categoryName
            };
            _db.Categories.Add(c);
            _db.SaveChanges();
            return RedirectToAction("Categories");
        }

        [Authorize(Roles = Startup.ADMIN_ROLE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BlockUser(string userId)
        {
            DateTime lockoutEnd = DateTime.Now.AddDays(30);
            //DateTimeOffset localLockoutEnd = new DateTimeOffset(lockoutEnd, new TimeSpan(+2, 0, 0));
            await userManager.SetLockoutEndDateAsync(userId, lockoutEnd);
            return RedirectToAction("Users");
        }

        [Authorize(Roles = Startup.ADMIN_ROLE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnblockUser(string userId)
        {
            await userManager.SetLockoutEndDateAsync(userId, DateTime.Now);
            return RedirectToAction("Users");
        }
    }
}