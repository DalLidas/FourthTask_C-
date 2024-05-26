using FourthTask.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FourthTask.ViewModels.Commands;

namespace FourthTask.ViewModels
{
    class AthorizationViewModel : ViewModelBase
    {
        public AthorizationViewModel()
        {
            AuthorizationCommand = new LambdaCommand(OnAuthorizationCommandExecute, CanAuthorizationCommandExecute);
            OpenRegistrationWindowCommand = new LambdaCommand(OnOpenRegistrationWindowCommandExecute, CanOpenRegistrationWindowCommandExecute);
        }


        #region Commands
        /// <summary>
        /// Созадине комынды для авторизации в системе
        /// </summary>
        public ICommand AuthorizationCommand { get; }
        private bool CanAuthorizationCommandExecute(object parameter) => true;
        private void OnAuthorizationCommandExecute(object parameter)
        {

        }


        /// <summary>
        /// Созадине комынды для авторизации в системе
        /// </summary>
        public ICommand OpenRegistrationWindowCommand { get; }
        private bool CanOpenRegistrationWindowCommandExecute(object parameter) => true;
        private void OnOpenRegistrationWindowCommandExecute(object parameter)
        {
            
        }

        #endregion Commands

    }
}
