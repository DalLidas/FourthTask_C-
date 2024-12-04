using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;

namespace FourthTask.ViewModels
{
    internal class TeacherMainPageVM : ViewModelBase
    {
        public TeacherMainPageVM()
        {
            init();

            ShowTeacherGroupsCommand = new LambdaCommand(OnShowTeacherGroupsCommandExecute, CanShowTeacherGroupsCommandExecute);

            ExitUserCommand = new LambdaCommand(OnExitUserCommandExecute, CanExitUserCommandExecute);
        }


        private void init()
        {
            var currStaff = Ioc.model?.GetCurrentStaff();

            _StaffName = currStaff?.FullName ?? "";
            _StaffBirth = currStaff?.Birth ?? "";
            _StaffMerit = currStaff?.Merit ?? "";
            _StaffJob = currStaff?.Job ?? "";
            _StaffInternship = currStaff?.Internship ?? 0;
        }


        #region Команды

        #region Команда показа групп со студентами
        public ICommand ShowTeacherGroupsCommand { get; }
        private bool CanShowTeacherGroupsCommandExecute(object parameter) => true;
        private void OnShowTeacherGroupsCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentGroupmatesPage();
        }
        #endregion Команда показа групп со студентами


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

        private string _StaffName = "";
        public string StaffName
        {
            get => _StaffName;
            set => Set(ref _StaffName, value);
        }

        private string _StaffBirth = "";
        public string StaffBirth
        {
            get => _StaffBirth;
            set => Set(ref _StaffBirth, value);
        }

        private string _StaffMerit = "";
        public string StaffMerit
        {
            get => _StaffMerit;
            set => Set(ref _StaffMerit, value);
        }

        private string _StaffJob = "";
        public string StaffJob
        {
            get => _StaffJob;
            set => Set(ref _StaffJob, value);
        }

        private int _StaffInternship = 0;
        public int StaffInternship
        {
            get => _StaffInternship;
            set => Set(ref _StaffInternship, value);
        }


        private Student? _SelectedItem;
        public Student? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }

        #endregion Поля
    }
}
