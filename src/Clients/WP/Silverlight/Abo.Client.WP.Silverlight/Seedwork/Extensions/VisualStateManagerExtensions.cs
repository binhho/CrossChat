using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Abo.Client.WP.Silverlight
{
    public static class VisualStateManagerExtensions
    {
        public static Task<bool> GoToState(this Control control, string state)
        {
            var taskSource = new TaskCompletionSource<bool>();
            var layoutRoot = VisualTreeHelper.GetChild(control, 0) as FrameworkElement;
            var visualStateGroups = VisualStateManager.GetVisualStateGroups(layoutRoot);
            var visualState = visualStateGroups.OfType<VisualStateGroup>().SelectMany(i => i.States.OfType<VisualState>()).FirstOrDefault(i => i.Name == state);

            if (visualState == null)
                throw new ArgumentException("VisualState wasn't found");

            if (visualState.Storyboard == null)
            {
                taskSource.TrySetResult(false);
                return taskSource.Task;
            }

            EventHandler storyboardCompletedHandler = null;
            storyboardCompletedHandler = (s, e) =>
                {
                    visualState.Storyboard.Completed -= storyboardCompletedHandler;
                    taskSource.TrySetResult(true);
                };

            visualState.Storyboard.Completed += storyboardCompletedHandler;
            VisualStateManager.GoToState(control, state, true);
            return taskSource.Task;
        }
    }
}
