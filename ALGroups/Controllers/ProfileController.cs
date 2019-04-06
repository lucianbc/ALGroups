using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALGroups.GroupSearch;
using ALGroups.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ALGroups.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ActionResult Index()
        {
            return View(model: "Profile details");
        }

        public ActionResult Groups()
        {
            MyGroups search = new MyGroups(_db, Requester());
            return View(search.Execute());
        }
    }
}