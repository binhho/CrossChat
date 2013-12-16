using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;

namespace Abo.Client.Core.Managers
{
    public abstract class ManagerBase
    {
        protected ManagerBase(ConnectionManager connectionManager)
        {
            if (connectionManager != null)
            {
                ConnectionManager = connectionManager;
                ConnectionManager.DtoReceived += _connectionManager_OnDtoReceived;
                ConnectionManager.RequestReceived += ConnectionManager_OnRequestReceived;
            }
        }

        protected ConnectionManager ConnectionManager { get; private set; }

        protected virtual void OnUnknownDtoReceived(BaseDto dto) { }

        protected virtual Task<ResponseBase> OnRequest(RequestBase request) { return Task.FromResult<ResponseBase>(null); }

        protected void UpdateProperties<T>(T instance, PropertyValuePair[] properties)
        {
            var type = instance.GetType().GetTypeInfo();
            foreach (var propertyValuePair in properties)
            {
                var propertyType = type.GetDeclaredProperty(propertyValuePair.Property);
                propertyType.SetValue(instance, Convert.ChangeType(propertyValuePair.Value, propertyType.PropertyType));
            }
        }

        protected TEntity ToEntity<TEntity>(BaseDto dto)
        {
            var entity = Activator.CreateInstance<TEntity>();
            AutoMapper.CopyPropertyValues(dto, entity);
            return entity;
        }

        private void _connectionManager_OnDtoReceived(object sender, DtoEventArgs e)
        {
            OnUnknownDtoReceived(e.Dto);
        }

        private async void ConnectionManager_OnRequestReceived(object sender, RequestEventArgs e)
        {
            var response = await OnRequest(e.Request);
            ConnectionManager.SendResponse(response);
        }
    }
}
