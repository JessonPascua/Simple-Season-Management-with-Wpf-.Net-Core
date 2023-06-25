using System;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
            this.Closed += (a, b) =>
            {
                Environment.Exit(0);
            };
        }
    }
}
