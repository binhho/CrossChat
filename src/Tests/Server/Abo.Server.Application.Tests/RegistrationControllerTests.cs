using System.Linq;
using Abo.Server.Application.DataTransferObjects;
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
    public class RegistrationControllerTests
    {
        [Test]
        public void ShouldntAllowRegisterWithNameThatAlreadyInUse()
        {
            var transactionFactoryMock = new Mock<ITransactionFactory>();

            var playersRepository = new InMemoryUserRepository();
            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());
            playersRepository.Add(new Domain.Entities.User("Egor", "123", "111111", true, 13, string.Empty, "Belarus"));

            var sessionMock = new Mock<ISession>();
            var registrationService = new RegistrationService(transactionFactoryMock.Object, uowFactory);

            var response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 20,
                        Comments = "Comments",
                        Country = "Russia",
                        Name = "Egor",
                        Password = "PSW",
                        PushUri = "",
                        Sex = true,
                        Huid = "HUID123", 
                    });

            Assert.AreEqual(RegistrationResponseType.NameIsInUse, response.Result);
        }

        [Test]
        public void ShouldSuccessfullyRegisterPlayerWithValidData()
        {
            var transactionFactoryMock = new Mock<ITransactionFactory>();

            var playersRepository = new InMemoryUserRepository();
            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());
            playersRepository.Add(new Domain.Entities.User("Egor", "123", "111111", true, 13, string.Empty, "Belarus"));

            var sessionMock = new Mock<ISession>();

            var registrationService = new RegistrationService(transactionFactoryMock.Object, uowFactory);

            var response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 24,
                        Huid = "HUID123", 
                        Password = "PSW",
                        PushUri = "",
                        Sex = true,
                        Comments = "Comments",
                        Country = "Russia",
                        Name = "John",
                    });

            Assert.AreEqual(RegistrationResponseType.Success, response.Result);
        }

        [Test]
        public void ShouldntAllowRegisterWithInvalidData()
        {
            var transactionFactoryMock = new Mock<ITransactionFactory>();

            var playersRepository = new InMemoryUserRepository();
            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());

            var sessionMock = new Mock<ISession>();

            var registrationService = new RegistrationService(transactionFactoryMock.Object, uowFactory);

            //trying to register with empty name
            var response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 20,
                        Comments = "Comments",
                        Country = "Russia",
                        Name = "",
                        Huid = "111",
                        Password = "12324"
                    });

            Assert.AreEqual(RegistrationResponseType.InvalidData, response.Result);

            //trying to register with extremly long name
            response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 20,
                        Comments = "Comments",
                        Country = "Russia",
                        Huid = "111",
                        Name = string.Join("", Enumerable.Range(1, 1000)),
                        Password = "12324"
                    });

            Assert.AreEqual(RegistrationResponseType.InvalidData, response.Result);

            //trying to register with empty psw
            response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 20,
                        Comments = "Comments",
                        Country = "Russia",
                        Name = "Egor",
                        Huid = "111",
                        Password = ""
                    });

            Assert.AreEqual(RegistrationResponseType.InvalidData, response.Result);

            //trying to register with extremly long psw
            response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 20,
                        Comments = "Comments",
                        Country = "Russia",
                        Name = "Egor",
                        Huid = "111",
                        Password = string.Join("", Enumerable.Range(1, 1000))
                    });

            Assert.AreEqual(RegistrationResponseType.InvalidData, response.Result);

            //trying to register with empty country
            response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 20,
                        Comments = "Comments",
                        Country = "",
                        Name = "Egor",
                        Password = "Egor",
                        Huid = "111"
                    });

            Assert.AreEqual(RegistrationResponseType.InvalidData, response.Result);

            //trying to register with empty huid
            response = registrationService.RegisterNewPlayer(sessionMock.Object, 
                new RegistrationRequest
                    {
                        Age = 20,
                        Comments = "Comments",
                        Country = "Belarus",
                        Name = "Egor",
                        Password = "Egor",
                        Huid = ""
                    });

            Assert.AreEqual(RegistrationResponseType.InvalidData, response.Result);

        }
    }
}
