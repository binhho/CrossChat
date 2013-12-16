using System;
using System.Runtime.CompilerServices;
using Abo.Client.Wpf.Infrastructure;
using GalaSoft.MvvmLight;

namespace Abo.Client.Wpf.ViewModels
{
    public class AboViewModelBase : ViewModelBase
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChangedToUIThread(string property)
        {
            UIDispatcher.Current.DispatchIfNeeded(() => RaisePropertyChanged(property));
        }
    }
}
