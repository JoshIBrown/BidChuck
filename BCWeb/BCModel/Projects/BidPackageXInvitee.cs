using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BCModel.Projects
{
    public class BidPackageXInvitee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BidPackageId { get; set; }
        [ForeignKey("BidPackageId")]
        [IgnoreDataMember]
        public virtual BidPackage BidPackage { get; set; }


        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [IgnoreDataMember]
        public virtual CompanyProfile Company { get; set; }

        public DateTime Sent { get; set; }
    }
}
