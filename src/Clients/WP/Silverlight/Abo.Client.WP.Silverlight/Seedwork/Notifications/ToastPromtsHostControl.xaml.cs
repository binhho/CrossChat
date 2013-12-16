using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Abo.Client.WP.Silverlight.Seedwork.Notifications
{
    public partial class ToastPromtsHostControl
    {
        public const int ToastTimeoutSeconds = 4;
        public const int MaxToastCount = 3;

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Queue<NotificationItem> _notificationQueue = new Queue<NotificationItem>();
        private readonly List<ToastItem> _toastItems = new List<ToastItem>();
        private static ToastPromtsHostControl _lastUsedInstance = null;

        public ToastPromtsHostControl()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_OnTick;
            _timer.Start();
            _lastUsedInstance = this;
        }

        private void _timer_OnTick(object sender, EventArgs e)
        {
            foreach (ToastItem toastItem in _toastItems.ToArray())
            {
                if (!toastItem.IsFinalizing && toastItem.Started.AddSeconds(ToastTimeoutSeconds) <= DateTime.Now)
                {
                    RemoveToast(toastItem);
                }
            }
        }

        private void UpdateVisibility()
        {
            if (Visibility == Visibility.Visible)
            {
                if (!_toastItems.Any() && !_notificationQueue.Any())
                    Visibility = Visibility.Collapsed;
            }
            else
            {
                if (_toastItems.Any() || _notificationQueue.Any())
                    Visibility = Visibility.Visible;
            }
        }

        private void RemoveToast(ToastItem toastItem)
        {
            if (toastItem.IsFinalizing)
                return;

            toastItem.IsFinalizing = true;
            toastItem.FinalizingActions();

            var storyboard = new Storyboard();
            var projectionAnimation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.6)), To = 90 };
            storyboard.Children.Add(projectionAnimation);
            Storyboard.SetTargetProperty(projectionAnimation, new PropertyPath("(UIElement.Projection).(PlaneProjection.RotationX)"));
            Storyboard.SetTarget(projectionAnimation, toastItem.Element);
            var item = toastItem;

            EventHandler completedHandler = null;
            completedHandler = (s, ea) =>
            {
                _toastItems.Remove(item);
                ActiveToastList.Items.Remove(item.Element);
                UpdateVisibility();
                TryDequeue();
                storyboard.Completed -= completedHandler;
            };

            storyboard.Completed += completedHandler;
            storyboard.Begin();
        }

        private void TryDequeue()
        {
            if (_toastItems.Count < MaxToastCount && _notificationQueue.Any())
            {
                var item = _notificationQueue.Dequeue();
                UpdateVisibility();
                AppendToast(item);
                TryDequeue();
            }
        }

        public static void EnqueueItem(UIElement content, Action submitAction, Color bgColor)
        {
            if (_lastUsedInstance == null)
                return;
            _lastUsedInstance._notificationQueue.Enqueue(new NotificationItem(content, bgColor, submitAction));
            _lastUsedInstance.TryDequeue();
        }

        public static void Clear()
        {
            if (_lastUsedInstance == null)
                return;
            _lastUsedInstance.ClearAll();
        }

        private void ClearAll()
        {
            _notificationQueue.Clear();
            _toastItems.ForEach(RemoveToast);
        }

        private void AppendToast(NotificationItem notification)
        {
            //root layout
            var layoutGrid = new Grid();
            var toastItem = new ToastItem(layoutGrid, DateTime.Now, notification);
            layoutGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            layoutGrid.Height = 65;
            layoutGrid.Margin = new Thickness(0, 0, 0, 4);
            layoutGrid.Background = new SolidColorBrush(notification.Color);
            layoutGrid.Projection = new PlaneProjection();
            layoutGrid.RenderTransformOrigin = new Point(0.5, 0.5);
            layoutGrid.RenderTransform = new CompositeTransform { TranslateX = -800 };
            layoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            layoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(65.0) });

            //close button
            var closeButton = new Button();
            Grid.SetColumn(closeButton, 1);
            closeButton.Tag = toastItem;
            closeButton.Width = 80;
            closeButton.Height = 90;
            closeButton.Content = "\u2716";
            closeButton.BorderThickness = new Thickness(0);
            closeButton.FontSize = 32;
            closeButton.Padding = new Thickness(0);
            closeButton.Opacity = 0.4;
            closeButton.Margin = new Thickness(-6, -12, -10, -8);
            closeButton.FontSize = 32;
            closeButton.HorizontalAlignment = HorizontalAlignment.Right;
            closeButton.Tap += closeButton_OnTap;
            layoutGrid.Children.Add(closeButton);

            //toast content
            var contentGrid = new Grid();
            contentGrid.Tag = toastItem;
            contentGrid.Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));

            contentGrid.Children.Add(notification.Content);
            layoutGrid.Children.Add(contentGrid);
            contentGrid.Tap += contentGrid_OnTap;

            toastItem.FinalizingActions += () => contentGrid.Tap -= contentGrid_OnTap;
            toastItem.FinalizingActions += () => closeButton.Tap -= closeButton_OnTap;

            //appear animation
            var animation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.2)), To = 0 };
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, layoutGrid);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));
            storyboard.Begin();

            ActiveToastList.Items.Add(layoutGrid);
            _toastItems.Add(toastItem);
        }

        private void contentGrid_OnTap(object sender, GestureEventArgs e)
        {
            var toastItem = ((FrameworkElement)sender).Tag as ToastItem;
            if (toastItem == null)
                return;

            if (!toastItem.IsFinalizing)
            {
                RemoveToast(toastItem);
                if (toastItem.NotificationItem.Action != null)
                    toastItem.NotificationItem.Action();
            }
        }

        private void closeButton_OnTap(object sender, GestureEventArgs e)
        {
            var toastItem = ((FrameworkElement)sender).Tag as ToastItem;
            if (toastItem == null)
                return;

            RemoveToast(toastItem);
        }

        private class ToastItem
        {
            public FrameworkElement Element { get; set; }
            public DateTime Started { get; set; }
            public bool IsFinalizing { get; set; }
            public NotificationItem NotificationItem { get; set; }
            public Action FinalizingActions { get; set; }

            public ToastItem(FrameworkElement element, DateTime started, NotificationItem notification)
            {
                Element = element;
                Started = started;
                NotificationItem = notification;
            }
        }

        private class NotificationItem
        {
            public UIElement Content { get; set; }
            public Color Color { get; set; }
            public Action Action { get; set; }

            public NotificationItem(UIElement content, Color color, Action action)
            {
                Content = content;
                Color = color;
                Action = action;
            }
        }
    }
}
