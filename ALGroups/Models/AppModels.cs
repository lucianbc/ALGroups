using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ALGroups.Models
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationTimestamp { get; private set; } = DateTime.Now;
        public virtual ICollection<Category> Categories { get; set; }
    }

    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string _name;
        [StringLength(255)]
        [Index(IsUnique = true)]
        public string Name
        {
            get { return _name; } set { _name = value.ToLower().Trim(); }
        }

        public virtual ICollection<Group> Groups { get; set; }
    }

    public class Membership
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool IsModerator { get; set; }
    }

    public class MembershipRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
    }

    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

        public DateTime CreationTimespamp { get; private set; } = DateTime.Now;
        public virtual ApplicationUser Creator { get; set; }
        public virtual Group Group { get; set; }
    }

    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }

        public DateTime CreationTimespamp { get; private set; } = DateTime.Now;
        public virtual ApplicationUser Creator { get; set; }
        public virtual Group Group { get; set; }
    }

    public class Activity : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date: ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy hh:mm:ss}")]
        public DateTime Start { get; set; }

        [Display(Name = "End Date: ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy hh:mm:ss}")]
        public DateTime? End { get; set; }

        [Display(Name = "Full day event: ")]
        public bool? IsFullDay { get; set; }

        public DateTime CreationTimespamp { get; private set; } = DateTime.Now;
        public virtual ApplicationUser Creator { get; set; }
        public virtual Group Group { get; set; }
            
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (End < Start)
            {
                yield return new ValidationResult("End Date must be greater than StartDate");
            }

            if (End == null && IsFullDay == null || End == null && IsFullDay == false || End != null && IsFullDay == true)
            {
                yield return new ValidationResult("Either Full Day or End Date must be specified");
            }
        }
    }
}