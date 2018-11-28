using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
using Lab_Schedule_Display.DataLayers;

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

            AvailableLabsList.ItemsSource = GetLabs((App.Current as App).ConnectionString, selectedTime);
            if (AvailableLabsList.Items.Count == 0)
            {
                NoLabsAvailable.Begin();
            }
            else if (DropShadowPanel1.Opacity == 1)
            {
                LabsAvailable.Begin();
            }
        }

        public ObservableCollection<Labs> GetLabs(string connectionString, TimeSpan timeSpan)
        {
            string GetLabsQuery = "SELECT DISTINCT * FROM labs WHERE labs.CloseTime > CONVERT(time,'" + timePicker1.Time.ToString() + "')";

            var labs = new ObservableCollection<Labs>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetLabsQuery;
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    var lab = new Labs();
                                    lab.LabName = dr.GetString(0);
                                    lab.LabLocation = dr.GetString(1);
                                    lab.SelectedTime = timePicker1.Time.ToString();
                                    lab.Level = "Level " + dr.GetInt32(2).ToString();
                                    labs.Add(lab);
                                }
                            }
                        }
                    }
                }
                return labs;
            }
            catch (Exception)
            {
                this.Frame.Navigate(typeof(Checks));
            }
            return null;
        }
    }
}
