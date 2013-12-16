using System;
using System.Windows;
using Abo.Utils;

namespace Abo.Client.Wpf.Infrastructure
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
            Application.Current.Dispatcher.BeginInvoke(action);
        }

        public void Dispatch<T>(Action<T> action, T state)
        {
            Application.Current.Dispatcher.BeginInvoke(action, state);
        }

        public void DispatchIfNeeded(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
