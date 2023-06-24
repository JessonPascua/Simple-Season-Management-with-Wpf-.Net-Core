using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class Execute_LogOutCommand : CommandBase
    {
        private readonly SessionManager _sessionManager = new SessionManager();

        public override void Execute(object? parameter)
        {
            _sessionManager.DeleteSession();
            Environment.Exit(0);
        }
        
    }
}
