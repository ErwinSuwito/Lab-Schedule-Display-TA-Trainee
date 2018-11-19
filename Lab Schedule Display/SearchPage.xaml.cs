using Lab_Schedule_Display.DataLayers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab_Schedule_Display
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchResult.ItemsSource = null;

            if (DropShadowPanel3.Opacity == 1)
            {
                SearchResultsAvailable.Begin();
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            searchResult.ItemsSource = GetSearchResults((App.Current as App).ConnectionString);
            if (searchResult.Items.Count == 0)
            {
                DropShadowPanel3.Visibility = Visibility.Visible;
                SearchResultsNotAvailable.Begin();
            }
            else
            {
                if (DropShadowPanel3.Opacity == 1)
                {
                    SearchResultsAvailable.Begin();
                }
            }
        }

        private void searchResult_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (SearchResults)e.ClickedItem;
            searchBox.Text = "";
            this.Frame.Navigate(typeof(LabPage), clickedItem.LabName, new DrillInNavigationTransitionInfo());
        }

        public ObservableCollection<SearchResults> GetSearchResults(string connectionString)
        {
            string GetLevelsQuery = "SELECT DISTINCT * FROM Schedule JOIN labs ON labs.LabName = Schedule.LabName JOIN lecturer ON lecturer.LecturerID = Schedule.LecturerID JOIN modules ON modules.ModuleCode = Schedule.ModuleCode WHERE (Schedule.LecturerID=(SELECT LecturerID FROM lecturer WHERE LecName LIKE'%" + searchBox.Text + "%') Or Schedule.LabName LIKE '%" + searchBox.Text + "%' Or Schedule.ModuleCode LIKE '%" + searchBox.Text + "%' Or Schedule.ModuleCode=(SELECT ModuleCode FROM modules WHERE ModuleName LIKE '%" + searchBox.Text + "%')) AND Schedule.UseDate = CONVERT(date, GETDATE())";

            var results = new ObservableCollection<SearchResults>();
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
                                    var result = new SearchResults();
                                    result.EndTime = dr.GetTimeSpan(7);
                                    result.IntakeCode = dr.GetString(4);
                                    result.LabLocation = dr.GetString(9);
                                    result.LabName = dr.GetString(1);
                                    result.UseDate = dr.GetDateTime(5);
                                    result.LecturerName = dr.GetString(13);
                                    result.ModuleCode = dr.GetString(2);
                                    result.ModuleName = dr.GetString(15);
                                    result.StartTime = dr.GetTimeSpan(6);
                                    results.Add(result);
                                }
                            }
                        }
                    }
                }
                return results;
            }
            catch (SqlException exSql)
            {
                if (Debugger.IsAttached)
                {
                    Helpers.ShowMsgComplete(exSql.Message + exSql.StackTrace, "SqlException");
                }
                else
                {
                    Helpers.ShowMsgComplete("The search query is too short or there is something wrong when connecting to the database. Please try again with another search query. If the problem persists, please contact TA.", "Unable to search");
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Helpers.ShowMsgComplete(ex.Message + ex.StackTrace, ex.ToString());
                }
                else
                {
                    Helpers.ShowMsgComplete("Please try again with another search query. If the problem persists, please contact TA.", "Unable to search");
                }
            }
            return null;
        }
    }
}
