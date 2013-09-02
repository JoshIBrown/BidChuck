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

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [IgnoreDataMember]
        public virtual Project Project { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Bid> Bids { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Scope> Scopes { get; set; }
        
        // more meta data
    }
}
