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
    public class ProjectDocument
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId"), IgnoreDataMember]
        public virtual Project Project { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), IgnoreDataMember]
        public virtual CompanyProfile Company { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        public string Notes { get; set; }
    }
}
