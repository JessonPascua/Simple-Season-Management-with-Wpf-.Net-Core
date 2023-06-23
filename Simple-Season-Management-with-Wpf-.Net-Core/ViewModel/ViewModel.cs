using Simple_Season_Management_with_Wpf_.Net_Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Simple_Season_Management_with_Wpf_.Net_Core.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        public ICommand SignInCommand { get; set; }

        public ViewModel()
        {
            SignInCommand = new SignInCommand();
        }
    }
}
