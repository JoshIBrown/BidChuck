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
    public class ComputedBid
    {
        [Key, Column(Order = 0)]
        public int BidPackageId { get; set; }
        [ForeignKey("BidPackageId"), IgnoreDataMember]
        public virtual BidPackage BidPackage { get; set; }

        [Key, Column(Order = 1)]
        public int SentToId { get; set; }
        [ForeignKey("SentToId"), IgnoreDataMember]
        public CompanyProfile SentTo { get; set; }

        [Key, Column(Order = 2)]
        public int ScopeId { get; set; }
        [ForeignKey("ScopeId"), IgnoreDataMember]
        public Scope Scope { get; set; }

        public decimal? RiskFactor { get; set; }
    }
}
