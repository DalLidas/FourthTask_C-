using FourthTask.ViewModels.Base;
using System.Windows.Input;
using System.Windows;
using FourthTask.ViewModels.Commands;
using FourthTask.PageNavigation.Ioc;
using System.Text.RegularExpressions;

namespace FourthTask.ViewModels
{
    class RegistrationViewModel : ViewModelBase
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
        private async void OnRegistrationCommandExecute(object parameter)
        {
            //Проверка данных аккаунта
            string errorMSG = "";

            if (_password != _secondPassword) errorMSG += "Неверный введён повторный пароль\n";
            if (!Regex.IsMatch(_email, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")) errorMSG += "Неправильный формат почты\n";

            if (errorMSG != "")
            {
                MessageBox.Show(errorMSG);
                return;
            }

            //Регистрация аккаунта
            if (Ioc.model is not null && Ioc.model.DBConnector is not null) 
            {
                bool ans = false; // False - Имя пользователя занято, True - Добавлен новый пользователь
                ans = await Ioc.model.RegistrateUser(_login.Trim(), _password.Trim(), _email.Trim());

                if (ans)
                {
                    Ioc.MainNavigationService?.NavigateToAuthorizationPage();
                    MessageBox.Show("Пользователь успешно зарегистрирован");
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином и паролем yже зарегистрирован");
                }
            }
            else
            {
                MessageBox.Show("Проблемы с соединением с базой данных");
            }
        }


        /// <summary>
        /// Созадине комынды для авторизации в системе
        /// </summary>
        public ICommand OpenAuthorizationCommand { get; }
        private bool CanOpenAuthorizationCommandExecute(object parameter) => true;
        private void OnOpenAuthorizationCommandExecute(object parameter)
        {
            Ioc.MainNavigationService?.NavigateToAuthorizationPage();
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

        private string _secondPassword = "";
        public string SecondPassword
        {
            get => _secondPassword;
            set => Set(ref _secondPassword, value);
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        #endregion Поля
    }
}
