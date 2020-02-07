using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NewsletterSub.Data
{
    public class NewsletterSubscription
    {
        public Guid NewsletterSubscriptionID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("Channel", TypeName = "nvarchar(24)")]
        [DisallowNull]
        public string? ChannelName { get; set; }

        public Guid ContactID { get; set; }
        public Contact? Contact { get; set; }
    }
}
