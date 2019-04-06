using ALGroups.Models;
using ALGroups.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALGroups.Controllers
{
    public class ActivityController : BaseController
    {
        [Authorize]
        public ActionResult List(int groupId)
        {
            ListActivities action = new ListActivities(_db, groupId, RequesterId());
            return Json(action.Execute(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Create(int groupId)
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Activity activity, int groupId)
        {
            if (ModelState.IsValid)
            {
                CreateActivity action = new CreateActivity(_db, groupId, RequesterId(), activity);
                action.Execute();
                return RedirectToAction("Calendar", "Group", new { id = groupId });
            }
            return View(activity);
        }
    }
}