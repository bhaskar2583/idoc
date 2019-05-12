using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CoverageId { get; set; }
    }
    public class CorporateProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CoverageId { get; set; }
    }
    public class CorporateProductsXProduct
    {
        public int Id { get; set; }
        public int CorporateProductId { get; set; }
        public int ProductId { get; set; }
    }
}
