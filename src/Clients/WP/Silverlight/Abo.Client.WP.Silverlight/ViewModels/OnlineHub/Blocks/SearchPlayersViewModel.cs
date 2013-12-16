using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Abo.Client.Core.Infrastructure;
using Abo.Client.Core.Managers;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks
{
    public class SearchPlayersViewModel : OnlineHubViewModelBase
    {
        private readonly SearchManager _searchManager;
        private string _query;
        private IEnumerable<PlayerViewModel> _result;

        public SearchPlayersViewModel(SearchManager searchManager)
        {
            _searchManager = searchManager;
        }

        protected override async Task OnReload()
        {
            Query = string.Empty;
            Result = null;
        }

        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        }

        public ICommand SearchCommand { get { return new RelayCommand(Search); } }

        private async void Search()
        {
            Result = null;
            if (string.IsNullOrWhiteSpace(Query))
                return;
            IsLoading = true;
            var result = await _searchManager.SearchAsync(Query);
            await Task.Delay(1500);
            Result = result.Select(i => new PlayerViewModel(i));
            IsLoading = false;
        }

        public IEnumerable<PlayerViewModel> Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }
    }
}