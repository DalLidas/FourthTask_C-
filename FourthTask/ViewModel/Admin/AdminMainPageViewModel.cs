using System.Windows.Input;
using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using FourthTask.ViewModels.Commands;

using FourthTask.DataBase;


namespace FourthTask.ViewModels
{
    class AdminMainPageViewModel : ViewModelBase
    {
        public AdminMainPageViewModel()
        {
            init();

            ShowUserTableCommand = new LambdaCommand(OnShowUserTableCommandExecute, CanShowUserTableCommandExecute);
            ShowStaffTableCommand = new LambdaCommand(OnShowStaffTableCommandExecute, CanShowStaffTableCommandExecute);
            ShowGroupTableCommand = new LambdaCommand(OnShowGroupTableCommandExecute, CanShowGroupTableCommandExecute);
            ShowStudentTableCommand = new LambdaCommand(OnShowStudentTableCommandExecute, CanShowStudentTableCommandExecute);
            ShowSubjectTableCommand = new LambdaCommand(OnShowSubjectTableCommandExecute, CanShowSubjectTableCommandExecute);
            ShowSpecializationTableCommand = new LambdaCommand(OnShowSpecializationTableCommandExecute, CanShowSpecializationTableCommandExecute);
            ShowExamTableCommand = new LambdaCommand(OnShowExamTableCommandExecute, CanShowExamTableCommandExecute);
            ShowJournalTableCommand = new LambdaCommand(OnShowJournalTableCommandExecute, CanShowJournalTableCommandExecute);

            ExitUserCommand = new LambdaCommand(OnExitUserCommandExecute, CanExitUserCommandExecute);
        }

        private void init()
        {
            User? currStudent = Ioc.model?.GetCurrentSession();

            _StudentName = currStudent?.Login ?? "";
        }


        #region Команды

        #region Команда показа пользователей
        public ICommand ShowUserTableCommand { get; }
        private bool CanShowUserTableCommandExecute(object parameter) => true;
        private void OnShowUserTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminUserPage();
        }
        #endregion Команда показа пользователей


        #region Команда показа преподавателей
        public ICommand ShowStaffTableCommand { get; }
        private bool CanShowStaffTableCommandExecute(object parameter) => true;
        private void OnShowStaffTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminStaffPage();
        }
        #endregion Команда показа предметов


        #region Команда показа группы
        public ICommand ShowGroupTableCommand { get; }
        private bool CanShowGroupTableCommandExecute(object parameter) => true;
        private void OnShowGroupTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminGroupPage();
        }
        #endregion Команда показа группы


        #region Команда показа студентов
        public ICommand ShowStudentTableCommand { get; }
        private bool CanShowStudentTableCommandExecute(object parameter) => true;
        private void OnShowStudentTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminStudentPage();
        }
        #endregion Команда показа студентов


        #region Команда показа предметов
        public ICommand ShowSubjectTableCommand { get; }
        private bool CanShowSubjectTableCommandExecute(object parameter) => true;
        private void OnShowSubjectTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminSubjectPage();
        }
        #endregion Команда показа предметов


        #region Команда показа специализации
        public ICommand ShowSpecializationTableCommand { get; }
        private bool CanShowSpecializationTableCommandExecute(object parameter) => true;
        private void OnShowSpecializationTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminSpecializationPage();
        }
        #endregion Команда показа специализации

        #region Команда показа экзаменов
        public ICommand ShowExamTableCommand { get; }
        private bool CanShowExamTableCommandExecute(object parameter) => true;
        private void OnShowExamTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminExamPage();
        }
        #endregion Команда показа экзаменов

        #region Команда показа журнала
        public ICommand ShowJournalTableCommand { get; }
        private bool CanShowJournalTableCommandExecute(object parameter) => true;
        private void OnShowJournalTableCommandExecute(object parameter)
        {
            Ioc.AdminNavigationService?.NavigateToAdminJournalPage();
        }
        #endregion Команда показа журнала


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

        private string _StudentName = "";
        public string StudentName
        {
            get => _StudentName;
            set => Set(ref _StudentName, value);
        }

        #endregion Поля
    }
}