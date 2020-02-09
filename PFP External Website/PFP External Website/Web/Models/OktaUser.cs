using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.OktaUser
{
    public class Profile
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public object mobilePhone { get; set; }
        public string secondEmail { get; set; }
        public string middleName { get; set; }
        public string login { get; set; }
        public string email { get; set; }
    }

    public class Provider
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Password
    {
    }

    public class RecoveryQuestion
    {
        public string question { get; set; }
    }

    public class Credentials
    {
        public Provider provider { get; set; }
        public Password password { get; set; }
        public RecoveryQuestion recovery_question { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Users
    {
        public string id { get; set; }
        public string status { get; set; }
        public DateTime? created { get; set; }
        public DateTime? activated { get; set; }
        public DateTime? statusChanged { get; set; }
        public DateTime? lastLogin { get; set; }
        public DateTime? lastUpdated { get; set; }
        public DateTime? passwordChanged { get; set; }
        public Profile profile { get; set; }
        public Credentials credentials { get; set; }
        public Links _links { get; set; }
    }
}