//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Models.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int USR_Id { get; set; }
        public string USR_FirstName { get; set; }
        public string USR_LastName { get; set; }
        public string USR_Email { get; set; }
        public string USR_Phone { get; set; }
        public string USR_OrganizationName { get; set; }
        public Nullable<bool> USR_Active { get; set; }
        public string USR_CreatedBy { get; set; }
        public Nullable<System.DateTime> USR_CreatedOn { get; set; }
        public string USR_UpdatedBy { get; set; }
        public Nullable<System.DateTime> USR_UpdatedOn { get; set; }
    }
}
