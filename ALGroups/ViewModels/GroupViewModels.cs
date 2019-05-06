using ALGroups.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALGroups.ViewModels
{
    public class GroupDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanJoin { get; set; }
        
        public GroupDetails(Group group)
        {
            Id = group.Id;
            Name = group.Name;
            Description = group.Description;
        }
    }

    public class GroupCard
    {
        private static readonly int DES_LEN = 47;

        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Categories { get; set; }
        public string Description { get; set; }

        public GroupCard(Group group)
        {
            Id = group.Id;
            Name = group.Name;
            Description = group.Description.Length > DES_LEN + 3
                ? group.Description.Substring(DES_LEN) + "..."
                : group.Description;
            Categories = group.Categories.Select(c => c.Name).ToList();
        }
    }

    public class CreateGroupForm
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public int[] SelectedCategories { get; set; }
    }

    public class MessagesViewModel
    {
        public List<Message> Messages { get; set; }
        public string GroupName { get; set; }
        public bool RequesterIsAdmin { get; set; }
    }

    public class FilesViewModel
    {
        public List<File> Files { get; set; }
        public string GroupName { get; set; }
        public bool RequesterIsAdmin { get; set; }
    }
}