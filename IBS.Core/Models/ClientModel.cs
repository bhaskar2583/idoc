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

        public List<DivisionModel> Divisions { get; set; }
        public DivisionModel SelectedDevision { get; set; }
        public bool IsActive { get; set; }
        public ClientModel()
        {
            Divisions = new List<DivisionModel>() { new DivisionModel() { Id = 1, Name = "Division 1" }, new DivisionModel() { Id = 2, Name = "Division 2" } };
        }
    }
}
