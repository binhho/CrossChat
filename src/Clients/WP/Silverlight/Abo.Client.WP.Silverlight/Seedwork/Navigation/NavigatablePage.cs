using System.Windows;
using Microsoft.Phone.Controls;

namespace Abo.Client.WP.Silverlight.Seedwork.Navigation
{
    /// <summary>
    /// Base class for page that support mvvm navigation
    /// </summary>
    public class NavigatablePage : PhoneApplicationPage
    {
        public NavigatablePage()
        {
            Style = Application.Current.Resources["TransitionPageStyle"] as Style;
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var viewModel = NavigationManagerBase.GetDataContext(NavigationContext);
            if (viewModel != null)
            {
                if (viewModel is INavigatableViewModel)
                    ((INavigatableViewModel) viewModel).OnNavigatedTo();
                DataContext = viewModel;
            }
            else
            {
                if (DataContext is INavigatableViewModel)
                    ((INavigatableViewModel)DataContext).OnNavigatedTo();
            }
        }
    }
}
