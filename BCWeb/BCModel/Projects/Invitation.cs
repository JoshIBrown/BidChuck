using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BCModel.Projects
{
    public class Invitation
    {
        [Key, Column(Order = 1)]
        public int BidPackageId { get; set; }
        [ForeignKey("BidPackageId")]
        [IgnoreDataMember]
        public virtual BidPackage BidPackage { get; set; }

        [Key, Column(Order = 2)]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [IgnoreDataMember]
        public virtual CompanyProfile Company { get; set; }



        public DateTime SentDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? RejectedDate { get; set; }

        public InvitationType InvitationType { get; set; }

    }

    public enum InvitationType
    {
        SentFromCreatedBy = 0,
        SelfInvite = 1
    }
}
