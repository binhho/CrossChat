using System;
using System.Windows;
using System.Windows.Threading;
using Abo.Utils.Extensions;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Info;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class FrameViewModel : ViewModelBase
    {
        private bool _isBusy;
        private string _memoryInfo;
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Deployment.Current.Dispatcher.BeginInvoke(() => Set("IsBusy", ref _isBusy, value)); }
        }

        public bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#endif
                return false;
            }
        }

        public FrameViewModel()
        {
#if DEBUG
            _timer.Tick += _timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
#endif
        }


        private void _timer_Tick(object sender, EventArgs e)
        {
            var usage = StringExtensions.ToShortSizeInBytesString(DeviceStatus.ApplicationCurrentMemoryUsage);
            var peak = StringExtensions.ToShortSizeInBytesString(DeviceStatus.ApplicationPeakMemoryUsage);
            var limit = StringExtensions.ToShortSizeInBytesString(DeviceStatus.ApplicationMemoryUsageLimit);
            MemoryInfo = string.Format("{0}; {1}; {2};", usage, peak, limit);
        }

        public string MemoryInfo
        {
            get { return _memoryInfo; }
            set { Set("MemoryInfo", ref _memoryInfo, value); }
        }
    }
}