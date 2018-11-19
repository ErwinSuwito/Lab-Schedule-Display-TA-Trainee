using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class newHomePage : Page
    {

        public newHomePage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (Grid)e.ClickedItem;

            if (clickedItem.Tag.ToString() == "Search")
            {
                rootFrame.Navigate(typeof(SearchPage), "", new DrillInNavigationTransitionInfo());
            }
            else if (clickedItem.Tag.ToString() == "Level4")
            {
                rootFrame.Navigate(typeof(MainPage), 4, new DrillInNavigationTransitionInfo());
            }
            else if (clickedItem.Tag.ToString() == "Level6")
            {
                rootFrame.Navigate(typeof(MainPage), 6, new DrillInNavigationTransitionInfo());
            }
            else if (clickedItem.Tag.ToString() == "Availability")
            {
                rootFrame.Navigate(typeof(LabAvailability), null, new DrillInNavigationTransitionInfo());
            }
            else if (clickedItem.Tag.ToString() == "Level3")
            {
                rootFrame.Navigate(typeof(MainPage), 3, new DrillInNavigationTransitionInfo());
            }
            else if (clickedItem.Tag.ToString() == "Level5")
            {
                rootFrame.Navigate(typeof(MainPage), 5, new DrillInNavigationTransitionInfo());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            navigationList.SelectedIndex = 4;
            rootFrame.Navigate(typeof(LabAvailability), null, new DrillInNavigationTransitionInfo());
            dateText.Text = DateTime.Now.ToLongDateString();
            timeText.Text = DateTime.Now.ToShortTimeString();

            DispatcherTimer dispatcher = new DispatcherTimer();
            dispatcher.Interval = new TimeSpan(0, 0, 30);
            dispatcher.Tick += Dispatcher_Tick;
            dispatcher.Start();
        }

        private void Dispatcher_Tick(object sender, object e)
        {
            timeText.Text = DateTime.Now.ToShortTimeString();
        }
    }
}
