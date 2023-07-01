using Microsoft.Extensions.DependencyInjection;
using System;

namespace Simple_Season_Management_with_Wpf_.Net_Core.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IServiceProvider? _serviceProvider;
        public ViewModelLocator()
        {

        }

        public ViewModelLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public LoginViewModel LoginViewModel => _serviceProvider.GetRequiredService<LoginViewModel>();
        public SignInViewModel SignInViewModel => _serviceProvider.GetRequiredService<SignInViewModel>();
    }
}
