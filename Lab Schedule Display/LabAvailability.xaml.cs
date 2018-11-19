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
    public sealed partial class LabAvailability : Page
    {
        public TimeSpan selectedTime;

        public LabAvailability()
        {
            this.InitializeComponent();

            //fetching data from db
            var newHome1 = new newHome();
            AvailableLabsList.ItemsSource = newHome1.GetLabs((App.Current as App).ConnectionString, DateTime.Now.TimeOfDay);
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            timePicker1.Time = DateTime.Now.TimeOfDay;
        }

        private void timePicker1_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            selectedTime = timePicker1.Time;
            var newHome1 = new newHome();
            AvailableLabsList.ItemsSource = newHome1.GetLabs((App.Current as App).ConnectionString, timePicker1.Time);
            if (AvailableLabsList.Items.Count == 0)
            {
                NoLabsAvailable.Begin();
            }
            else if (DropShadowPanel1.Opacity == 1)
            {
                LabsAvailable.Begin();
            }
        }
    }
}
