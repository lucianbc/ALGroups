using ALGroups.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALGroups.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ApplicationDbContext _db;
        protected readonly ApplicationUserManager userManager;

        protected BaseController()
        {
            _db = new ApplicationDbContext();
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
        }
        
        protected ApplicationUser Requester()
        {
            if (!Request.IsAuthenticated) return null;
            return userManager.FindById(User.Identity.GetUserId());
        }

        protected String RequesterId()
        {
            return User.Identity.GetUserId();
        }
    }
}