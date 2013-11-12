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
        [Key, Column(Order = 1)]
        public int BidId { get; set; }
        [ForeignKey("BidId")]
        [IgnoreDataMember]
        public Bid Bid { get; set; }

        [Key, Column(Order = 2)]
        public int ScopeId { get; set; }
        [ForeignKey("ScopeId")]
        [IgnoreDataMember]
        public Scope Scope { get; set; }

        public decimal? RiskFactor { get; set; }
    }
}
