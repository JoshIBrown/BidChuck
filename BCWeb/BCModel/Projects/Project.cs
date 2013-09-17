using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel.Projects
{
    public enum ProjectType
    {
        [Description("Federal Government")]
        Federal = 0,
        [Description("State Government")]
        State = 1,
        [Description("Local Government")]
        Local = 2,
        [Description("Private")]
        Private = 3,
        [Description("Private - Non-Profit")]
        PrivateNP = 4
    }

    public class Project
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        [IgnoreDataMember]
        public virtual UserProfile CreatedBy { get; set; }

        public int? ArchitectId { get; set; }
        [ForeignKey("ArchitectId")]
        [IgnoreDataMember]
        public virtual CompanyProfile Architect { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime BidDateTime { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        [IgnoreDataMember]
        public virtual State State { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<BidPackage> BidPackages { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ProjectXScope> Scopes { get; set; }


        public int BuildingTypeId { get; set; }
        [ForeignKey("BuildingTypeId")]
        [IgnoreDataMember]
        public virtual BuildingType BuildingType { get; set; }


        public ProjectType ProjectType { get; set; }

        public int ConstructionTypeId { get; set; }
        [ForeignKey("ConstructionTypeId")]
        [IgnoreDataMember]
        public virtual ConstructionType ConstructionType { get; set; }


    }
}
