using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FourthTask.ViewModels.Base;
using FourthTask.ViewModels.Commands;

namespace FourthTask.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecute);

            #endregion Команды
        }


        #region Команды

        #region Команда закрытия окна
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object parameter) => true;
        private void OnCloseApplicationCommandExecute(object parameter) 
        {
            Application.Current.Shutdown();
        }
        #endregion Команда закрытия окна

        #endregion Команды


        #region Поля

        private string _Title = "Trains manager";
        public string Title 
        { 
            get => _Title;
            set => Set(ref _Title, value);
        }


        private string _Status = "Working hard";
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        #endregion Поля
    }
}