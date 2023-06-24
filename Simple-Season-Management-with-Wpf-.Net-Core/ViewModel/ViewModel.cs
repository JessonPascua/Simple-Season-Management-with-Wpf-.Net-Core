﻿using Simple_Season_Management_with_Wpf_.Net_Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Simple_Season_Management_with_Wpf_.Net_Core.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        private string? _username;
        public string? Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private SecureString _password = new();
        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand SignInCommand { get; set; }
        public ICommand LogInCommand { get; set; }

        public ViewModel()
        {
            SignInCommand = new Execute_OpenSignInCommand();
            LogInCommand = new Execute_LogInCommand(this);
        }
    }
}
