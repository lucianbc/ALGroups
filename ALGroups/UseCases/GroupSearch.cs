using ALGroups.Models;
using ALGroups.UseCases;
using ALGroups.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ALGroups.GroupSearch
{
    public class LatestGroups : UseCase
    {
        public LatestGroups(ApplicationDbContext db) : base(db) { }

        public List<GroupCard> Execute()
        {
            var groups =
                from g in _db.Groups
                orderby g.CreationTimestamp descending
                select g;
            return groups.ToList().Select(g => new GroupCard(g)).ToList();
        }
    }

    public class MyGroups : AuthenticatedUserUseCase
    {
        public MyGroups(ApplicationDbContext db, ApplicationUser requester) : base(db, requester) { }

        public List<Group> Execute()
        {
            var groups = IsAdmin(requester) ? AdminGroups() : UserGroups();
                
            return groups.ToList();
        }

        private IQueryable<Group> AdminGroups()
        {
            return (
                from g in _db.Groups
                select g
                );
        }

        private IQueryable<Group> UserGroups()
        {
            return (
                from m in _db.Memberships
                where m.User.Id == requester.Id
                select m.Group
                );
        }
    }

    public class GroupsByName : UseCase
    {
        public GroupsByName(ApplicationDbContext db, string name) : base(db)
        {
            this.name = name;
        }

        private readonly string name;

        public List<GroupCard> Execute()
        {
            var groups =
                from g in _db.Groups
                where g.Name == name
                select g;
            return groups.ToList().Select(g => new GroupCard(g)).ToList();
        }
    }
}