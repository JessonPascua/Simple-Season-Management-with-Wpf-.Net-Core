using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class SignInCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            if (parameter == null)
            {
                return;
            }

            MessageBox.Show("Signed");
        }
    }
}
