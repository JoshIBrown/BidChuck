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
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime LastEditTimestamp { get; set; }

        [Required]
        public int RecipientId { get; set; }
        [ForeignKey("RecipientId"), IgnoreDataMember]
        public virtual UserProfile Recipient { get; set; }

        [Required]
        public bool Read { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId"), IgnoreDataMember]
        public virtual Project Project { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public string Message { get; set; }
    }

    public class NotificationTemplate
    {
        [Key]
        public NotificationType NotificationType { get; set; }
        [Required]
        public string Text { get; set; }
    }

    public enum NotificationType
    {
        InvitationToBid = 0,
        InvitationResponse = 1,
        InvitationRequest = 2,
        ProjectChange = 3,
        BidSubmitted = 4
    }
}
