using System.Linq;
using Abo.Server.Application.DataTransferObjects;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Entities;
using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Application.Services
{
    public class BlacklistService : AppService
    {
        public BlacklistService(IUnitOfWorkFactory unitOfWorkFactory,
            ITransactionFactory transactionFactory)
            : base(unitOfWorkFactory, transactionFactory)
        {
        }

        public UserBlacklistResponse GetBlacklist(ISession session, UserBlacklistRequest request)
        {
            var response = request.CreateResponse<UserBlacklistResponse>();
            response.Blacklist = session.User.PersonalBlackList.ProjectedAsCollection<UserDto>().ToArray();
            return response;
        }

        public AddToBlacklistResponse AddToBlacklist(ISession session, AddToBlacklistRequest request)
        {
            var response = request.CreateResponse<AddToBlacklistResponse>();
            response.Success = true;

            if (session.User.PersonalBlackList.Any(i => i.Id == request.TargetUserId))
            {
                response.Success = false;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                var target = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (target == null)
                {
                    response.Success = false;
                }
                else
                {
                    session.User.PersonalBlackList.Add(target);
                    uow.Commit();
                }
            }
            //TODO: notify target about it somehow
            return response;
        }

        public RemoveFromBlacklistResponse RemoveFromBlacklist(ISession session, RemoveFromBlacklistRequest request)
        {
            var response = request.CreateResponse<RemoveFromBlacklistResponse>();
            response.Success = true;

            if (session.User.PersonalBlackList.All(i => i.Id != request.TargetUserId))
            {
                response.Success = false;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                var target = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (target == null)
                {
                    response.Success = false;
                }
                else
                {
                    session.User.PersonalBlackList.Remove(target);
                    uow.Commit();
                }
            }
            //TODO: notify target about it somehow
            return response;
        }

    }
}
