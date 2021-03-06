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
    
    public partial class Hospital
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hospital()
        {
            this.NYSPFPDatas = new HashSet<NYSPFPData>();
        }
    
        public int HOS_Id { get; set; }
        public Nullable<int> HOS_PFI { get; set; }
        public string HOS_Unique_Id { get; set; }
        public string HOS_HospitalName { get; set; }
        public string HOS_Parent_Id { get; set; }
        public bool HOS_Active { get; set; }
        public string HOS_CreatedBy { get; set; }
        public System.DateTime HOS_CreatedOn { get; set; }
        public string HOS_UpdatedBy { get; set; }
        public System.DateTime HOS_UpdatedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NYSPFPData> NYSPFPDatas { get; set; }
    }
}
