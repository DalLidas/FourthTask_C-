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
            #region �������

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecute);

            #endregion �������
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

        #region ��������� ����
        private string _Title = "Trains manager";

        /// <summary>
        /// ��������� ����
        /// </summary>
        public string Title 
        { 
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion ��������� ����

        #region ������ ����
        private string _Status = "Working hard";

        /// <summary>
        /// ��������� ����
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion ������ ����

        #endregion ����
    }
}