using System;
using Abo.Server.Application.Contracts;
using Abo.Server.Application.DataTransferObjects;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Messages;
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
    public class ChatControllerTests
    {
        [Test]
        public void EveryoneInChatExceptBusyShouldReceiveTheMessage()
        {
            var playersRepository = new InMemoryUserRepository();
            var uowFactory = new UnitOfWorkFactoryMock(playersRepository, Mock.Of<IPublicMessageRepository>());
            var transactionFactoryMock = new Mock<ITransactionFactory>();

            var sessionManager = new SessionManagerMock();
            var aliceSessionMock = sessionManager.AppendPlayerAsSession("Alice");
            var bobSessionMock = sessionManager.AppendPlayerAsSession("Bob");
            var egorSessionMock = sessionManager.AppendPlayerAsSession("Egor");
            //var johnSessionMock = sessionManager.AppendPlayerAsSession("John", PlayerState.Dueling);

            var chatService = new ChatService(sessionManager, Mock.Of<ISettings>(), null, transactionFactoryMock.Object, uowFactory, Mock.Of<IPushNotificator>());
            
            chatService.PublicMessage(aliceSessionMock.Object, new PublicMessageRequest { Body = "Hi dudes!" });

            bobSessionMock.Verify(i => i.Send(It.Is<PublicMessageDto>(msg => msg.Body == "Hi dudes!" && msg.AuthorName == "Alice")), Times.Once());
            egorSessionMock.Verify(i => i.Send(It.Is<PublicMessageDto>(msg => msg.Body == "Hi dudes!" && msg.AuthorName == "Alice")), Times.Once());
            
            //John should not receive the message because he is busy (dueling or whatever)
            //johnSessionMock.Verify(i => i.Send(It.IsAny<PublicMessageDto>()), Times.Never());
        }

        [Test]
        public void DevoicedPeopleAreNotAllowedToChat()
        {
            var sessionManager = new SessionManagerMock();
            var aliceSessionMock = sessionManager.AppendPlayerAsSession("Alice");
            var bobSessionMock = sessionManager.AppendPlayerAsSession("Bob");
            var playerRepo = new InMemoryUserRepository();
            playerRepo.Add(aliceSessionMock.Object.User);
            playerRepo.Add(bobSessionMock.Object.User);

            //grant moderatorship to Bob
            bobSessionMock.Object.User.GrantModeratorship();

            var uowFactory = new UnitOfWorkFactoryMock(playerRepo, Mock.Of<IPublicMessageRepository>());
            var chatService = new ChatService(sessionManager, Mock.Of<ISettings>(), null, Mock.Of<ITransactionFactory>(), uowFactory, Mock.Of<IPushNotificator>());

            //Alice sends a message - everything is ok
            chatService.PublicMessage(aliceSessionMock.Object, new PublicMessageRequest {Body = "F&ck you!"});
            //Make sure Bob has received the message
            bobSessionMock.Verify(i => i.Send(It.Is<PublicMessageDto>(msg => msg.Body == "F&ck you!")), Times.Once());
            //Bob devoices Alice because of foul language
            chatService.Devoice(bobSessionMock.Object, new DevoiceRequest { Reason = "Foul language", TargetUserId = aliceSessionMock.Object.User.Id});
            bobSessionMock.Verify(i => i.Send(It.Is<DevoiceResponse>(response => response.Result == DevoiceResponseType.Success)), Times.Once());
            //Alice doesn't beleave that she is devoiced and tries to send a message
            chatService.PublicMessage(aliceSessionMock.Object, new PublicMessageRequest { Body = "Am I really devoiced?" });
            //Make sure Bob hasn't receive the message
            bobSessionMock.Verify(i => i.Send(It.Is<PublicMessageDto>(msg => msg.Body == "Am I really devoiced?")), Times.Never());
            //Server will send a system message notifying User that he is devoiced.   
        }

        [Test]
        public void OnlyModeratorsCanDevoice()
        {
            var sessionManager = new SessionManagerMock();
            var aliceSessionMock = sessionManager.AppendPlayerAsSession("Alice");
            var bobSessionMock = sessionManager.AppendPlayerAsSession("Bob");
            var playerRepo = new InMemoryUserRepository();
            playerRepo.Add(aliceSessionMock.Object.User);
            playerRepo.Add(bobSessionMock.Object.User);

            //neither Bob nor Alice are moderators

            var uowFactory = new UnitOfWorkFactoryMock(playerRepo, Mock.Of<IPublicMessageRepository>());
            var chatService = new ChatService(sessionManager, Mock.Of<ISettings>(), null, Mock.Of<ITransactionFactory>(), uowFactory, Mock.Of<IPushNotificator>());

            chatService.Devoice(bobSessionMock.Object, new DevoiceRequest { Reason = "Just for fun", TargetUserId = aliceSessionMock.Object.User.Id });
            bobSessionMock.Verify(i => i.Send(It.Is<DevoiceResponse>(response => response.Result == DevoiceResponseType.Failed)), Times.Once());
        }
    }
}
