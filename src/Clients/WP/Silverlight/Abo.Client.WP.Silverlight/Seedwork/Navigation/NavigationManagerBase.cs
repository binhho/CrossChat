using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Abo.Client.WP.Silverlight.Seedwork.Navigation
{
    /// <summary>
    /// Base class for navigation manager
    /// </summary>
    public abstract class NavigationManagerBase
    {
        private readonly NavigationBuilder _navigationBuilder;
        private readonly PopupController _popupController;
        private const string ViewModelIdArgumentName = "viewModelId"; 

        protected NavigationManagerBase(NavigationBuilder navigationBuilder, PopupController popupController)
        {
            _navigationBuilder = navigationBuilder;
            _popupController = popupController;
        }

        /// <summary>
        /// Root views, navigation on these pages will clean navigation stack
        /// </summary>
        public abstract Type[] RootViews { get; }

        /// <summary>
        /// Starts navigation
        /// </summary>
        public abstract void StartupNavigation(NavigationContext navigationContext);

        public abstract string ViewsFolder { get; }

        /// <summary>
        /// Navigates to page associated with specified view model
        /// </summary>
        /// <param name="viewModel"></param>
        public void Navigate(object viewModel)
        {
            var page = _navigationBuilder.ViewsMap[viewModel.GetType()];

            var frame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (frame == null)
                throw new InvalidOperationException("Frame is null");

            if (frame.Content is FrameworkElement)
            {
                var dc = ((FrameworkElement) frame.Content).DataContext;
                if (dc != null && dc.Equals(viewModel))
                    return;
            }

            if (!page.IsSubclassOf(typeof (Page)))
            {
                _popupController.DisplayPopup(Activator.CreateInstance(page) as FrameworkElement, viewModel as INavigatableViewModel);
                return;
            }

            bool navigationResult = frame.Navigate(GetPageUri(page, ViewModelsRegistry.Register(viewModel)));
            if (!navigationResult)
            {
                //TODO: log and do smth
            }

            bool isRootPage = RootViews.Contains(page);
            if (isRootPage)
            {
                while (frame.BackStack.Any())
                {
                    var entry = frame.RemoveBackEntry();
                    //TODO: log entry.Source;
                }
            }
        }

        internal static object GetDataContext(NavigationContext navContext)
        {
            string id;
            if (navContext != null && navContext.QueryString != null &&
                navContext.QueryString.TryGetValue(ViewModelIdArgumentName, out id))
            {
                return ViewModelsRegistry.GetAndDeregister(id);
            }
            return null;
        }

        private Uri GetPageUri(Type page, string viewModelId)
        {
            return new Uri(string.Format("{3}/{0}.xaml?{1}={2}", page.Name, ViewModelIdArgumentName, viewModelId, ViewsFolder), UriKind.Relative);
        }
    }
}
