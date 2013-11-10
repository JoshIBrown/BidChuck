using BCModel.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel
{

    public enum BusinessType
    {
        [Description("General Contractor")]
        GeneralContractor = 0,
        [Description("Sub-Contractor")]
        SubContractor = 1,
        [Description("Architect")]
        Architect = 2,
        [Description("Materials Vendor")]
        MaterialsVendor = 3,
        [Description("Materials Manufacturer")]
        MaterialsMfg = 4,
        [Description("Consultant")]
        Consultant = 5,
        [Description("Engineer")]
        Engineer = 6,
        [Description("Owner/Client")]
        Owner = 7
    }
    public class CompanyProfile
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }

        
        public string PostalCode { get; set; }

        public int? StateId { get; set; }
        [ForeignKey("StateId")]
        [IgnoreDataMember]
        public virtual State State { get; set; }


        public int? CountyId { get; set; }
        [ForeignKey("CountyId")]
        [IgnoreDataMember]
        public virtual County County { get; set; }

        
        public string Phone { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        public bool Published { get; set; }

        
        public int OperatingDistance { get; set; }

        public string BusinessLicense { get; set; }

        public string Website { get; set; }

        public BusinessType BusinessType { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<UserProfile> Users { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<CompanyXScope> Scopes { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Invitation> InvitesToBPs { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
