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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Lab_Schedule_Display
{
    public sealed partial class LabItem : UserControl
    {

        public string Level
        {
            get { return (string)GetValue(levelLevel); }
            set { SetValue(levelLevel, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty levelLevel =
            DependencyProperty.Register("Level", typeof(string), typeof(LabItem), new PropertyMetadata(null));

        public string LabLocation
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(LabItem), new PropertyMetadata(0));

        public string LabName
        {
            get { return (string) GetValue(LabNameProperty); }
            set { SetValue(LabNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabNameProperty =
            DependencyProperty.Register("LabName", typeof(string), typeof(LabItem), new PropertyMetadata(0));

        public LabItem()
        {
            this.InitializeComponent();
        }

    }
}
