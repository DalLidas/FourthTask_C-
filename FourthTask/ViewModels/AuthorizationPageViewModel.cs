using FourthTask.ViewModels.Base;
using System.Windows.Input;
using System.Windows;
using FourthTask.ViewModels.Commands;
using FourthTask.PageNavigation.Ioc;


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
        private bool CanAuthorizationCommandExecute(object parameter) => Ioc.model is not null && Ioc.model.DBConnector is not null;
        private async void OnAuthorizationCommandExecute(object parameter)
        {
            if (Ioc.model is not null && Ioc.model.DBConnector is not null) 
            {
                await Ioc.model.StartSession(_login.Trim(), _password.Trim());

                if (Ioc.model.GetSessionStatus())
                {
                    switch (Ioc.model.GetSessionPrivilages())
                    {
                        case Models.Privilages.admin:
                            Ioc.MainNavigationService?.NavigateToAdminMainPage();
                            break;
                        case Models.Privilages.teacher:
                            Ioc.MainNavigationService?.NavigateToTeacherMainPage();
                            break;
                        case Models.Privilages.student:
                            Ioc.MainNavigationService?.NavigateToStudentMainPage();
                            break;
                        case Models.Privilages.None:
                            Ioc.MainNavigationService?.NavigateToNoneMainPage();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином и паролем не зарегистрировано");
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
        public ICommand OpenRegistrationWindowCommand { get; }
        private bool CanOpenRegistrationWindowCommandExecute(object parameter) => true;
        private void OnOpenRegistrationWindowCommandExecute(object parameter)
        {
            Ioc.MainNavigationService?.NavigateToRegistrationPage();
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
