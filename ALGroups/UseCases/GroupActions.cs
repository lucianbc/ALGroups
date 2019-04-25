using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALGroups.Models;
using ALGroups.ViewModels;

namespace ALGroups.UseCases
{
    public class GetMessages : GroupMemberUseCase
    {
        public GetMessages(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db, groupId, requester)
        { }
        
        public MessagesViewModel Execute()
        {
            var messages = (from m in _db.Messages
                    where m.Group.Id == _membership.Group.Id
                    orderby m.CreationTimespamp descending
                    select m).ToList();

            return new MessagesViewModel
            {
                Messages = messages,
                RequesterIsAdmin = _membership.IsModerator,
                GroupName = _membership.Group.Name
            };
        }
    }

    public class GetFiles : GroupMemberUseCase
    {
        public GetFiles(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db, groupId, requester)
        { }

        public FilesViewModel Execute()
        {
            var files = (from m in _db.Files
                    where m.Group.Id == _membership.Group.Id
                    orderby m.CreationTimespamp descending
                    select m).ToList();
            return new FilesViewModel
            {
                Files = files,
                RequesterIsAdmin = _membership.IsModerator,
                GroupName = _membership.Group.Name
            };
        }
    }

    public class DeleteMessage : ModeratorUseCase
    {
        readonly int messageId;
        public DeleteMessage(ApplicationDbContext db, int groupId, ApplicationUser requester, int messageId) : base(db, groupId, requester)
        {
            this.messageId = messageId;
        }

        public void Execute()
        {
            Message m = new Message
            {
                Id = messageId
            };
            _db.Messages.Attach(m);
            _db.Messages.Remove(m);
            _db.SaveChanges();
        }
    }

    public class DeleteFile : ModeratorUseCase
    {
        readonly int fileId;
        public DeleteFile(ApplicationDbContext db, int groupId, ApplicationUser requester, int fileId) : base(db, groupId, requester)
        {
            this.fileId = fileId;
        }

        public void Execute()
        {
            File f = _db.Files.Find(fileId);
            if (f == null)
            {
                throw new HttpException(404, "File not found");
            }
            if (System.IO.File.Exists(f.FilePath))
                System.IO.File.Delete(f.FilePath);
            _db.Files.Remove(f);
            _db.SaveChanges();
        }
    }

    public class PostFile : GroupMemberUseCase
    {
        HttpPostedFileBase file;
        readonly string basePath;
        public PostFile(ApplicationDbContext db, int groupId, ApplicationUser requester, HttpPostedFileBase file, string basePath) : base(db, groupId, requester)
        {
            this.file = file;
            this.basePath = basePath;
        }

        public File Execute()
        {
            var fileName = System.IO.Path.GetFileName(file.FileName);
            var path = System.IO.Path.Combine(basePath, Guid.NewGuid().ToString());
            file.SaveAs(path);
            File f = new File
            {
                FileName = fileName,
                FilePath = path,
                Creator = _membership.User,
                Group = _membership.Group,
                ContentType = file.ContentType
            };
            _db.Files.Add(f);
            _db.SaveChanges();
            return f;
        }
    }

    public class RetrieveFile : GroupMemberUseCase
    {
        private int fileId;

        public RetrieveFile(ApplicationDbContext db, int groupId, ApplicationUser requester, int fileId) : base(db, groupId, requester)
        {
            this.fileId = fileId;
        }

        public Tuple<File, byte[]> Execute()
        {
            File f = _db.Files.Find(fileId);
            if (f == null)
            {
                throw new HttpException(404, "File not found");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(f.FilePath);
            return new Tuple<File, byte[]>(f, fileBytes);
        }
    }

    public class PostMessage : GroupMemberUseCase
    {
        private readonly PostMessageForm message;

        public PostMessage(ApplicationDbContext db, int groupId, ApplicationUser requester, PostMessageForm message) : base(db, groupId, requester)
        {
            this.message = message;
        }
        
        public Message Execute()
        {
            Message m = new Message
            {
                Group = _membership.Group,
                Creator = _membership.User,
                Title = message.Title,
                Content = message.Content
            };
            _db.Messages.Add(m);
            _db.SaveChanges();
            return m;
        }
    }

    public class CreateActivity : GroupMemberUseCase
    {
        private readonly Activity a;
        public CreateActivity(ApplicationDbContext db, int groupId, ApplicationUser requester, Activity a) : base(db, groupId, requester)
        {
            this.a = a;
        }

        public Activity Execute()
        {
            a.Creator = _membership.User;
            a.Group = _membership.Group;
            _db.Activities.Add(a);
            _db.SaveChanges();
            return a;
        }
    }

    public class ListActivities : GroupMemberUseCase
    {
        public ListActivities(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db, groupId, requester)
        {
        }
        
        public List<Activity> Execute()
        {
            var acts = (from a in _db.Activities
                        where a.Group.Id == _membership.Group.Id
                        select a).ToList();
            return acts;
        }
    }

    public class GetPageData : GroupMemberUseCase
    {
        public GetPageData(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db, groupId, requester)
        {
        }

        public BasicView Execute()
        {
            return new BasicView
            {
                IsModerator = _membership.IsModerator,
                GroupName = _membership.Group.Name
            };
        }
    }

    public class GetMembers : GroupMemberUseCase
    {
        private ApplicationUser requester;

        public GetMembers(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db, groupId, requester)
        {
            this.requester = requester;
        }

        public MembersView Execute()
        {
            var members = (from m in _db.Memberships
                           where m.Group.Id == _membership.Group.Id && m.User.Id != requester.Id
                           select m).ToList();
            return new MembersView
            {
                Mebmers = members,
                CanAlter = _membership.IsModerator,
                GroupName = _membership.Group.Name
            };
        }
    }

    public class GetRequests : ModeratorUseCase
    {
        public GetRequests(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db, groupId, requester)
        { }

        public RequestsView Execute()
        {
            var requests = (from r in _db.Requests
                           where r.Group.Id == _membership.Group.Id
                           select r).ToList();
            return new RequestsView
            {
                Requests = requests,
                CanAlter = _membership.IsModerator,
                GroupName = _membership.Group.Name
            };
        }
    }

    public class MakeModerator : ModeratorUseCase
    {
        private readonly int membershipId;

        public MakeModerator(ApplicationDbContext db, int groupId, ApplicationUser requester, int membershipId) : base(db, groupId, requester)
        {
            this.membershipId = membershipId;
        }

        public Membership Execute()
        {
            Membership membership = _db.Memberships.Where(m => m.Id == membershipId).First();
            membership.IsModerator = true;
            _db.SaveChanges();
            return membership;
        }
    }

    public class KickUser : ModeratorUseCase
    {
        private readonly int membershipId;

        public KickUser(ApplicationDbContext db, int groupId, ApplicationUser requester, int membershipId) : base(db, groupId, requester)
        {
            this.membershipId = membershipId;
        }

        public void Execute()
        {
            Membership membership = new Membership
            {
                Id = membershipId
            };
            _db.Memberships.Attach(membership);
            _db.Memberships.Remove(membership);
            _db.SaveChanges();
        }
    }

    public class ManageRequest : ModeratorUseCase
    {
        private readonly int requestId;
        private readonly bool accept;

        public ManageRequest(ApplicationDbContext db, int groupId, ApplicationUser requester, int requestId, bool accept) : base(db, groupId, requester)
        {
            this.requestId = requestId;
            this.accept = accept;
        }

        public void Execute()
        {
            var req = (from r in _db.Requests
                       where r.Id == requestId
                       select r).First();
            if (accept)
            {
                var mem = new Membership
                {
                    IsModerator = false,
                    User = req.User,
                    Group = req.Group
                };
                _db.Memberships.Add(mem);
            }

            _db.Requests.Remove(req);
            _db.SaveChanges();
        }
    }
}