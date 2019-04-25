using ALGroups.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALGroups.Controllers
{
    public class MembershipController : BaseController
    {
        [Authorize]
        public ActionResult Ask(int groupId)
        {
            RequestMembership action = new RequestMembership(_db, Requester(), groupId);
            action.Execute();
            return RedirectToAction("Details", "Group", new { id = groupId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Kick(int groupId, int membershipId)
        {
            KickUser action = new KickUser(_db, groupId, Requester(), membershipId);
            action.Execute();
            return RedirectToAction("Members", "Group", new { id = groupId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult MakeModerator(int groupId, int membershipId)
        {
            MakeModerator action = new MakeModerator(_db, groupId, Requester(), membershipId);
            action.Execute();
            return RedirectToAction("Members", "Group", new { id = groupId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult ManageRequest(int groupId, int requestId, bool accept)
        {
            ManageRequest action = new ManageRequest(_db, groupId, Requester(), requestId, accept);
            action.Execute();
            return RedirectToAction("Requests", "Group", new { id = groupId });
        }
    }
}