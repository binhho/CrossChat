using System;
using Abo.Server.Application.DataTransferObjects;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Entities;
using Abo.Server.Domain.Exceptions;
using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Application.Services
{
    /// <summary>
    /// </summary>
    public class RegistrationService : AppService
    {
        public RegistrationService(
            ITransactionFactory transactionFactory,
            IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory, transactionFactory)
        {
        }

        /// <summary>
        /// </summary>
        public RegistrationResponse RegisterNewPlayer(ISession session, RegistrationRequest request)
        {
            var response = request.CreateResponse(new RegistrationResponse { Result = RegistrationResponseType.Success });
            User user = null;
            
            try
            {
                user = new User(request.Name, request.Password, request.Huid, request.Sex, request.Age, request.PushUri, request.Country);
                user.ChangePhoto(request.PhotoId);
            }
            catch (InvalidPlayerRegistrationDataException)
            {
                response.Result = RegistrationResponseType.InvalidData;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                if (uow.UsersRepository.AnyMatching(UserSpecification.Name(user.Name)))
                {
                    response.Result = RegistrationResponseType.NameIsInUse;
                    return response;
                }
                
                var playerWithSameHuid = uow.UsersRepository.FirstMatching(UserSpecification.Huid(request.Huid));
                if (playerWithSameHuid != null && playerWithSameHuid.IsBanned)
                {
                    response.Result = RegistrationResponseType.Banned;
                    return response;
                }

                uow.UsersRepository.Add(user);
                uow.Commit();
            }
            response.User = user.ProjectedAs<UserDto>();
            return response;
        }

        /// <summary>
        /// </summary>
        public ResponseBase Deactivate(ISession session, DeactivationRequest request)
        {
            var response = request.CreateResponse();
            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                session.User.Delete();
                uow.Commit();
            }
            return response;
        }
    }
}
