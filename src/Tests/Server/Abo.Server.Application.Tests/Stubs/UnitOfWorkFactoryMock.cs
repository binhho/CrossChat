using Abo.Server.Domain.Repositories;
using Abo.Server.Domain.Seedwork;
using Moq;

namespace Abo.Server.Application.Tests.Stubs
{
    public class UnitOfWorkFactoryMock : IUnitOfWorkFactory
    {
        private readonly IUserRepository _staticUserRepository;
        private readonly IPublicMessageRepository _publicMessageRepository;

        public UnitOfWorkFactoryMock(IUserRepository staticUserRepository, IPublicMessageRepository publicMessageRepository)
        {
            _staticUserRepository = staticUserRepository;
            _publicMessageRepository = publicMessageRepository;
        }

        public IUnitOfWork Create()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.SetupGet(i => i.UsersRepository).Returns(_staticUserRepository);
            uow.SetupGet(i => i.PublicMessageRepository).Returns(_publicMessageRepository);
            return uow.Object;
        }

    }
}
