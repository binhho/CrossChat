using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Model;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;

namespace Abo.Client.Core.Managers
{
    public class FriendsManager : ManagerBase
    {
        private readonly Subject<User> _friendsSubject = new Subject<User>();
        private readonly FriendsServiceProxy _friendsServiceProxy;

        public FriendsManager(ConnectionManager connectionManager, FriendsServiceProxy friendsServiceProxy)
            : base(connectionManager)
        {
            _friendsServiceProxy = friendsServiceProxy;
        }

        /// <summary>
        /// Reloads Friends collection
        /// </summary>
        /// <returns></returns>
        public async void ReloadFriendsAsync()
        {
            var response = await _friendsServiceProxy.GetFriends(new UserFriendsRequest());
            _friendsSubject.Clear();
            if (response.Friends != null)
            {
                var friends = response.Friends.Select(ToEntity<User>).ToArray();
                Observable.Create<User>()
                foreach (var friend in friends)
                {
                    _friendsSubject.OnNext(friend);
                }
            }
        }

        /// <summary>
        /// Adds specified User to friends list
        /// </summary>
        public async Task<bool> AddToFriendsAsync(User user)
        {
            var result = await _friendsServiceProxy.AddToFriends(new AddToFriendsRequest { TargetUserId = user.Id });
            if (!result.Success)
            {
                return false;
            }
            _friendsSubject.Add(user);
            return true;
        }

        /// <summary>
        /// Removes specified User from friends list
        /// </summary>
        public async Task<bool> RemoveFromFriendsAsync(User user)
        {
            var result = await _friendsServiceProxy.RemoveFromFriends(new RemoveFromFriendsRequest { TargetUserId = user.Id });
            if (!result.Success)
            {
                return false;
            }
            //RemoveEntityFromList(_friendsSubject, i => i.Id == user.Id);
            return true;
        }

        protected override void OnUnknownDtoReceived(BaseDto dto)
        {
            var profileChanges = dto as UserPropertiesChangedInfo;
            if (profileChanges != null)
            {
                //UpdatePropertiesForList(_friendsSubject, p => p.Id == profileChanges.UserId, profileChanges.Properties);
            }

            base.OnUnknownDtoReceived(dto);
        }
    }
}