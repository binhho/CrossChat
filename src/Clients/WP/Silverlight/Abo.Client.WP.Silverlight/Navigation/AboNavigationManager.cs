using System;
using System.Windows.Navigation;
using Abo.Client.Core.Managers;
using Abo.Client.WP.Silverlight.Seedwork;
using Abo.Client.WP.Silverlight.Seedwork.Navigation;
using Abo.Client.WP.Silverlight.ViewModels;
using Abo.Client.WP.Silverlight.Views;

namespace Abo.Client.WP.Silverlight.Navigation
{
    public class AboNavigationManager : NavigationManagerBase
    {
        private readonly AccountManager _accountManager;

        public AboNavigationManager(NavigationBuilder navigationBuilder, 
            PopupController popupController,
            AccountManager accountManager)
            : base(navigationBuilder, popupController)
        {
            _accountManager = accountManager;
        }

        public override Type[] RootViews
        {
            get 
            { 
                return new[]
                       {
                           typeof (MainMenuPage),
                           typeof (RegistrationPage),
                       }; 
            }
        }

        public override void StartupNavigation(NavigationContext navigationContext)
        {
            if (!_accountManager.IsRegistered)
            {
                ServiceLocator.Resolve<RegistrationViewModel>().Show();
            }
            else
            {
                ServiceLocator.Resolve<MainMenuViewModel>().Show();
            }
        }

        public override string ViewsFolder
        {
            get { return "/Views"; }
        }
    }
}
