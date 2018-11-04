using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;

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
    }
}
