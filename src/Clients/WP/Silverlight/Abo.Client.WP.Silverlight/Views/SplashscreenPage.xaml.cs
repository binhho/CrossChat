using System.Windows.Navigation;
using Abo.Client.WP.Silverlight.Seedwork;
using Abo.Client.WP.Silverlight.Seedwork.Navigation;

namespace Abo.Client.WP.Silverlight.Views
{
    public partial class SplashscreenPage : NavigatablePage
    {
        public SplashscreenPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ServiceLocator.Resolve<NavigationManagerBase>().StartupNavigation(NavigationContext);
        }
    }
}