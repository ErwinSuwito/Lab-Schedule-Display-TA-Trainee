using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        string whichLab;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            whichLab = e.Parameter.ToString();
            headerText.Text = whichLab;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mapImage.Source = new BitmapImage(new Uri(BaseUri, "/MapAssets/" + whichLab +".jpg"));
        }

        private void mapImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            DropShadowPanel1.Visibility = Visibility.Visible;
            ImageError.Begin();
        }
    }
}
