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
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Email { get; set; }

   
        public string FirstName { get; set; }

    
        public string LastName { get; set; }


        // you need to track the id of the entities you want to be associated with
        // as well as have a reference to the entity itself
        // you set the foreign key to point at the id, the entity reference uses this
        // leaving out required at the moment.  it break db update because we don't have a default value for it.
        public int? StateId { get; set; }
        [ForeignKey("StateId")]
        [IgnoreDataMember]
        public virtual State State { get; set; }


        public int? CountyId { get; set; }
        [ForeignKey("CountyId")]
        [IgnoreDataMember]
        public virtual County County { get; set; }

        [Required]
        public string Phone { get; set; }

        public bool Published { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Scope> Scopes { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<UserProfile> Delegates { get; set; }

    }
}
