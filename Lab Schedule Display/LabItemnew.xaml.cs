using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Data.Sql;
using System.Data.SqlClient;
using Windows.UI;
using Lab_Schedule_Display;
using Windows.UI.Xaml.Media;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Animation;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Lab_Schedule_Display
{
    public sealed partial class LabItemnew : UserControl
    {

        public string Level
        {
            get { return (string)GetValue(levelLevel); }
            set { SetValue(levelLevel, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty levelLevel =
            DependencyProperty.Register("Level", typeof(string), typeof(LabItem), new PropertyMetadata(null));

        public string LabLocation
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(LabItem), new PropertyMetadata(0));

        public string LabName
        {
            get { return (string) GetValue(LabNameProperty); }
            set { SetValue(LabNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabNameProperty =
            DependencyProperty.Register("LabName", typeof(string), typeof(LabItem), new PropertyMetadata(0));

        public string SelectedTime
        {
            get { return (string)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(string), typeof(LabItemnew), new PropertyMetadata(default(string)));

        public LabItemnew()
        {
            this.InitializeComponent();
        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(LabPage), LabName, new DrillInNavigationTransitionInfo());
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           if ((App.Current as App).useLocal == false)
           {
                try
                {
                    using (SqlConnection conn = new SqlConnection((App.Current as App).ConnectionStringRemote))
                    {
                        conn.Open();
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "SELECT * FROM Schedule WHERE StartTime < CONVERT (time,'" + SelectedTime + "') AND EndTime > CONVERT (time, '" + SelectedTime + "') AND LabName=@labName AND UseDate = CONVERT (date, GETDATE())";
                                cmd.Parameters.AddWithValue("@labName", LabName);
                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        //unavailable
                                        //#66C13F3F
                                        rootGrid.Background = new SolidColorBrush(Helpers.GetSolidColorBrush("#66C13F3F").Color);
                                    }
                                    else
                                    {
                                        //available
                                        //#663CE05A
                                        rootGrid.Background = new SolidColorBrush(Helpers.GetSolidColorBrush("#663CE05A").Color);
                                    }
                                }
                                //cmd.CommandText = "SELECT * FROM labs WHERE LabName=@labName AND CONVERT (time," + currentTime + ") < CloseTime";
                                cmd.CommandText = "SELECT * FROM labs WHERE LabName=@labName AND CONVERT (time,'" + SelectedTime + "') < CloseTime";
                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    if (!dr.HasRows)
                                    {
                                        //lab closed
                                        //#66C13F3F
                                        rootGrid.Background = new SolidColorBrush(Helpers.GetSolidColorBrush("#FFC13F3F").Color);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException sqlex)
                {
                    Helpers.ShowMsgComplete(sqlex.Message + sqlex.StackTrace + sqlex.LineNumber, "An error occurred.");
                }
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection((App.Current as App).ConnectionStringLocal))
                    {
                        conn.Open();
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "SELECT * FROM Schedule WHERE StartTime < CONVERT (time,'" + SelectedTime + "') AND EndTime > CONVERT (time, '" + SelectedTime + "') AND LabName=@labName AND UseDate = CONVERT (date, GETDATE())";
                                cmd.Parameters.AddWithValue("@labName", LabName);
                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        //unavailable
                                        rootGrid.Background = new SolidColorBrush(Helpers.GetSolidColorBrush("#66C13F3F").Color);
                                    }
                                    else
                                    {
                                        //available
                                        rootGrid.Background = new SolidColorBrush(Helpers.GetSolidColorBrush("#663CE05A").Color);
                                    }
                                }
                                cmd.CommandText = "SELECT * FROM labs WHERE LabName=@labName AND CONVERT (time,'" + SelectedTime + "') < CloseTime";
                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    if (!dr.HasRows)
                                    {
                                        //lab closed
                                        rootGrid.Background = new SolidColorBrush(Helpers.GetSolidColorBrush("#FFC13F3F").Color);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SqlException sqlex)
                {
                    Helpers.ShowMsgComplete(sqlex.Message + sqlex.StackTrace + sqlex.LineNumber, "An error occurred.");
                }
            }
        }
    }
}
