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
    public class ProjectXScope
    {
        [Key, Column(Order = 0)]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [IgnoreDataMember]
        public virtual Project Project { get; set; }

        [Key,Column(Order=1)]
        public int ScopeId { get; set; }
        [ForeignKey("ScopeId")]
        [IgnoreDataMember]
        public virtual Scope Scope { get; set; }

    }
}
