using System;
using NewsletterSub.Data;

namespace Microsoft.EntityFrameworkCore
{
    public static class NewsletterSubModelBuilder
    {
        public static void BuildNewsletterSubModels(this ModelBuilder modelBuilder)
        {
            _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<Contact>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Contact>()
              .HasIndex(c => c.Email)
              .IsUnique();

            modelBuilder.Entity<NewsletterSubscription>()
              .Property(c => c.CreatedAt)
              .HasDefaultValueSql("getdate()");
        }
    }
}
