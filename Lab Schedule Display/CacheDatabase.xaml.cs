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
            GetLabScheduleData();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void CopyToSqlite()
        {
        }

        private void InitializeSqlite()
        {

        }

        private void GetLabScheduleData()
        {
            string GetLevelsQuery = "SELECT * FROM labs";

            try
            {
                using (SqlConnection conn = new SqlConnection((App.Current as App).ConnectionStringRemote))
                {
                    //conn.Open();
                    //if (conn.State == System.Data.ConnectionState.Open)
                    //{
                    //    SqlDataAdapter da = new SqlDataAdapter();
                    //    SqlCommand command = new SqlCommand();
                    //command.CommandText = "SELECT * FROM labs";


                    //    da.SelectCommand = command;

                    //    da.Fill(dataSet, "Labs");

                    //    //SqlDataAdapter da2 = new SqlDataAdapter();
                    //    //da2.SelectCommand = "SELECT * FROM lecturer";

                    //    //da2.Fill(dataSet, "Lecturer");

                    //    //SqlDataAdapter da3 = new SqlDataAdapter();
                    //    //da3.SelectCommand = "SELECT * FROM modules";

                    //    //da3.Fill(dataSet, "Modules");

                    //    //SqlDataAdapter da4 = new SqlDataAdapter();
                    //    //da4.SelectCommand = "SELECT * FROM Schedule";

                    //    //da4.Fill(dataSet, "Schedule");
                }
                    

                    CopyToSqlite();
            }
            catch (Exception ex)
            { 
                Helpers.ShowMsgComplete(ex.Message + ex.StackTrace, ex.ToString());
            }
        }
    }
}
