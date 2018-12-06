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
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Lab_Schedule_Display.DbTables;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CacheDatabase : Page
    {
        public DataSet dataSet;
        public CacheDatabase()
        {
            this.InitializeComponent();
            dataSet = new DataSet("LabSchedule");
            GetLabData();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private bool InsertLabsToLocal(string LabName, string Location, int Level, TimeSpan CloseTime)
        {
            string InsertLabsToLocal = "INSERT INTO labs VALUES(@labname, @location, @level, @closetime";

            try
            {
                using (SqlConnection conn = new SqlConnection((App.Current as App).ConnectionStringLocal))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = InsertLabsToLocal;
                            cmd.Parameters.AddWithValue("@labname", LabName);
                            cmd.Parameters.AddWithValue("@location", Location);
                            cmd.Parameters.AddWithValue("@level", Level);
                            cmd.Parameters.AddWithValue("@closetime", CloseTime);

                            cmd.ExecuteNonQuery();

                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        private void GetLabData()
        {
            string LabTableQuery = "SELECT * FROM labs";
            bool DoInsertLabToLocal = false;
            try
            {
                using (SqlConnection conn = new SqlConnection((App.Current as App).ConnectionStringRemote))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = LabTableQuery;
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    DoInsertLabToLocal = InsertLabsToLocal(dr.GetString(0), dr.GetString(1), dr.GetInt32(2), dr.GetTimeSpan(3));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                Helpers.ShowMsgComplete(ex.Message + ex.StackTrace, ex.ToString());
            }
            finally
            {
                if (DoInsertLabToLocal == true)
                {
                    //Copy the other tables.
                    this.Frame.Navigate(typeof(newHomePage));
                }
            }
        }
    }
}
