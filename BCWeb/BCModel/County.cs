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
    public class County
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int StateId { get; set; }
        [ForeignKey("StateId")]
        [IgnoreDataMember]
        public virtual State State { get; set; }


        // ignore datamember to avoid serialzing whole collection in the db audit
        [IgnoreDataMember]
        public virtual ICollection<UserProfile> Users { get; set; }
    }
}
