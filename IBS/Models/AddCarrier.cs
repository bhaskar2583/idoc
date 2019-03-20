using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IBS.Models
{
    [Table("AddCarrier")]
    public class AddCarrier
    {
        public int Id { get; set; }

        public string CarrierName { get; set; }

        public bool Status { get; set; }

    }
}