using BCModel.Projects;
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
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [IgnoreDataMember]
        public virtual CompanyProfile Company { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<UserXScope> Scopes { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
