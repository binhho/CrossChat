using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Abo.Client.WP.Silverlight.Seedwork.Controls
{
    public partial class WebImage : UserControl
    {
        public WebImage()
        {
            InitializeComponent();
        }

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public string DefaultImageUrl
        {
            get { return (string)GetValue(DefaultImageUrlProperty); }
            set { SetValue(DefaultImageUrlProperty, value); }
        }

        public static readonly DependencyProperty DefaultImageUrlProperty = DependencyProperty.Register("DefaultImageUrl", typeof(string), typeof(WebImage), new PropertyMetadata(null, OnDefaultImageUrlStaticChanged));

        private static void OnDefaultImageUrlStaticChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WebImage)d).ApplyDefaultImage(false);
        }

        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl", typeof(string), typeof(WebImage), new PropertyMetadata(null, OnImageUrlStaticChanged));

        private static void OnImageUrlStaticChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WebImage)d).OnImageUrlChanged();
        }

        private void OnImageUrlChanged()
        {
            try
            {
                ApplyDefaultImage();
                if (string.IsNullOrWhiteSpace(ImageUrl) ||
                    !ValidateUrl(ImageUrl))
                {
                    return;
                }
                var source = new BitmapImage();
                source.ImageFailed += source_ImageFailed;
                source.ImageOpened += source_ImageOpened;
                progress.Visibility = Visibility.Visible;
                source.UriSource = new System.Uri(ImageUrl, System.UriKind.RelativeOrAbsolute);
                image.Source = source;
            }
            catch (System.Exception exc)
            {
                ApplyDefaultImage();
            }
        }

        private bool ValidateUrl(string imageUrl)
        {
            var lowerUrl = imageUrl.ToLowerInvariant();
            return true;// lowerUrl.StartsWith("http:") || lowerUrl.StartsWith("ms-");
        }

        private void ApplyDefaultImage(bool ignoreImageUrl = true)
        {
            progress.Visibility = Visibility.Collapsed;
            try
            {
                if (!ignoreImageUrl && !string.IsNullOrWhiteSpace(ImageUrl))
                {
                    return;
                }
                return;
                //string defaultImage = "/AstralBattles/Resources/Custom.png"; //DefaultImage
                //if (!string.IsNullOrWhiteSpace(defaultImage))
                //{
                //    var source = new BitmapImage();
                //    source.UriSource = new System.Uri(defaultImage, System.UriKind.RelativeOrAbsolute);
                //    image.Source = source;
                //}
            }
            catch
            {
            }
        }

        void source_ImageOpened(object sender, RoutedEventArgs e)
        {
            progress.Visibility = Visibility.Collapsed;
        }

        void source_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ApplyDefaultImage();
            progress.Visibility = Visibility.Collapsed;
        }

    }
}
