using Abo.Server.Application.Contracts;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Services;
using Abo.Server.Application.Sessions;
using Abo.Server.Application.Tests.Stubs;
using Abo.Server.Domain.Repositories;
using Abo.Server.Domain.Seedwork;
using Moq;
using NUnit.Framework;

namespace Abo.Server.Application.Tests
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        [Test]
        public void ShouldSuccesfullyAuthenticate()
        {
            var playersRepository = new InMemoryUserRepository();
            playersRepository.Add(new Domain.Entities.User("Egor", "123", "111111", true, 13, string.Empty, "Belarus"));

            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());

            var sessionMock = new Mock<ISession>();
            var transactionFactoryMock = new Mock<ITransactionFactory>();

            var authService = new AuthenticationService(Mock.Of<ISettings>(), transactionFactoryMock.Object, uowFactory);
            var response = authService.Authenticate(sessionMock.Object, 
                new AuthenticationRequest
                    {
                        Name = "Egor",
                        Password = "123",
                        Huid = "111111",
                    });

            Assert.AreEqual(AuthenticationResponseType.Success, response.Result);
        }
        [Test]
        public void ShouldNotAuthenticateBannedPlayers()
        {
            var playersRepository = new InMemoryUserRepository();
            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());

            var moderator = new Domain.Entities.User("Egor", "123", "111111", true, 13, string.Empty, "Belarus");
            moderator.GrantModeratorship();

            var player = new Domain.Entities.User("Egor", "123", "111111", true, 13, string.Empty, "Belarus");
            playersRepository.Add(player);
            player.Ban(moderator);

            var sessionMock = new Mock<ISession>();
            var transactionFactoryMock = new Mock<ITransactionFactory>();

            var authService = new AuthenticationService(Mock.Of<ISettings>(), transactionFactoryMock.Object, uowFactory);
            var response = authService.Authenticate(sessionMock.Object, 
                new AuthenticationRequest
                {
                    Name = "Egor",
                    Password = "123",
                    Huid = "111111",
                });

            Assert.AreEqual(AuthenticationResponseType.Banned, response.Result);
        }
    }
}
