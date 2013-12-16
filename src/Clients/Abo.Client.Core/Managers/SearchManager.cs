using System.Linq;
using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Model;
using Abo.Server.Application.DataTransferObjects.Requests;


namespace Abo.Client.Core.Managers
{
    public class SearchManager : ManagerBase
    {
        private readonly UsersSearchServiceProxy _searchServiceProxy;

        public SearchManager(ConnectionManager connectionManager, UsersSearchServiceProxy searchServiceProxy)
            : base(connectionManager)
        {
            _searchServiceProxy = searchServiceProxy;
        }

        public async Task<User[]> SearchAsync(string query)
        {
            UsersSearchResponse response = await _searchServiceProxy.SearchUser(new UsersSearchRequest{ QueryString = query });
            if (response.Result == null)
                return new User[0];

            return response.Result.Select(ToEntity<User>).ToArray();
        }

        public Task<UserDetails> GetDetails(string name)
        {
            return GetDetails(0, name);
        }

        public Task<UserDetails> GetDetails(int id)
        {
            return GetDetails(id, null);
        }

        private async Task<UserDetails> GetDetails(int id, string name)
        {
            var response = await _searchServiceProxy.GetUserDetails(new GetUserDetailsRequest() { UserId = id, Name = name });
            if (response.User == null)
                return null;
            var user = ToEntity<User>(response.User);
            return new UserDetails
                        {
                            User = user,
                            IsInMyBlacklist = response.IsInBlacklist,
                            IsMyFriend = response.IsFriend,
                        };
        }
    }
}
