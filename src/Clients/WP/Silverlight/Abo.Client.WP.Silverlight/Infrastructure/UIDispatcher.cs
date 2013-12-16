using System;
using System.Windows;
using Abo.Utils;

namespace Abo.Client.WP.Silverlight.Infrastructure
{
    public class UIDispatcher : IThreadDispatcher
    {
        private static IThreadDispatcher _current = null;

        public static IThreadDispatcher Current
        {
            get { return _current ?? (_current = new UIDispatcher()); }
        }

        public void Dispatch(Action action)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }

        public void Dispatch<T>(Action<T> action, T state)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action, state);
        }

        public void DispatchIfNeeded(Action action)
        {
            if (Deployment.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatch(action);
            }
        }
    }
}
