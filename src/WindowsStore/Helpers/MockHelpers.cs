using Coex.AppLab.Components.WindowsStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Coex.AppLab.Components.WindowsStore.Controls.Helpers
{
    public class MockHelpers
    {
        /// <summary>
        /// Displays a fake progress dialog
        /// </summary>
        /// <param name="fakeDelay"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task ShowFakeProgressDialog(int fakeDelay, string message = "Please wait")
        {
            var progressControl = new FakeProgressDialogControl { Message = message };
            var progressDialog = new CustomDialog(progressControl);
            progressDialog.ShowAsync();
            await Task.Delay(fakeDelay);
            progressDialog.CloseDialog(null); // Fake delay
            progressDialog.Dispose();
        }

        /// <summary>
        /// Shows a fake Active Directory login dialog
        /// </summary>
        /// <returns></returns>
        public static async Task ShowFakeActiveDirectoryDialog()
        {
            var fakeADLoginControl = new FakeADLoginDialogControl();
            var fakeAdLoginDialog = new CustomDialog(fakeADLoginControl, "Connecting to service");
            var cancelCommand = new UICommand("Cancel");

            fakeADLoginControl.LoggedIn += (object sender, Windows.UI.Xaml.RoutedEventArgs e) =>
            {
                fakeAdLoginDialog.CloseDialog(cancelCommand);
            };

            fakeAdLoginDialog.Commands.Add(cancelCommand);
            fakeAdLoginDialog.CancelCommandIndex = 0;

            await fakeAdLoginDialog.ShowAsync();
        }
    }
}
