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
            ShowStudentGroupmatesCommand = new LambdaCommand(OnShowStudentGroupmatesCommandExecute, CanShowStudentGroupmatesCommandExecute);
            ShowStudentTeachersCommand = new LambdaCommand(OnShowStudentTeachersCommandExecute, CanShowStudentTeachersCommandExecute);
            ShowStudentExamsCommand = new LambdaCommand(OnShowStudentExamsCommandExecute, CanShowStudentExamsCommandExecute);

            ExitUserCommand = new LambdaCommand(OnExitUserCommandExecute, CanExitUserCommandExecute);
        }


        #region Команды

        #region Команда показа студентов моей группы
        public ICommand ShowStudentGroupmatesCommand { get; }
        private bool CanShowStudentGroupmatesCommandExecute(object parameter) => true;
        private void OnShowStudentGroupmatesCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentGroupmatesPage();
        }
        #endregion Команда показа студентов моей группы


        #region Команда показа преподавателей
        public ICommand ShowStudentTeachersCommand { get; }
        private bool CanShowStudentTeachersCommandExecute(object parameter) => true;
        private void OnShowStudentTeachersCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentTeachersPage();
        }
        #endregion Команда показа предметов


        #region Команда показа экзаменов
        public ICommand ShowStudentExamsCommand { get; }
        private bool CanShowStudentExamsCommandExecute(object parameter) => true;
        private void OnShowStudentExamsCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentExamsPage();
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