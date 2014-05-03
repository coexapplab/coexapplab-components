using Coex.AppLab.Components.WindowsStore.Controls;
using Coex.AppLab.Components.WindowsStore.Controls.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Coex.AppLab.Components.WindowsStore.SampleProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void FakeADButton_Click(object sender, RoutedEventArgs e)
        {
            await MockHelpers.ShowFakeActiveDirectoryDialog();
        }

        private async void FakeProgressButton_Click(object sender, RoutedEventArgs e)
        {
            await MockHelpers.ShowFakeProgressDialog(2000);
        }

        private async void UserControlDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var myUserControl = new MyUserControl();
            var popupDialog = new CustomDialog(myUserControl, "This is a custom title");

            // Adding an anonymous UICommand handler
            popupDialog.Commands.Add(new UICommand("Confirm", async cmd =>
            {
                // You can read Dependency Properties like so
                var dataBoundProperty = myUserControl.DataBoundProperty;

                var dialog = new MessageDialog("You entered: " + dataBoundProperty);
                await dialog.ShowAsync();

            }));

            popupDialog.Commands.Add(new UICommand("Cancel"));
            popupDialog.DefaultCommandIndex = 0;
            popupDialog.CancelCommandIndex = 1;

            await popupDialog.ShowAsync();
        }
    }
}
