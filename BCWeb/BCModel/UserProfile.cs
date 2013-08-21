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

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string County { get; set; }

        [Required]
        public string Phone { get; set; }

        public bool Published { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Scope> Scopes { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<UserProfile> Delegates { get; set; }

    }
}
