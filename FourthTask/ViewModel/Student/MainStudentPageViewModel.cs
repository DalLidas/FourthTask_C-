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
using System;

namespace FourthTask.ViewModels
{
    internal class MainStudentPageViewModel : ViewModelBase
    {
        public MainStudentPageViewModel()
        {
            ShowStudentMyGroupCommand = new LambdaCommand(OnShowStudentMyGroupCommandExecute, CanShowStudentMyGroupCommandExecute);
            ShowStudentsSubjectsCommand = new LambdaCommand(OnShowStudentsSubjectsCommandExecute, CanShowStudentsSubjectsCommandExecute);
            ShowStudentExamsCommand = new LambdaCommand(OnShowStudentExamsCommandExecute, CanShowStudentExamsCommandExecute);
            ShowDeanWorkmanCommand = new LambdaCommand(OnShowDeanWorkmanCommandExecute, CanShowDeanWorkmanCommandExecute);

            ExitUserCommand = new LambdaCommand(OnExitUserCommandExecute, CanExitUserCommandExecute);
        }


        #region Команды

        #region Команда показа студентов моей группы
        public ICommand ShowStudentMyGroupCommand { get; }
        private bool CanShowStudentMyGroupCommandExecute(object parameter) => true;
        private void OnShowStudentMyGroupCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentsGroupmatesPage();
        }
        #endregion Команда показа студентов моей группы


        #region Команда показа предметов
        public ICommand ShowStudentsSubjectsCommand { get; }
        private bool CanShowStudentsSubjectsCommandExecute(object parameter) => true;
        private void OnShowStudentsSubjectsCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentsSubjectsPage();
        }
        #endregion Команда показа предметов


        #region Команда показа экзаменов
        public ICommand ShowStudentExamsCommand { get; }
        private bool CanShowStudentExamsCommandExecute(object parameter) => true;
        private void OnShowStudentExamsCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentsExamsPage();
        }
        #endregion Команда показа экзаменов


        #region Команда показа пре
        public ICommand ShowDeanWorkmanCommand { get; }
        private bool CanShowDeanWorkmanCommandExecute(object parameter) => true;
        private void OnShowDeanWorkmanCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToDeanWorkmanPage();
        }
        #endregion Команда показа экзаменов


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