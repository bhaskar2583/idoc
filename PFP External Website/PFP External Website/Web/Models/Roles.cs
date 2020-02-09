namespace Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Roles
    {
        public int Id { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Name required")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description required")]
        public string Description { get; set; }

        [Display(Name = "Active")]
        public string ActiveText { get { if (Active == true) return "Yes"; else return "No"; } }
        public bool? Active { get; set; }

        [Display(Name = "Users count by role")]
        public int UserCount { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Updated On")]
        public DateTime? UpdatedOn { get; set; }

        public string Current { get; set; }
    }

}