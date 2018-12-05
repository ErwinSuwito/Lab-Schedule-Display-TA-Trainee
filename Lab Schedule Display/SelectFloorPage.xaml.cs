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
    public sealed partial class SelectFloorPage : Page
    {
        public SelectFloorPage()
        {
            this.InitializeComponent();
            if ((App.Current as App).useLocal == false)
            {
                LabsList.ItemsSource = GetLevels((App.Current as App).ConnectionStringRemote);
            }
            else
            {
                LabsList.ItemsSource = GetLevels((App.Current as App).ConnectionStringLocal);
            }
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
    }
}
