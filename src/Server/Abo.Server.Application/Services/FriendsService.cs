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
    public class FriendsService : AppService
    {
        public FriendsService(IUnitOfWorkFactory unitOfWorkFactory, 
            ITransactionFactory transactionFactory) 
            : base(unitOfWorkFactory, transactionFactory)
        {
        }

        public UserFriendsResponse GetFriends(ISession session, UserFriendsRequest request)
        {
            var response = request.CreateResponse<UserFriendsResponse>();
            response.Friends = session.User.Friends.ProjectedAsCollection<UserDto>().ToArray();
            return response;
        }

        public AddToFriendsResponse AddToFriends(ISession session, AddToFriendsRequest request)
        {
            var response = request.CreateResponse<AddToFriendsResponse>();
            response.Success = true;

            if (session.User.Friends.Any(i => i.Id == request.TargetUserId))
            {
                response.Success = false;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                var friend = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (friend == null)
                {
                    response.Success = false;
                }
                else
                {
                    session.User.Friends.Add(friend);
                    uow.Commit();
                }
            } 
            return response;
        }

        public RemoveFromFriendsResponse RemoveFromFriends(ISession session, RemoveFromFriendsRequest request)
        {
            var response = request.CreateResponse<RemoveFromFriendsResponse>();
            response.Success = true;
            
            if (session.User.Friends.All(i => i.Id != request.TargetUserId))
            {
                response.Success = false;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                var friend = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (friend == null)
                {
                    response.Success = false;
                }
                else
                {
                    session.User.Friends.Remove(friend);
                    uow.Commit();
                }
            } 
            return response;
        }

    }
}
