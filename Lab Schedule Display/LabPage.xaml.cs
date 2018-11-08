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
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LabPage : Page
    {
        string labName;

        public LabPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            labName = e.Parameter.ToString();
            headerText.Text = labName;
            LabScheduleView.ItemsSource = GetSchedules((App.Current as App).ConnectionString);
            try
            {
                using (SqlConnection conn = new SqlConnection((App.Current as App).ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand()) 
                        {
                            cmd.CommandText = "SELECT * FROM Schedule WHERE StartTime < CONVERT(time, GETDATE()) AND EndTime > CONVERT (time, GETDATE()) AND LabName=@labName AND UseDate = CONVERT (date, GETDATE())";
                            cmd.Parameters.AddWithValue("@labName", labName);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    labStatus.Text = "This lab is unavailable for use.";
                                    symbolIcon1.Symbol = Symbol.Cancel;
                                    //rootPanel.Background = new SolidColorBrush(Color.FromArgb(51, 114, 33, 33));
                                    Windows.UI.Xaml.Media.AcrylicBrush myBrush = new Windows.UI.Xaml.Media.AcrylicBrush();
                                    myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.Backdrop;
                                    myBrush.TintColor = Color.FromArgb(51, 114, 33, 33);
                                    myBrush.FallbackColor = Color.FromArgb(51, 114, 33, 33);
                                    myBrush.TintOpacity = 0.5;

                                    backgroundAcrylic.Background = myBrush;
                                }
                                else
                                {
                                    labStatus.Text = "This lab is available for use.";
                                    symbolIcon1.Symbol = Symbol.Accept;
                                    //rootPanel.Background = new SolidColorBrush(Color.FromArgb(51, 33, 114, 33));

                                    Windows.UI.Xaml.Media.AcrylicBrush myBrush = new Windows.UI.Xaml.Media.AcrylicBrush();
                                    myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.Backdrop;
                                    myBrush.TintColor = Color.FromArgb(51, 33, 114, 33);
                                    myBrush.FallbackColor = Color.FromArgb(51, 33, 114, 33);
                                    myBrush.TintOpacity = 0.9;

                                    backgroundAcrylic.Background = myBrush;
                                }
                            }
                            cmd.CommandText = "SELECT * FROM labs WHERE LabName=@labName AND CONVERT (time, GETDATE()) < CloseTime";
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (!dr.HasRows)
                                {
                                    labStatus.Text = "This lab has been closed.";
                                    symbolIcon1.Symbol = Symbol.Cancel;
                                    rootPanel.Background = new SolidColorBrush(Color.FromArgb(51, 114, 33, 33));

                                    Windows.UI.Xaml.Media.AcrylicBrush myBrush = new Windows.UI.Xaml.Media.AcrylicBrush();
                                    myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.Backdrop;
                                    myBrush.TintColor = Color.FromArgb(51, 114, 33, 33);
                                    myBrush.FallbackColor = Color.FromArgb(51, 114, 33, 33);
                                    myBrush.TintOpacity = 0.5;

                                    backgroundAcrylic.Background = myBrush;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception sqlex)
            {
                Helpers.ShowMsgComplete(sqlex.Message, "An error occurred.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            else
            {
                this.Frame.Navigate(typeof(newHome), new DrillInNavigationTransitionInfo());
            }
        }

        public ObservableCollection<Schedules> GetSchedules(string connectionString)
        {
            string GetSchedulesQuery = "SELECT Schedule.LabName, Schedule.ModuleCode, lecturer.LecName, IntakeCode, ModuleName, StartTime, EndTime FROM Schedule JOIN lecturer ON lecturer.LecturerID = Schedule.LecturerID JOIN modules ON modules.ModuleCode = Schedule.ModuleCode WHERE Schedule.LabName=@labName AND UseDate =CONVERT (date, GETDATE())";
            var schedules = new ObservableCollection<Schedules>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetSchedulesQuery;
                            cmd.Parameters.AddWithValue("@labName", labName);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    var schedule = new Schedules();
                                    schedule.ModuleCode = dr.GetString(1);
                                    schedule.LecturerName = dr.GetString(2);
                                    schedule.IntakeCode = dr.GetString(3);
                                    schedule.ModuleName = dr.GetString(4);
                                    schedule.StartTime = dr.GetTimeSpan(5);
                                    schedule.EndTime = dr.GetTimeSpan(6);
                                    
                                    if (DateTime.Now.TimeOfDay < schedule.EndTime && DateTime.Now.TimeOfDay > schedule.StartTime)
                                    {
                                        schedule.myBrush = new SolidColorBrush(Colors.DarkGreen);
                                    }
                                    else if (DateTime.Now.TimeOfDay > schedule.EndTime)
                                    {
                                        schedule.myBrush = new SolidColorBrush(Colors.Red);
                                    }
                                    else if (DateTime.Now.TimeOfDay < schedule.StartTime && DateTime.Now.TimeOfDay < schedule.EndTime)
                                    {
                                        schedule.myBrush = new SolidColorBrush(Colors.LightGreen);
                                    }

                                    schedules.Add(schedule);
                                }
                            }
                        }
                    }
                }
                return schedules;
            }
            catch (Exception exSql)
            {
                Helpers.ShowMsgComplete(exSql.Message, "Unable to connect to the database");
            }
            return null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (LabScheduleView.Items.Count == 0)
            {
                if (labStatus.Text != "This lab has been closed.")
                {
                    LabScheduleView.Visibility = Visibility.Collapsed;
                    LabScheduleHeader.Visibility = Visibility.Collapsed;
                    symbolIcon1.Symbol = Symbol.Accept;
                    rootPanel.Background = new SolidColorBrush(Color.FromArgb(51, 33, 114, 33));
                    showNoSchedule.Begin();
                }
            }

            if (headerText.Text == "TL04-04")
            {
                bgImage.Source = new BitmapImage(new Uri(this.BaseUri, "/LabAssets/TL04-04.jpeg"));
            }
        }
    }
}
