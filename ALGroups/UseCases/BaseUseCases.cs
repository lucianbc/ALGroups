using ALGroups.Models;
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

        public GroupMemberUseCase(ApplicationDbContext db, int groupId, string userId) : base(db)
        {
            var membership = (from m in _db.Memberships
                         where m.Group.Id.Equals(groupId) && m.User.Id.Equals(userId)
                         select m).FirstOrDefault();
            _membership = membership ?? throw new UnauthorizedAccessException("You are not a member of the given group");
        }
    }

    public abstract class ModeratorUseCase : GroupMemberUseCase
    {
        public ModeratorUseCase(ApplicationDbContext db, int groupId, string userId) : base(db, groupId, userId)
        {
            if (!_membership.IsModerator)
            {
                throw new UnauthorizedAccessException("You cannot perform the given acction because you are not a moderator");
            }
        }
    }
}