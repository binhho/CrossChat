using System.Windows;
using System.Windows.Controls;

namespace Abo.Client.WP.Silverlight.Views.Battlefield
{
    public partial class ElementInfoControl : UserControl
    {
        public ElementInfoControl()
        {
            InitializeComponent();
        }

        public bool HideName
        {
            get { return (bool)GetValue(HideNameProperty); }
            set { SetValue(HideNameProperty, value); }
        }

        public static readonly DependencyProperty HideNameProperty =
            DependencyProperty.Register("HideName", typeof(bool), typeof(ElementInfoControl), new PropertyMetadata(false));
    }
}
