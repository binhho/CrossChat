using System.Linq;
using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Model;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Utils.Collections;

namespace Abo.Client.Core.Managers
{
    public class BlacklistManager : ManagerBase
    {
        private readonly ExtendedCcObservableCollection<User> _blacklistSubject = null;
        private readonly BlacklistServiceProxy _blacklistServiceProxy;

        public BlacklistManager(ConnectionManager connectionManager,
            BlacklistServiceProxy blacklistServiceProxy) : base(connectionManager)
        {
            _blacklistServiceProxy = blacklistServiceProxy;
            Blacklist = _blacklistSubject = new ExtendedCcObservableCollection<User>();
        }

        /// <summary>
        /// Active Blacklist
        /// </summary>
        public ICcRoObservableCollection<User> Blacklist { get; private set; }

        /// <summary>
        /// Reloads Blacklist collection
        /// </summary>
        /// <returns></returns>
        public async Task ReloadBlacklistAsync()
        {
            var response = await _blacklistServiceProxy.GetBlacklist(new UserBlacklistRequest());
            _blacklistSubject.Clear();
            if (response.Blacklist != null)
            {
                var blacklist = response.Blacklist.Select(ToEntity<User>).ToArray();
                _blacklistSubject.AddRange(blacklist);
            }
        }

        /// <summary>
        /// Adds specified User to Blacklist
        /// </summary>
        public async Task<bool> AddToBlacklistAsync(User user)
        {
            var result = await _blacklistServiceProxy.AddToBlacklist(new AddToBlacklistRequest { TargetUserId = user.Id });
            if (!result.Success)
            {
                return false;
            }
            _blacklistSubject.Add(user);
            return true;
        }

        /// <summary>
        /// Removes specified User from Blacklist
        /// </summary>
        public async Task<bool> RemoveFromBlacklistAsync(User user)
        {
            var result = await _blacklistServiceProxy.RemoveFromBlacklist(new RemoveFromBlacklistRequest { TargetUserId = user.Id });
            if (!result.Success)
            {
                return false;
            }
            RemoveEntityFromList(_blacklistSubject, i => i.Id == user.Id);
            return true;
        }

        protected override void OnUnknownDtoReceived(BaseDto dto)
        {
            var userPropertiesChangedInfo = dto as UserPropertiesChangedInfo;
            if (userPropertiesChangedInfo != null)
            {
                UpdatePropertiesForList(_blacklistSubject, p => p.Id == userPropertiesChangedInfo.UserId, userPropertiesChangedInfo.Properties);
            }

            base.OnUnknownDtoReceived(dto);
        }
    }
}