using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Spatial;
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
        [Description("County Government")]
        County = 2,
        [Description("Private")]
        Private = 3,
        [Description("Private - Non-Profit")]
        PrivateNP = 4,
        [Description("City Government")]
        City = 5
    }

    public enum ProjectCategory
    {
        [Description("New Construction")]
        NewConstruction = 0,
        [Description("Renovation")]
        Renovation = 1,
        [Description("Tenant Improvement")]
        TenantImprovement = 2,
        [Description("Addition")]
        Addition = 3

    }

    public enum WalkThruStatus
    {
        NoWalkThru = 0,
        WalkThruTBD = 1,
        WalkThruIncluded = 2
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

        [Required]
        public int ArchitectId { get; set; }
        [ForeignKey("ArchitectId")]
        [IgnoreDataMember]
        public virtual CompanyProfile Architect { get; set; }


        public int? ClientId { get; set; }
        [ForeignKey("ClientId")]
        [IgnoreDataMember]
        public virtual CompanyProfile Client { get; set; }


        [Required]
        public string Title { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime BidDateTime { get; set; }


        [Required]
        public WalkThruStatus WalkThruStatus { get; set; }
        public DateTime? WalkThruDateTime { get; set; }


        public DbGeography GeoLocation { get; set; }
        
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
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

        public ProjectCategory ProjectCategory { get; set; }

        public int ConstructionTypeId { get; set; }
        [ForeignKey("ConstructionTypeId")]
        [IgnoreDataMember]
        public virtual ConstructionType ConstructionType { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ProjectDocument> ProjectDocuments { get; set; }
    }
}