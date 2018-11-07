using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Data.Sql;
using System.Data.SqlClient;
using Windows.UI;
using Lab_Schedule_Display;
using Windows.UI.Xaml.Media;
using System;
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

        public TimeSpan currentTime
        {
            get { return (TimeSpan)GetValue(currentTimeProperty); }
            set { SetValue(currentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for currentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty currentTimeProperty =
            DependencyProperty.Register("currentTime", typeof(TimeSpan), typeof(LabItemnew), new PropertyMetadata(0));

        public string LabLocation
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
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

        public LabItemnew()
        {
            this.InitializeComponent();
        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(LabPage), LabName);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection((App.Current as App).ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            //cmd.CommandText = "SELECT * FROM Schedule WHERE EndTime > CONVERT (time," + currentTime +") AND LabName=@labName AND UseDate = CONVERT (date, GETDATE())";
                            cmd.CommandText = "SELECT * FROM Schedule WHERE StartTime < CONVERT(time, GETDATE()) AND EndTime > CONVERT (time, GETDATE()) AND LabName=@labName AND UseDate = CONVERT (date, GETDATE())";
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
                            cmd.CommandText = "SELECT * FROM labs WHERE LabName=@labName AND CONVERT (time, GETDATE()) < CloseTime";
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
                Helpers.ShowMsgComplete(sqlex.Message, "An error occurred.");
            }
        }
    }
}
