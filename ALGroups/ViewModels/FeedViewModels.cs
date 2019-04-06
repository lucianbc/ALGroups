using ALGroups.Models;
using ALGroups.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALGroups.ViewModels
{
    public class FeedViewModel
    {
        public string Content { get; set; }
    }

    public class PostMessageForm
    {
        public string Content { get; set; }
        public string Title { get; set; }
    }

    public class MembersView
    {
        public List<Membership> Mebmers;
        public bool CanAlter { get; set; }
        public string GroupName { get; set; }
    }

    public class RequestsView
    {
        public List<MembershipRequest> Requests { get; set; }
        public bool CanAlter { get; set; }
        public string GroupName { get; set; }
    }

    public class BasicView
    {
        public bool IsModerator { get; set; }
        public string GroupName { get; set; }
    }
}