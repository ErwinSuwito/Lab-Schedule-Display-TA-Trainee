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
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LabAvailability : Page
    {
        public TimeSpan selectedTime;
        DispatcherTimer dispatcherTimer;

        public LabAvailability()
        {
            this.InitializeComponent();

            timePicker1.Time = DateTime.Now.TimeOfDay;
            //fetching data from db
            if ((App.Current as App).useLocal == false)
            {
                AvailableLabsList.ItemsSource = GetLabs((App.Current as App).ConnectionStringRemote);
            }
            else
            {
                AvailableLabsList.ItemsSource = GetLabs((App.Current as App).ConnectionStringLocal);
            }

            //setting up dispatcherTimer to auto-update time
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1, 0);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            timePicker1.Time = DateTime.Now.TimeOfDay;
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            timePicker1.Time = DateTime.Now.TimeOfDay;
        }

        private void timePicker1_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            selectedTime = timePicker1.Time;

            if ((App.Current as App).useLocal == false)
            {
                AvailableLabsList.ItemsSource = GetLabs((App.Current as App).ConnectionStringRemote);
            }
            else
            {
                AvailableLabsList.ItemsSource = GetLabs((App.Current as App).ConnectionStringLocal);
            }
            
            if (AvailableLabsList.Items.Count == 0)
            {
                NoLabsAvailable.Begin();
            }
            else if (DropShadowPanel1.Opacity == 1)
            {
                LabsAvailable.Begin();
            }
        }

        public ObservableCollection<Labs> GetLabs(string connectionString)
        {
            string GetLabsQuery = "SELECT DISTINCT * FROM labs WHERE labs.CloseTime > CONVERT(time,'" + timePicker1.Time.ToString() + "')";
            Debug.WriteLine(GetLabsQuery);
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
                                    lab.SelectedTime = selectedTime.ToString();
                                    lab.Level = "Level " + dr.GetInt32(2).ToString();
                                    labs.Add(lab);
                                }
                            }
                        }
                    }
                }
                return labs;
            }
            catch (Exception exSql)
            {
                Helpers.ShowMsgComplete(exSql.Message, "Unable to connect to the database.");
            }
            return null;
        }
    }
}
