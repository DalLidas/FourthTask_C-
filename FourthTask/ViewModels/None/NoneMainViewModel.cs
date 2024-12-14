using System.Windows.Input;
using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using FourthTask.ViewModels.Commands;
using FourthTask.DataBase;


namespace FourthTask.ViewModels
{
    internal class NoneMainViewModel : ViewModelBase
    {
        public NoneMainViewModel()
        {
           
            SendRegistrationRequestDataCommand = new LambdaCommand(OnSendRegistrationRequestDataCommandExecute, CanSendRegistrationRequestDataCommandExecute);

            ExitUserCommand = new LambdaCommand(OnExitUserCommandExecute, CanExitUserCommandExecute);
        }

        #region Команды

        #region Команда отправки формы для регистрации в системе
        public ICommand SendRegistrationRequestDataCommand { get; }
        private bool CanSendRegistrationRequestDataCommandExecute(object parameter) => true;
        private void OnSendRegistrationRequestDataCommandExecute(object parameter)
        {
            //Ioc.StudentNavigationService?.NavigateToStudentTeachersPage();
        }
        #endregion Команда отправки формы для регистрации в системе

        #region Команда выхода из учётной записи
        public ICommand ExitUserCommand { get; }

        private bool CanExitUserCommandExecute(object parameter) => true;
        private void OnExitUserCommandExecute(object parameter)
        {
            Ioc.model?.CloseSession();
            Ioc.MainNavigationService?.NavigateToAuthorizationPage();
        }
        #endregion Команда выхода из учётной записи

        #endregion Команды




        #region Поля

        private string _Title = "Технологический ВУЗ \"Сессия\"";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion Поля
    }
}