using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Coex.AppLab.Components.WindowsStore.Controls
{
    public sealed partial class FakeADLoginDialogControl : UserControl
    {
        // A delegate type for hooking up LoggedIn event
        public delegate void LoggedInEventHandler(object sender, RoutedEventArgs e);

        // An event that clients can use to be notified whenever we log in fakely
        public event LoggedInEventHandler LoggedIn;

        public FakeADLoginDialogControl()
        {
            this.InitializeComponent();
            this.Loaded += FakeADLoginDialogControl_Loaded;
        }

        async void FakeADLoginDialogControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Show state 1
            State1.Visibility = Windows.UI.Xaml.Visibility.Visible;
            // Hide state 2
            State2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            // Wait a fake 5 seconds
            await Task.Delay(3000);

            // Hide state 1
            State1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            // Show state 2
            State2.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            loginProgress.IsActive = true;
            await Task.Delay(4000);
            loginProgress.IsActive = false;
            if (LoggedIn != null)
                LoggedIn(this, e);
        }

    }
}
