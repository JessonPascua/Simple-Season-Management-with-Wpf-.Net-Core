using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class Execute_LogInCommand : CommandBase
    {
        private readonly ViewModel.ViewModel? _viewModel;
        public Execute_LogInCommand(ViewModel.ViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            if (!string.IsNullOrEmpty(_viewModel?.Username) && _viewModel.Password?.Length > 0)
            {
               
                
            }
        }
    }
}
