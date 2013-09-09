using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel
{
    public class UserXScope
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [IgnoreDataMember]
        public virtual UserProfile User { get; set; }

        [Key, Column(Order = 1)]
        public int ScopeId { get; set; }
        [ForeignKey("ScopeId")]
        [IgnoreDataMember]
        public virtual Scope Scope { get; set; }
    }
}
