using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel
{
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

        [Required]
        public string PostalCode { get; set; }

        public int StateId { get; set; }
        [ForeignKey("StateId")]
        [IgnoreDataMember]
        public virtual State State { get; set; }


        public int? CountyId { get; set; }
        [ForeignKey("CountyId")]
        [IgnoreDataMember]
        public virtual County County { get; set; }

        [Required]
        public string Phone { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        public bool Published { get; set; }

        [Required]
        public int OperatingDistance { get; set; }

        public int BusinessTypeId { get; set; }
        [ForeignKey("BusinessTypeId")]
        [IgnoreDataMember]
        public virtual BusinessType BusinessType { get; set; }


        //public int? ManagerId { get; set; }
        //[ForeignKey("ManagerId")]
        //[IgnoreDataMember]
        //public virtual UserProfile Manager { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<UserProfile> Users { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<CompanyXScope> Scopes { get; set; }
    }
}
