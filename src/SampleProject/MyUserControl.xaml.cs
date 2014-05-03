using Coex.AppLab.Components.WindowsStore.Controls;

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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Coex.AppLab.Components.WindowsStore.SampleProject
{
    // Any user control that is to be popped has to extend PopupBase
    public sealed partial class MyUserControl : UserControl
    {
        /// <summary>
        /// Any databound property (you can have it two-way binded)
        /// Important:
        /// You need to set the DataContext on the UserControl like so
        /// DataContext="{Binding RelativeSource={RelativeSource Mode=Self}}">
        /// </summary>
        public string DataBoundProperty
        {
            get { return (string)GetValue(DataBoundPropertyyProperty); }
            set { SetValue(DataBoundPropertyyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataBoundPropertyy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataBoundPropertyyProperty =
            DependencyProperty.Register("DataBoundProperty", typeof(string), typeof(MyUserControl), new PropertyMetadata(""));

        
        public MyUserControl()
        {
            this.InitializeComponent();
        }
    }
}
