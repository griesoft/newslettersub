using System.Threading.Tasks;

namespace NewsletterSub
{
    /// <summary>
    /// A newsletter subscribing service, which makes newsletter subscribing easy and fast with less configuration needed.
    /// </summary>
    public interface INewsletterService
    {
        /// <summary>
        /// Creates a new newsletter subscription if it doesn't exist already. This also creates a new contact if it doesn't exist already.
        /// </summary>
        /// <param name="email">The email of the contact.</param>
        /// <param name="channel">The channel to which the contact is subscribing to. Like the name of the site.</param>
        /// <param name="firstname">The first name of the contact.</param>
        /// <param name="lastname">The last name of the contact.</param>
        /// <returns></returns>
        /// <remarks>Use the <paramref name="channel"/> parameter to keep track to what pages the contact has subscribed to.</remarks>
        Task AddNewsletterSubscription(string email, string channel, string? firstname = null, string? lastname = null);

        /// <summary>
        /// Remove a contact / user from the subscriber list of a channel. The contact will not be removed 
        /// and remains in the database even if the contact isn't subscribed to any channel.
        /// </summary>
        /// <param name="email">The email of an existing contact.</param>
        /// <param name="channel">The name of the channel from which the user wished to unsubscribe from.</param>
        /// <returns>Return true if removal was successful. In anyother case, like when the contact doesn't exist or 
        /// was not subscribed to the channel, false will be returned.</returns>
        Task<bool> RemoveNewsletterSubscription(string email, string channel);
    }
}
