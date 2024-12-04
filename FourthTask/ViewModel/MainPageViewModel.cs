using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using FourthTask.ViewModels.Commands;
using System.IO;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace FourthTask.ViewModels
{
    internal class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Subject> subjects { get; set; }
        public ObservableCollection<Student> myGroup { get; set; }

        //private Dictionary<string, bool> visibleTableFlags { get; set; }
        


        public MainPageViewModel()
        {
            subjects = new ObservableCollection<Subject>();
            myGroup = new ObservableCollection<Student>();

            //visibleTableFlags =
            //[
            //    _StudentMyGroupVisibleFlag,
            //    _StudentMyExamsVisibleFlag,
            //    _SubjectVisibleFlag,
            //];

            ShowSubjectCommand = new LambdaCommand(OnShowSubjectCommandExecute, CanShowSubjectCommandExecute);
            ExitUserCommand = new LambdaCommand(OnExitUserCommandExecute, CanExitUserCommandExecute);
        }


        #region �������

        #region ������� ������ ��������� ���� ������
        public ICommand ShowSubjectCommand { get; }

        private bool CanShowSubjectCommandExecute(object parameter) => true;
        private async void OnShowSubjectCommandExecute(object parameter)
        {
            if (Ioc.model is not null)
            {
                List<Subject> buffSubjects = await Ioc.model.GetSubjects();
                if (buffSubjects is not null)
                {
                    subjects.Clear();
                    foreach (Subject subject in buffSubjects.OrderBy(x => x.ID))
                    {
                        subjects.Add(subject);
                    }
                }
            }

            Ioc.StudentNavigationService?.NavigateToStudentGroupmatesPage();
        }
        #endregion ������� ������ ��������� ���� ������

       
       
        #region ������� ������ �� ������� ������
        public ICommand ExitUserCommand { get; }

        private bool CanExitUserCommandExecute(object parameter) => true;
        private void OnExitUserCommandExecute(object parameter)
        {
            Ioc.model?.CloseSession();
            Ioc.MainNavigationService?.NavigateToAuthorizationPage();
        }
        #endregion ������� ������ �� ������� ������

        #endregion �������


        #region ����

        private string _Title = "��������������� ��� \"������\"";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion ����
    }
}