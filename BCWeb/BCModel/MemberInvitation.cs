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
    public class MemberInvitation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        public int SenderId { get; set; }
        [ForeignKey("SenderId")]
        [IgnoreDataMember]
        public virtual UserProfile Sender { get; set; }

        public string RcptEmail { get; set; }

        public int? RcptId { get; set; }
        [ForeignKey("RcptId")]
        [IgnoreDataMember]
        public virtual UserProfile Rcpt { get; set; }

        public DateTime? AcceptDate { get; set; }
    }
}
