using ALGroups.GroupSearch;
using ALGroups.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALGroups.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            LatestGroups search = new LatestGroups(_db);
            var result = search.Execute();
            return View(result);
        }
    }
}