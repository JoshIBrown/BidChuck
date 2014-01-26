using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel.SocialNetwork
{
    public class ContactRequest
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        [ForeignKey("SenderId"), IgnoreDataMember]
        public virtual CompanyProfile Sender { get; set; }

        [Required]
        public int RecipientId { get; set; }
        [ForeignKey("RecipientId"), IgnoreDataMember]
        public virtual CompanyProfile Recipient { get; set; }

        public DateTime? AcceptDate { get; set; }
        public DateTime? DeclineDate { get; set; }
        public DateTime SentDate { get; set; }
    }
}
