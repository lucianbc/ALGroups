using ALGroups.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALGroups.UseCases
{
    public abstract class UseCase
    {
        protected readonly ApplicationDbContext _db;

        public UseCase(ApplicationDbContext db)
        {
            this._db = db;
        }

        protected bool IsAdmin(ApplicationUser user)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            return userManager.IsInRole(user.Id, Startup.ADMIN_ROLE);
        }
    }

    public abstract class AuthenticatedUserUseCase : UseCase
    {
        protected readonly ApplicationUser requester;
        
        public AuthenticatedUserUseCase(ApplicationDbContext db, ApplicationUser requester) : base(db)
        {
            this.requester = requester;
        }
    }

    public abstract class GroupMemberUseCase : UseCase
    {
        protected readonly Membership _membership;
        
        public GroupMemberUseCase(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db)
        {
            if (IsAdmin(requester))
            {
                Group g = _db.Groups.Find(groupId);
                _membership = new Membership
                {
                    Group = g,
                    User = requester,
                    IsModerator = true
                };
            }
            else
            {
                var membership = (from m in _db.Memberships
                                  where m.Group.Id.Equals(groupId) && m.User.Id.Equals(requester.Id)
                                  select m).FirstOrDefault();
                _membership = membership ?? throw new UnauthorizedAccessException("You are not a member of the given group");
            }
        }
    }

    public abstract class ModeratorUseCase : GroupMemberUseCase
    {
        public ModeratorUseCase(ApplicationDbContext db, int groupId, ApplicationUser requester) : base(db, groupId, requester)
        {
            if (!_membership.IsModerator)
            {
                throw new UnauthorizedAccessException("You cannot perform the given acction because you are not a moderator");
            }
        }
    }
}