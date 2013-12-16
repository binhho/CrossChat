using System.Windows;
using System.Windows.Controls;

namespace Abo.Client.WP.Silverlight.Seedwork.Controls
{
    public partial class ProgressLine : UserControl
    {
        public ProgressLine()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ProgressLine_Loaded);
        }

        void ProgressLine_Loaded(object sender, RoutedEventArgs e)
        {
            OnPercentageChanged();
        }

        public double ProgressWidth
        {
            get { return (double)GetValue(ProgressWidthProperty); }
            set { SetValue(ProgressWidthProperty, value); }
        }

        public static readonly DependencyProperty ProgressWidthProperty =
            DependencyProperty.Register("ProgressWidth", typeof(double), typeof(ProgressLine), new PropertyMetadata(0.0));

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(ProgressLine), new PropertyMetadata(0.0, OnPercentageStaticChanged));

        private static void OnPercentageStaticChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d as ProgressLine != null)
            {
                ((ProgressLine) d).OnPercentageChanged();
            }
        }

        private void OnPercentageChanged()
        {
            ProgressWidth = Percentage*ActualWidth/100;
        }
    }
}
