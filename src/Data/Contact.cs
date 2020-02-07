using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NewsletterSub.Data
{
    public class Contact
    {
        public Guid ContactID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisallowNull]
        public string? Email { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<NewsletterSubscription>? NewsletterSubscriptions { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
