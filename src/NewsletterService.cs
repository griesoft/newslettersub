using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsletterSub.Data;

namespace NewsletterSub
{
    /// <inheritdoc />
    public class NewsletterService : INewsletterService
    {
        private readonly INewsletterSubDbContext _context;
        private readonly ILogger _logger;

        public NewsletterService(INewsletterSubDbContext context, ILogger<NewsletterService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddNewsletterSubscription(string email, string channel, string? firstname = null, string? lastname = null)
        {
            try
            {
                var contact = _context.Contacts
                    .Include(c => c.NewsletterSubscriptions)
                    .SingleOrDefault(c => c.Email == email);

                // Create new contact if doesn't exist
                if (contact == null)
                {
                    contact = new Contact()
                    {
                        Email = email,
                        FirstName = firstname,
                        LastName = lastname
                    };

                    _context.Contacts.Add(contact);
                }

                await CreateAndSaveNewsletterSubscription(contact, channel).ConfigureAwait(false);

                _logger.LogInformation("Added newsletter subscription successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saving newsletter subscription failed");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> RemoveNewsletterSubscription(string email, string channel)
        {
            try
            {
                var contact = _context.Contacts
                    .Include(c => c.NewsletterSubscriptions)
                    .SingleOrDefault(c => c.Email == email);

                if (contact == null)
                {
                    return false;
                }

                var subscription = contact.NewsletterSubscriptions.SingleOrDefault(sub => sub.ChannelName == channel);

                if (subscription == null)
                {
                    return false;
                }

                _context.NewsletterSubscriptions.Remove(subscription);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                _logger.LogInformation("Removed newsletter subscription successfully.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Removing newsletter subscription failed");
                throw;
            }
        }


        private async Task CreateAndSaveNewsletterSubscription(Contact contact, string channel)
        {
            _ = contact.NewsletterSubscriptions ??= new List<NewsletterSubscription>();

            // Add newsletter subscription if doesn't exist
            if (!contact.NewsletterSubscriptions.Any(nSub => nSub.ChannelName == channel))
            {
                contact.NewsletterSubscriptions.Add(new NewsletterSubscription()
                {
                    ChannelName = channel
                });
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
