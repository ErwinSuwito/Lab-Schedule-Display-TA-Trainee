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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnhandledExceptionPage : Page
    {
        public UnhandledExceptionPage()
        {
            this.InitializeComponent();
        }

        string errorMessage;
        string stackTrace;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (errorParameters)e.Parameter;
            errorMessage = parameters.ErrorMessage;
            stackTrace = parameters.StackTrace;
        }

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            errorMessageText.Text = "Error: " + errorMessage;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            this.Frame.Navigate(typeof(Checks));
            dispatcherTimer.Stop();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Helpers.ShowMsgComplete(errorMessage, stackTrace);
        }
    }
}
