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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            LabsList.ItemsSource = Getlabs((App.Current as App).ConnectionString);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public ObservableCollection<Labs> Getlabs(string connectionString)
        {
            const string GetLabsQuery = "SELECT * FROM labs";

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
                Helpers.ShowMsgComplete(exSql.Message, "Unable to connect to the database");
            }
            return null;
        }
    }
}
