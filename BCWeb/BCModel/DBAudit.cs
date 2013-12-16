using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCModel.Audit
{
    public class DBAudit
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int AuditId { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [MaxLength(1)]
        [Required]
        public string ActionType { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Entity { get; set; }

        [Column(TypeName = "xml")]
        public string Columns { get; set; }

        [Column(TypeName = "xml")] // changed to plain string because 
        public string NewValue { get; set; }     
    }
}
