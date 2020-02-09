using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PagedList.Mvc;

namespace Web.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public int UserID { get; set; }

        public int RoleID { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string OrganizationName { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Please select Role")]
        public List<Roles> Roles { get; set; }

        [DisplayName("User")]
        [Required(ErrorMessage = "Please select User")]
        public List<UserModel> Users { get; set; }

    }

    public class UserRoleModel
    {
        public UserRole UserRole { get; set; }

        public PagedList.IPagedList<UserRole> UserRoles { get; set; }
    }
}