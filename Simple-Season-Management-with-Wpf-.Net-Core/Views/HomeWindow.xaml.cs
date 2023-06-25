using System;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Views
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
            this.Closed += (a, b) =>
            {
                Environment.Exit(0);
            };
        }
    }
}
