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
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime LastEditTimestamp { get; set; }

        [Required]
        public int RecipientId { get; set; }
        [ForeignKey("RecipientId"), IgnoreDataMember]
        public virtual UserProfile Recipient{ get; set; }

        [Required]
        public bool Read { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string LinkText { get; set; }
    }

    public enum NotificationType
    {
        InvitationToBid = 0,
        InvitationResponse = 1,
        ProjectChange = 2,
        BidSubmitted = 3
    }
}
