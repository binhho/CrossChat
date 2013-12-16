using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Abo.Client.Wpf.ViewModels;

namespace Abo.Client.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private async void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);
            Keyboard.Focus(InputBox);
        }
    }
}
