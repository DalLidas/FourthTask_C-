using System.ComponentModel;
using System.Runtime.CompilerServices;

using FourthTask.ViewModels.Base;

namespace FourthTask.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _Title = "Trains manager";

        #region ��������� ����

        /// <summary>
        /// ��������� ����
        /// </summary>
        public string Title 
        { 
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion ��������� ����
    }
}