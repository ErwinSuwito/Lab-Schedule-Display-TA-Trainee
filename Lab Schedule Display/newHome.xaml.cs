using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class newHome : Page
    {

        public TimeSpan selectedTime;
        
        public newHome()
        {
            this.InitializeComponent();
            LabsList.ItemsSource = GetLevels((App.Current as App).ConnectionString);
            timePicker1.Time = DateTime.Now.TimeOfDay;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            currentTime.Text = DateTime.Now.ToShortTimeString();
            currentDate.Text = DateTime.Now.ToLongDateString();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            currentTime.Text = DateTime.Now.ToShortTimeString();
            AvailableLabsList.ItemsSource = GetLabs((App.Current as App).ConnectionString);
        }

        public ObservableCollection<Labs> GetLabs(string connectionString)
        {
            const string GetLabsQuery = "SELECT DISTINCT * FROM labs WHERE labs.CloseTime > CONVERT(time, GETDATE())";

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

        public ObservableCollection<Levels> GetLevels(string connectionString)
        {
            const string GetLevelsQuery = "SELECT DISTINCT Level FROM labs";

            var levels = new ObservableCollection<Levels>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetLevelsQuery;
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    var level = new Levels();
                                    level.Level = dr.GetInt32(0).ToString();
                                    levels.Add(level);
                                }
                            }
                        }
                    }
                }
                return levels;
            }
            catch (Exception exSql)
            {
                Helpers.ShowMsgComplete(exSql.Message, "Unable to connect to the database.");
            }
            return null;
        }

        private void timePicker1_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            selectedTime = timePicker1.Time;
            AvailableLabsList.ItemsSource = GetLabs((App.Current as App).ConnectionString);
            if (AvailableLabsList.Items.Count == 0)
            {
                defaultPanel.Visibility = Visibility.Collapsed;
                DropShadowPanel1.Visibility = Visibility.Visible;
            }
        }
    }
}