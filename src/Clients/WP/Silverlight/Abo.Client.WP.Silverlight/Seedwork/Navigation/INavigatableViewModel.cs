using System;

namespace Abo.Client.WP.Silverlight.Seedwork.Navigation
{
    /// <summary>
    /// Make sure that your viewmodel base class implements this interface in order to handle OnNavigatedTo event
    /// </summary>
    public interface INavigatableViewModel
    {
        void OnNavigatedTo();
        void RequestClose();
        event EventHandler CloseRequested;
        void OnClose();
    }
}
