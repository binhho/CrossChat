using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Services;
using Abo.Server.Application.Tests.Stubs;
using Abo.Server.Domain.Repositories;
using Abo.Server.Domain.Seedwork;
using Moq;
using NUnit.Framework;

namespace Abo.Server.Application.Tests
{
    [TestFixture]
    public class PlayersSearchControllerTests
    {
        [Test]
        public void ShouldFindExistedPlayer()
        {
            var sessionManager = new SessionManagerMock();
            var aliceSessionMock = sessionManager.AppendPlayerAsSession("Alice");

            var playersRepository = new InMemoryUserRepository();
            playersRepository.Add(aliceSessionMock.Object.User);
            playersRepository.Add(new Domain.Entities.User("Egor", "123", "111111", true, 13, string.Empty, "Belarus"));
            playersRepository.Add(new Domain.Entities.User("EgorBo", "123", "111111", true, 13, string.Empty, "Belarus"));
            playersRepository.Add(new Domain.Entities.User("EgoBo", "123", "111111", true, 13, string.Empty, "Belarus"));

            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());
            
            var searchService = new UsersSearchService(uowFactory, Mock.Of<ITransactionFactory>());
            var response = searchService.SearchUser(aliceSessionMock.Object, new UsersSearchRequest { QueryString = "egor" });

            Assert.AreEqual(2, response.Result.Length);
        }

        [Test]
        public void ShouldGetDetailsForUser()
        {
            var sessionManager = new SessionManagerMock();
            var aliceSessionMock = sessionManager.AppendPlayerAsSession("Alice");

            var playersRepository = new InMemoryUserRepository();
            playersRepository.Add(aliceSessionMock.Object.User);
            var egorPlayer = new Domain.Entities.User("Egor", "123", "111111", true, 13, string.Empty, "Belarus");
            egorPlayer.GetType().GetProperty("Id").SetValue(egorPlayer, 5);
            playersRepository.Add(egorPlayer);

            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());
            var searchService = new UsersSearchService(uowFactory, Mock.Of<ITransactionFactory>());

            
            var response = searchService.GetUserDetails(aliceSessionMock.Object, new GetUserDetailsRequest { Name = "Egor" });
            Assert.AreEqual("Egor", response.User.Name);

            response = searchService.GetUserDetails(aliceSessionMock.Object, new GetUserDetailsRequest { Name = "Egorko" });
            Assert.IsNull(response.User);

            response = searchService.GetUserDetails(aliceSessionMock.Object, new GetUserDetailsRequest { UserId = 0 });
            Assert.IsNull(response.User);

            response = searchService.GetUserDetails(aliceSessionMock.Object, new GetUserDetailsRequest { UserId = 0 });
            Assert.IsNull(response.User);

            response = searchService.GetUserDetails(aliceSessionMock.Object, new GetUserDetailsRequest { UserId = egorPlayer.Id });
            Assert.IsNotNull(response.User);
        }
    }
}
