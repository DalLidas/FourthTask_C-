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

namespace FourthTask.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            
        }

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