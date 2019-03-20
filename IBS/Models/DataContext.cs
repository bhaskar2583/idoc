using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IBS.Models
{
    public class DataContext:DbContext
    {
        public DbSet<AddCarrier> Carriers { get; set; }
    }
}