using System;

namespace API.Models
{
    public class UserVM
    {
        public int USR_Id { get; set; }
        public string USR_FirstName { get; set; }
        public string USR_LastName { get; set; }
        public string USR_Email { get; set; }
        public string USR_Phone { get; set; }
        public string USR_OrganizationName { get; set; }
        public bool? USR_Active { get; set; }
        public string USR_CreatedBy { get; set; }
        public DateTime? USR_CreatedOn { get; set; }
        public string USR_UpdatedBy { get; set; }
        public DateTime? USR_UpdatedOn { get; set; }
        public int USR_RoleId { get; set; }
        public string USR_UserName { get; set; }
        
    }
}