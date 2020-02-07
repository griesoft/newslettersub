using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewsletterSub.Data
{
    /// <summary>
    /// This is the database context interface for this service. It must be implemented by the user of the service,
    /// otherwise the service can not work as intended.
    /// </summary>
    public interface INewsletterSubDbContext
    {
        DbSet<Contact> Contacts { get; set; }
        DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
