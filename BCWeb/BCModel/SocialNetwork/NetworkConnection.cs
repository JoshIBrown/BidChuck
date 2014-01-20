using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel.SocialNetwork
{
    public class NetworkConnection
    {
        [Key, Column(Order = 0)]
        public int LeftId { get; set; }
        [ForeignKey("LeftId"), IgnoreDataMember]
        public virtual CompanyProfile Left { get; set; }

        [Key, Column(Order = 1)]
        public int RightId { get; set; }
        [ForeignKey("RightId"), IgnoreDataMember]
        public virtual CompanyProfile Right { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
