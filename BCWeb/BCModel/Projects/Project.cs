using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel.Projects
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        [IgnoreDataMember]
        public virtual UserProfile Creator { get; set; }

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

        public string Architect { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<BidPackage> BidPackages { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ProjectXScope> Scopes { get; set; }


        public int BuildingTypeId { get; set; }
        [ForeignKey("BuildingTypeId")]
        [IgnoreDataMember]
        public virtual BuildingType BuildingType { get; set; }

        public int ProjectTypeId { get; set; }
        [ForeignKey("ProjectTypeId")]
        [IgnoreDataMember]
        public virtual ProjectType ProjectType { get; set; }

        public int ConstructionTypeId { get; set; }
        [ForeignKey("ConstructionTypeId")]
        [IgnoreDataMember]
        public virtual ConstructionType ConstructionType { get; set; }

        
    }
}
