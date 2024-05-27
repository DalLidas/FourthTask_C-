using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FourthTask.ViewModels
{
    class RegistrationViewModel : ViewBase
    {
        public RegistrationViewModel()
        {
            RegistrationCommand = new LambdaCommand(OnRegistrationCommandExecute, CanRegistrationCommandExecute);
            OpenAuthorizationCommand = new LambdaCommand(OnOpenAuthorizationCommandExecute, CanOpenAuthorizationCommandExecute);
        }


        #region Commands
        /// <summary>
        /// Созадине комынды для авторизации в системе
        /// </summary>
        public ICommand RegistrationCommand { get; }
        private bool CanRegistrationCommandExecute(object parameter) => true;
        private void OnRegistrationCommandExecute(object parameter)
        {
            Ioc.NavigationService.NavigateToAuthorizationPage();
        }


        /// <summary>
        /// Созадине комынды для авторизации в системе
        /// </summary>
        public ICommand OpenAuthorizationCommand { get; }
        private bool CanOpenAuthorizationCommandExecute(object parameter) => true;
        private void OnOpenAuthorizationCommandExecute(object parameter)
        {
            Ioc.NavigationService.NavigateToAuthorizationPage();
        }

        #endregion Commands
    }
}
