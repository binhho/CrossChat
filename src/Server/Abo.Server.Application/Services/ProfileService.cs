using System;
using System.Collections.Generic;
using System.Linq;
using Abo.Server.Application.Contracts;
using Abo.Server.Application.DataTransferObjects;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Services.Helpers;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Seedwork;
using Abo.Utils;

namespace Abo.Server.Application.Services
{
    public class ProfileService : AppService
    {
        private readonly IFileStorage _fileStorage;
        private readonly ISessionManager _sessionManager;
        private readonly ProfileChangesNotificator _profileChangesNotificator;

        public ProfileService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IFileStorage fileStorage,
            ISessionManager sessionManager,
            ProfileChangesNotificator profileChangesNotificator,
            ITransactionFactory transactionFactory) 
            : base(unitOfWorkFactory, transactionFactory)
        {
            _fileStorage = fileStorage;
            _sessionManager = sessionManager;
            _profileChangesNotificator = profileChangesNotificator;
        }

        public ChangePhotoResponse ChangePhoto(ISession session, ChangePhotoRequest request)//TODO: define request
        {
            var response = request.CreateResponse<ChangePhotoResponse>();
            response.Success = true;
            int photoId = request.BuiltinPhotoId;
            if (!request.PhotoData.IsNullOrEmpty() && request.PhotoData.Length > 10)
            {
                try
                {
                    photoId = _fileStorage.AppendFile(request.PhotoData);
                }
                catch (Exception exc)
                {
                    photoId = 0;
                    response.Success = false;
                }
            }
            response.PhotoId = photoId;

            if (response.Success)
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    uow.Attach(session.User);
                    session.User.ChangePhoto(photoId);

                    uow.Commit();
                }
                _profileChangesNotificator.NotifyEverybodyInChatAboutProfileChanges(session.User.Id, new Dictionary<string, object> { { "PhotoId", photoId } });
            }

            return response;
        }
    }
}
