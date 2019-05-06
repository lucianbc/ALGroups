using ALGroups.Models;
using ALGroups.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALGroups.UseCases
{
    public class FindGroupDetails : UseCase
    {
        private readonly ApplicationUser requester;
        private readonly int groupId;

        public FindGroupDetails(ApplicationDbContext db, int id, ApplicationUser requester = null) : base(db)
        {
            this.requester = requester;
            this.groupId = id;
        }

        public GroupDetails Execute()
        {
            var group = (from g in _db.Groups
                         where g.Id == groupId
                         select g).FirstOrDefault();

            return new GroupDetails(group)
            {
                CanJoin = requester != null && !HasSentRequest()
            };
        }

        private bool HasSentRequest()
        {
            var x = (from m in _db.Requests
                    where (m.Group.Id == groupId) && (m.User.Id == requester.Id)
                    select m).FirstOrDefault();
            return x != null;
        }
    }

    public class CreateGroup : AuthenticatedUserUseCase
    {
        private readonly CreateGroupForm form;

        public CreateGroup(ApplicationDbContext db, ApplicationUser requester, CreateGroupForm form) : base(db, requester)
        {
            this.form = form;
        }

        public GroupDetails Execute()
        {
            var categories = (from c in _db.Categories
                              where form.SelectedCategories.Contains(c.Id)
                              select c)
                              .ToList();
            var group = new Group
            {
                Name = this.form.Name,
                Description = this.form.Description,
                Categories = categories
            };

            _db.Groups.Add(group);

            var membership = new Membership
            {
                Group = group,
                User = requester,
                IsModerator = true
            };

            _db.Memberships.Add(membership);

            _db.SaveChanges();
            return new GroupDetails(group);
        }
    }

    public class RequestMembership : AuthenticatedUserUseCase
    {
        private readonly int groupId;
        public RequestMembership(ApplicationDbContext db, ApplicationUser requester, int groupId) : base(db, requester)
        {
            this.groupId = groupId;
        }

        public MembershipRequest Execute()
        {
            var isMember = _db.Memberships.Any(m => m.User.Id == requester.Id && m.Group.Id == groupId);

            if (isMember)
            {
                throw new InvalidOperationException("Already a member of the group!");
            }

            var hasRequested = _db.Requests.Any(m => m.User.Id == requester.Id && m.Group.Id == groupId);

            if (hasRequested)
            {
                throw new InvalidOperationException("Already requested permission");
            }

            Group g = new Group
            {
                Id = groupId
            };

            _db.Groups.Attach(g);

            MembershipRequest mr = new MembershipRequest
            {
                User = requester,
                Group = g
            };

            _db.Requests.Add(mr);
            _db.SaveChanges();

            return mr;
        }
    }
}