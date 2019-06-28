 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Models
{
    public class ClientModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Division { get; set; }
        public List<PolicyModel> Policies { get; set; }

        public List<DivisionModel> Divisions { get; set; }
        public DivisionModel SelectedDivision { get; set; }
        public bool IsActive { get; set; }
        public ClientModel()
        {
            Policies = new List<PolicyModel>();
            Divisions = new List<DivisionModel>() { new DivisionModel() { Id = 1, Name = "HBS" }, new DivisionModel() { Id = 2, Name = "SBS" } };
        }
    }
}
