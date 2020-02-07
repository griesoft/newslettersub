using Microsoft.EntityFrameworkCore;
using NewsletterSub.Data;

namespace NewsletterSub.Tests.Data
{
    public class FakeNewsletterSubDbContext : DbContext, INewsletterSubDbContext
    {
        public FakeNewsletterSubDbContext()
        {

        }
        public FakeNewsletterSubDbContext(DbContextOptions<FakeNewsletterSubDbContext> options)
            : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }
    }
}
