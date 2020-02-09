using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class HospitalModel
    {
        public int Id { get; set; }
        public string HospitalName { get; set; }
        //public bool Active { get; set; }
        //public string CreatedBy { get; set; }
        //public Nullable<System.DateTime> CreatedOn { get; set; }
        //public string UpdatedBy { get; set; }
        //public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Parent_Id { get; set; }
    }
}