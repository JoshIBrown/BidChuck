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
    public class BidPackage
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        [IgnoreDataMember]
        public virtual CompanyProfile CreatedBy { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [IgnoreDataMember]
        public virtual Project Project { get; set; }

        public int? TemplateBidPackageId { get; set; }
        [ForeignKey("TemplateBidPackageId")]
        [IgnoreDataMember]
        public virtual BidPackage TemplateBidPackage { get; set; }

        [Required]
        public DateTime? BidDateTime { get; set; }

        [Required]
        public bool IsMaster { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Bid> Bids { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<BidPackageXScope> Scopes { get; set; }

        [IgnoreDataMember]
        public ICollection<BidPackageXInvitee> Invitees { get; set; }

        [IgnoreDataMember]
        public ICollection<BidPackage> PatternedBidPackages { get; set; }

        // more meta data
    }
}
