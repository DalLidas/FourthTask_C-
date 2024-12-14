using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;
using System.Windows;

namespace FourthTask.ViewModels
{
    class AdminStaffVM : ViewModelBase
    {
        public ObservableCollection<Staff> Staff { get; set; }
        public ObservableCollection<Staff> AddingStaff { get; set; }

        public AdminStaffVM()
        {
            Staff = new ObservableCollection<Staff>();
            AddingStaff = new ObservableCollection<Staff>() { new Staff() };

            DeleteSelectedCommand = new LambdaCommand(OnDeleteSelectedCommandExecute, CanDeleteSelectedCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecute, CanRefreshCommandExecute);
            AddNewItemCommand = new LambdaCommand(OnAddNewItemCommandExecute, CanAddNewItemCommandExecute);

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Staff> buffStaff = await Ioc.model.GetAdminStaff();

                if (buffStaff is not null)
                {
                    Staff.Clear();

                    foreach (Staff exam in buffStaff.OrderBy(x => x.ID))
                    {
                        Staff.Add(exam);
                    }
                }
            }
        }


        #region Команды

        #region Команда для удаления выделенного поля
        public ICommand DeleteSelectedCommand { get; }
        private bool CanDeleteSelectedCommandExecute(object parameter) => true;
        private async void OnDeleteSelectedCommandExecute(object parameter)
        {
            if (_SelectedItem is not null)
            {
                if (Ioc.model is not null)
                {
                    if (await Ioc.model.DeleteAdminStaff(_SelectedItem) == 0)
                    {
                        MessageBox.Show("Предупреждение. Удаление записи проведено не успешно.");
                    }
                    else
                    {
                        MessageBox.Show("Удаление записи проведено успешно.");
                    }
                }
            }

            GetData();
        }
        #endregion Команда для удаления выделенного поля

        #region Команда обновления таблицы
        public ICommand RefreshCommand { get; }
        private bool CanRefreshCommandExecute(object parameter) => true;
        private async void OnRefreshCommandExecute(object parameter)
        {
            if (Ioc.model is null) { MessageBox.Show("Ошибка инициализации модели"); return; }

            var request = await Ioc.model.GetAdminStaff();

            if (request.Count != Staff.Count) { MessageBox.Show("Ошибка размерности таблиц полученной и изменённой"); return; }

            for (int i = 0; i < request.Count; ++i)
            {
                if (Staff.ElementAt(i) != request[i])
                {
                    await Ioc.model.SetAdminStaff(Staff.ElementAt(i));
                }
            }

            GetData();
        }
        #endregion Команда обновления таблицы

        #region Команда обновления таблицы
        public ICommand AddNewItemCommand { get; }
        private bool CanAddNewItemCommandExecute(object parameter) => true;
        private async void OnAddNewItemCommandExecute(object parameter)
        {
            if (_AddingItem is not null)
            {
                string errString = "";
                if (_AddingItem.FullName is null) { errString += "Фио не коректно;\n"; }
                if (_AddingItem.Birth is null) { errString += "Год рождения не коректен;\n"; }
                if (_AddingItem.Job is null) { errString += "Должность не коректена;\n"; }
                if (_AddingItem.Merit is null) { errString += "Заслуги не коректны;\n"; }
                if (_AddingItem.Internship is null) { errString += "Стаж не коректен;\n"; }

                if (errString.Length != 0) { MessageBox.Show("Некоректная запись:\n" + errString); return; }

                if (Ioc.model is not null)
                {
                    if (await Ioc.model.SetAdminStaff(_AddingItem) == 0)
                    {
                        MessageBox.Show("Предупреждение. Добавление записи проведено не успешно.");
                    }
                    else
                    {
                        MessageBox.Show("Добавление записи проведено успешно.");
                    }
                }
            }

            GetData();
        }
        #endregion Команда обновления таблицы

        #endregion Команды


        private string _Title = "Технологический ВУЗ \"Сессия\"";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }


        private Staff? _SelectedItem;
        public Staff? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }


        private Staff? _AddingItem;
        public Staff? AddingItem
        {
            get => _AddingItem;
            set => Set(ref _AddingItem, value);
        }
    }
}