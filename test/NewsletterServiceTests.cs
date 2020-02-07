using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NewsletterSub.Data;
using NewsletterSub.Tests.Data;
using NUnit.Framework;

namespace NewsletterSub.Tests
{
    [TestFixture]
    public class NewsletterServiceTests
    {
        private Contact _contact;
        private NewsletterSubscription _newsletterSubscription;

        [SetUp]
        public void Init()
        {
            _contact = new Contact()
            {
                Email = "jg@test.com",
                FirstName = "Test",
                LastName = "Testerich"
            };
            _newsletterSubscription = new NewsletterSubscription()
            {
                ChannelName = "Test"
            };
        }

        [Test]
        public async Task AddNewsletterSubscription_ShouldAdd_ContactAndSubscription()
        {
            // Arrange
            var logMock = new Mock<ILogger<NewsletterService>>();
            var fakeContextOptions = new DbContextOptionsBuilder<FakeNewsletterSubDbContext>()
                .UseInMemoryDatabase(databaseName: "AddNewsletterSubscription_ShouldAdd_ContactAndSubscription")
                .Options;

            // Act
            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                var service = new NewsletterService(fakeContext, logMock.Object);

                await service.AddNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName, _contact.FirstName, _contact.LastName);
            }

            // Assert
            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                Assert.Greater(fakeContext.Contacts.Count(), 0);
                Assert.Greater(fakeContext.NewsletterSubscriptions.Count(), 0);
            }
        }

        [Test]
        public async Task AddNewsletterSubscription_ShouldAddOnly_Subscription()
        {
            // Arrange
            var logMock = new Mock<ILogger<NewsletterService>>();
            var fakeContextOptions = new DbContextOptionsBuilder<FakeNewsletterSubDbContext>()
                .UseInMemoryDatabase(databaseName: "AddNewsletterSubscription_ShouldAddOnly_Subscription")
                .Options;

            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                var service = new NewsletterService(fakeContext, logMock.Object);

                await service.AddNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName, _contact.FirstName, _contact.LastName);
            }

            _newsletterSubscription.ChannelName = "Test2";

            // Act
            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                var service = new NewsletterService(fakeContext, logMock.Object);

                await service.AddNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName, _contact.FirstName, _contact.LastName);
            }

            // Assert
            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                Assert.AreEqual(fakeContext.Contacts.Count(), 1);
                Assert.AreEqual(fakeContext.NewsletterSubscriptions.Count(), 2);
            }
        }

        [Test]
        public async Task AddNewsletterSubscription_ShouldNotAdd_Existing()
        {
            // Arrange
            var logMock = new Mock<ILogger<NewsletterService>>();
            var fakeContextOptions = new DbContextOptionsBuilder<FakeNewsletterSubDbContext>()
                .UseInMemoryDatabase(databaseName: "AddNewsletterSubscription_ShouldNotAdd_Existing")
                .Options;

            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                var service = new NewsletterService(fakeContext, logMock.Object);

                await service.AddNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName, _contact.FirstName, _contact.LastName);
            }

            // Act
            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                var service = new NewsletterService(fakeContext, logMock.Object);

                await service.AddNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName, _contact.FirstName, _contact.LastName);
            }

            // Assert
            using (var fakeContext = new FakeNewsletterSubDbContext(fakeContextOptions))
            {
                Assert.AreEqual(1, fakeContext.Contacts.Count());
                Assert.AreEqual(1, fakeContext.NewsletterSubscriptions.Count());
            }
        }

        [Test]
        public async Task RemoveNewsletterSubscription_ContactDoesntExist_ShouldReturnFalse()
        {
            // Arrange
            var logMock = new Mock<ILogger<NewsletterService>>();
            var fakeContextOptions = new DbContextOptionsBuilder<FakeNewsletterSubDbContext>()
                .UseInMemoryDatabase(databaseName: "RemoveNewsletterSubscription_ContactDoesntExist_ShouldReturnFalse")
                .Options;
            using var actFakeContext = new FakeNewsletterSubDbContext(fakeContextOptions);

            // Act
            var result = await new NewsletterService(actFakeContext, logMock.Object)
                .RemoveNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName);

            // Assert
            using var assertFakeContext = new FakeNewsletterSubDbContext(fakeContextOptions);
            Assert.AreEqual(0, assertFakeContext.Contacts.Count());
            Assert.AreEqual(0, assertFakeContext.NewsletterSubscriptions.Count());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveNewsletterSubscription_SubscriptionDoesntExist_ShouldReturnFalse()
        {
            // Arrange
            var logMock = new Mock<ILogger<NewsletterService>>();
            var fakeContextOptions = new DbContextOptionsBuilder<FakeNewsletterSubDbContext>()
                .UseInMemoryDatabase(databaseName: "RemoveNewsletterSubscription_SubscriptionDoesntExist_ShouldReturnFalse")
                .Options;
            using var actFakeContext = new FakeNewsletterSubDbContext(fakeContextOptions);
            actFakeContext.Contacts.Add(new Contact() { Email = _contact.Email });
            await actFakeContext.SaveChangesAsync();

            // Act
            var result = await new NewsletterService(actFakeContext, logMock.Object)
                .RemoveNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName);

            // Assert
            using var assertFakeContext = new FakeNewsletterSubDbContext(fakeContextOptions);
            Assert.AreEqual(1, assertFakeContext.Contacts.Count());
            Assert.AreEqual(0, assertFakeContext.NewsletterSubscriptions.Count());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveNewsletterSubscription__ShouldReturnTrue_SubscriptionIsRemoved()
        {
            // Arrange
            var logMock = new Mock<ILogger<NewsletterService>>();
            var fakeContextOptions = new DbContextOptionsBuilder<FakeNewsletterSubDbContext>()
                .UseInMemoryDatabase(databaseName: "RemoveNewsletterSubscription__ShouldReturnTrue_SubscriptionIsRemoved")
                .Options;
            using var actFakeContext = new FakeNewsletterSubDbContext(fakeContextOptions);
            await new NewsletterService(actFakeContext, logMock.Object).AddNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName);

            // Act
            var result = await new NewsletterService(actFakeContext, logMock.Object)
                .RemoveNewsletterSubscription(_contact.Email, _newsletterSubscription.ChannelName);

            // Assert
            using var assertFakeContext = new FakeNewsletterSubDbContext(fakeContextOptions);
            Assert.AreEqual(1, assertFakeContext.Contacts.Count());
            Assert.AreEqual(0, assertFakeContext.NewsletterSubscriptions.Count());
            Assert.IsTrue(result);
        }
    }
}
