namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public partial class Hospital
    {
        [Display(Name = "Id")]
        public int HOS_Id { get; set; }

        [Display(Name = "Hospital Name")]
        public string HOS_HospitalName { get; set; }
    }
}