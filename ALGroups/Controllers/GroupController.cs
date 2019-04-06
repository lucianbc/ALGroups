using ALGroups.GroupSearch;
using ALGroups.Models;
using ALGroups.UseCases;
using ALGroups.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALGroups.Controllers
{
    public class GroupController : BaseController
    {
        public ActionResult Details(int id)
        {
            var requester = Requester();
            if (RequesterInGroup(id, requester))
            {
                return RedirectToAction("Messages", new { id });
            }
            FindGroupDetails groupDetails = new FindGroupDetails(_db, id, Requester());
            return View(groupDetails.Execute());
        }

        public ActionResult Search(string name)
        {
            GroupsByName search = new GroupsByName(_db, name);
            return View(search.Execute());
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateGroupForm form)
        {
            CreateGroup createGroup = new CreateGroup(_db, Requester(), form);
            var g = createGroup.Execute();
            return RedirectToAction("Messages", new { id = g.Id });
        }
        
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [Route("Group/{id}/Messages")]
        public ActionResult Messages(int id)
        {
            GetMessages search = new GetMessages(_db, id, RequesterId());
            return View(model: search.Execute());
        }

        [Authorize]
        [Route("Group/{id}/Files")]
        public ActionResult Files(int id)
        {
            GetFiles search = new GetFiles(_db, id, RequesterId());
            return View(model: search.Execute());
        }

        [Authorize]
        [Route("Group/{id}/Members")]
        public ActionResult Members(int id)
        {
            GetMembers action = new GetMembers(_db, id, RequesterId());
            return View(action.Execute());
        }

        [Authorize]
        [Route("Group/{id}/Requests")]
        public ActionResult Requests(int id)
        {
            GetRequests action = new GetRequests(_db, id, RequesterId());
            return View(action.Execute());
        }

        [Authorize]
        [Route("Group/{id}/Calendar")]
        public ActionResult Calendar(int id)
        {
            return View((new GetPageData(_db, id, RequesterId())).Execute());
        }

        [Authorize]
        [Route("Group/{id}/Info")]
        public ActionResult Info(int id)
        {
            return View((new GetPageData(_db, id, RequesterId())).Execute());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Message(int id, PostMessageForm message)
        {
            PostMessage action = new PostMessage(_db, id, RequesterId(), message);
            action.Execute();
            return RedirectToAction("Messages", new { id });
        }

        [Authorize]
        [HttpPost]
        public ActionResult FileUpload(int id, HttpPostedFileBase uploadedFile)
        {
            if (Request.Files.Count > 0)
            {
                PostFile action = new PostFile(_db, id, RequesterId(), uploadedFile, Server.MapPath("~/Files/"));
                action.Execute();
            }
            return RedirectToAction("Files", new { id });
        }

        [Authorize]
        [Route("Group/{id}/FileDownload/{fileId}")]
        public ActionResult FileDownload(int id, int fileId)
        {
            RetrieveFile action = new RetrieveFile(_db, id, RequesterId(), fileId);
            var result = action.Execute();
            return File(result.Item2, result.Item1.ContentType, result.Item1.FileName);
        }

        private bool RequesterInGroup(int id, ApplicationUser requester)
        {
            return requester != null && _db.Memberships.Any(m => m.Group.Id == id && m.User.Id == requester.Id);
        }
    }
}