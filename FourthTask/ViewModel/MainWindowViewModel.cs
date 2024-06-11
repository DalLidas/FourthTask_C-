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

        private ObservableCollection<TrainRoat> _trainRoats;
        public MainWindowViewModel()
        {
            _trainRoats = new ObservableCollection<TrainRoat>();

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecute);
        }


        #region �������

        #region ������� �������� ����
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object parameter) => true;
        private void OnCloseApplicationCommandExecute(object parameter) 
        {
            Application.Current.Shutdown();
        }
        #endregion ������� �������� ����

        #endregion �������


        #region ����

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


        #endregion ����
    }
}