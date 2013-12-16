using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace Abo.Client.WP.Silverlight.Seedwork.Navigation
{
    public class PopupController
    {
        private Popup _popup = null;
        private PhoneApplicationFrame _frame;

        public PopupController()
        {            
            _frame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (_frame != null)
            {
                _frame.BackKeyPress += frame_BackKeyPress;
                _frame.OrientationChanged += _frame_OrientationChanged;
            }
        }

        private void _frame_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if (_popup != null && _popup.Child is FrameworkElement && _frame != null)
            {
                var container = _popup.Child as FrameworkElement;
                container.Width = _frame.ActualWidth;
                container.Height = _frame.ActualHeight;
            }
        }

        public void DisplayPopup(FrameworkElement element, INavigatableViewModel viewModel)
        {
            if (_frame == null || element == null || viewModel == null)
                return;

            Popup popup = new Popup();

            popup.DataContext = viewModel;

            viewModel.CloseRequested += viewModel_CloseRequested;

            Grid grid = new Grid();
            grid.Name = "PopupRootLayout";
            element.Tap += element_Tap;
            grid.Tap += grid_Tap;
            grid.Width = _frame.ActualWidth;
            grid.Height = _frame.ActualHeight;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.Background = new SolidColorBrush(Color.FromArgb(145, 0, 0, 0));
            element.VerticalAlignment = VerticalAlignment.Top;
            element.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(element);

            popup.Closed += (s, e) =>
            {
                viewModel.CloseRequested -= viewModel_CloseRequested;
                grid.Tap -= grid_Tap;
                element.Tap -= element_Tap;
                viewModel.OnClose();
                _popup = null;
            };

            popup.Child = grid;
            popup.IsOpen = true;
            _popup = popup;
        }

        void element_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            e.Handled = true;
        }

        private void grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var grid = sender as Grid;
            if (grid != null && grid.Name == "PopupRootLayout" && _popup != null && _popup.IsOpen)
            {
                _popup.IsOpen = false;
            }
        }

        private void viewModel_CloseRequested(object sender, EventArgs e)
        {
            if (_popup != null && _popup.DataContext != null && _popup.DataContext.Equals(sender) && _popup.IsOpen)
            {
                _popup.IsOpen = false;
            }
        }

        private void frame_BackKeyPress(object sender, CancelEventArgs e)
        {
            if (_popup != null && _popup.IsOpen)
            {
                e.Cancel = true;
                _popup.IsOpen = false;
            }
        }
    }
}
