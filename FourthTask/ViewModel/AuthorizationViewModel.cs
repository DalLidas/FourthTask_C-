using FourthTask.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FourthTask.ViewModels.Commands;
using FourthTask.PageNavigation.Ioc;
using FourthTask.Models.Model;
using System.Security;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;

namespace FourthTask.ViewModels
{
    class AuthorizationViewModel : ViewModelBase
    {
        public AuthorizationViewModel()
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
        private async void OnAuthorizationCommandExecute(object parameter)
        {
            string dbPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location ?? "") ?? "", "MyData.db");

            await Ioc.InitModel(dbPath, _login, _password);

            if (Ioc.model is null || Ioc.model.GetSessionStatus())
                Ioc.NavigationService?.NavigateToMainPage();
            else
                MessageBox.Show("Пользователя с таким логином и паролем не зарегистрировано");
        }

        
        /// <summary>
        /// Созадине комынды для авторизации в системе
        /// </summary>
        public ICommand OpenRegistrationWindowCommand { get; }
        private bool CanOpenRegistrationWindowCommandExecute(object parameter) => true;
        private void OnOpenRegistrationWindowCommandExecute(object parameter)
        {
            Ioc.NavigationService?.NavigateToRegistrationPage();
        }

        #endregion Commands

        #region Поля

        private string _login = "";
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        #endregion Поля
    }
}
