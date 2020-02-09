namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class UserProfile
    {
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
    }
}
