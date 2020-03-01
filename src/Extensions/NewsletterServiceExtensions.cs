using NewsletterSub;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NewsletterServiceExtensions
    {
        /// <summary>
        /// Add the <see cref="NewsletterService"/> to the project and register it for dependency injection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNewsletterService(this IServiceCollection services)
        {
            services.AddScoped<INewsletterService, NewsletterService>();
            return services;
        }
    }
}
