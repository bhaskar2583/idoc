using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{

    public class UserModel
    {
        public int USR_Id { get; set; }

        [Display(Name = "First Name")]
        [RegularExpression("^((?!^First Name$)[a-zA-Z '])+$", ErrorMessage = "First name is required and must be properly formatted.")]
        [Required(ErrorMessage = "Please Enter User First Name")]
        public string USR_FirstName { get; set; }

        [Display(Name = "Last Name")]
        [RegularExpression("^((?!^Last Name$)[a-zA-Z '])+$", ErrorMessage = "Last name is required and must be properly formatted.")]
        [Required(ErrorMessage = "Please Enter User Last Name")]
        public string USR_LastName { get; set; }

        [Display(Name = "Email")]
        [RegularExpression("^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$",
        ErrorMessage = "Email is required and must be properly formatted.")]
        [Required(ErrorMessage = "Please Enter User Email")]
        public string USR_Email { get; set; }

        [DisplayName("Phone")]
        [RegularExpression("^[01]?[- .]?\\(?[2-9]\\d{2}\\)?[- .]?\\d{3}[- .]?\\d{4}$",
        ErrorMessage = "Phone is required and must be properly formatted.")]
        public string USR_Phone { get; set; }


        [Display(Name = "OrganizationName")]
        [Required(ErrorMessage = "Please Enter User OrganizationName")]
        public string USR_OrganizationName { get; set; }

        public bool USR_Active { get; set; }

        public string USR_OKTAID { get; set; }

        public int USR_RoleId { get; set; }

        public int USR_OrganizationId { get; set; }
        
        [DisplayName("Role")]
        public List<Roles> Roles { get; set; }

        public string USR_UserName { get; set; }

        public bool USR_IsAdmin { get; set; }
    }
}