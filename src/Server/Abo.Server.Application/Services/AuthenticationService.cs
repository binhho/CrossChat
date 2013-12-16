using System.Linq;
using Abo.Server.Application.Contracts;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Entities;
using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Application.Services
{
    public class AuthenticationService : AppService
    {
        private readonly ISettings _settings;

        public AuthenticationService(
            ISettings settings,
            ITransactionFactory transactionFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(unitOfWorkFactory, transactionFactory)
        {
            _settings = settings;
        }

        public AuthenticationResponse Authenticate(ISession session, AuthenticationRequest request)
        {
            var response = request.CreateResponse<AuthenticationResponse>();
            if (_settings.IsServerBusy)
            {
                response.Result = AuthenticationResponseType.ServerIsBusy;
                return response;
            }
            
            response.Result = AuthenticationResponseType.Success;

            using (var uow = UnitOfWorkFactory.Create())
            {
                var user = uow.UsersRepository.FirstMatching(UserSpecification.NameAndPassword(request.Name, request.Password));
                if (user == null)
                {
                    response.Result = AuthenticationResponseType.InvalidNameOrPassword;
                }
                else
                {
                    if (user.IsBanned)
                    {
                        response.Result = AuthenticationResponseType.Banned;
                    }
                    else if (user.Huid != request.Huid)
                    {
                        user.ChangeHuid(request.Huid);
                    }
                }
                
                uow.Commit();
                
                if (response.Result == AuthenticationResponseType.Success)
                {
                    //yes, it's ugly and looks like we have leaking abstraction (we do actually) but IMO it's the most harmless way to load linked data
                    user.Friends.Count();
                    user.PersonalBlackList.Count();
                    session.SetUser(user);
                    response.User = user.ProjectedAs<UserDto>();
                }
            }
            return response;
        }
    }
}
