using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;

namespace Lab_Schedule_Display
{
    class Helpers
    {

        static public async void ShowMsgBody(string notifBody)
        {
            var dialog = new MessageDialog(notifBody);
            await dialog.ShowAsync();
        }

        static public async void ShowMsgComplete(string notifBody, string notifTitle)
        {
            var dialog = new MessageDialog(notifBody, notifTitle);
            await dialog.ShowAsync();
        }

        static public bool HasInternetAccess { get { return CheckInternetAccess(); } }

        private static bool CheckInternetAccess()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            var HasInternetAccess = (connectionProfile != null &&
                connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
            return HasInternetAccess;
        }

        static public bool ConnectionTest { get { return CheckDbConnection((App.Current as App).connectionString); } }

        private static bool CheckDbConnection(string connectionString)
        {
            using (var connectTest = new SqlConnection(connectionString))
            {
                //try
                //{
                    connectTest.Open();
                    return true;
                //}
                //catch (SqlException)
                //{
                    return false;
                //}
            }
        }

        public static SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }
}
