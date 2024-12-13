using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;
using System.Windows;

namespace FourthTask.ViewModels
{
    class AdminGroupVM : ViewModelBase
    {
        public ObservableCollection<Group> Groups { get; set; }
        public ObservableCollection<Group> AddingGroups { get; set; }

        public AdminGroupVM()
        {
            Groups = new ObservableCollection<Group>();
            AddingGroups = new ObservableCollection<Group>() { new Group() };

            DeleteSelectedCommand = new LambdaCommand(OnDeleteSelectedCommandExecute, CanDeleteSelectedCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecute, CanRefreshCommandExecute);
            AddNewItemCommand = new LambdaCommand(OnAddNewItemCommandExecute, CanAddNewItemCommandExecute);

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Group> buffGroups = await Ioc.model.GetAdminGroup();

                if (buffGroups is not null)
                {
                    Groups.Clear();

                    foreach (Group exam in buffGroups.OrderBy(x => x.ID))
                    {
                        Groups.Add(exam);
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
                    if (await Ioc.model.DeleteAdminGroup(_SelectedItem) == 0)
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

            var request = await Ioc.model.GetAdminGroup();

            if (request.Count != Groups.Count) { MessageBox.Show("Ошибка размерности таблиц полученной и изменённой"); return; }

            for (int i = 0; i < request.Count; ++i)
            {
                if (Groups.ElementAt(i) != request[i])
                {
                    await Ioc.model.SetAdminGroup(Groups.ElementAt(i));
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
                if (_AddingItem.Faculty is null) { errString += "Факультет не коректен;\n"; }
                if (_AddingItem.Name is null) { errString += "Название группы не коректено;\n"; }
               
                if (errString.Length != 0) { MessageBox.Show("Некоректная запись:\n" + errString); return; }

                if (Ioc.model is not null)
                {
                    if (await Ioc.model.SetAdminGroup(_AddingItem) == 0)
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


        private Group? _SelectedItem;
        public Group? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }


        private Group? _AddingItem;
        public Group? AddingItem
        {
            get => _AddingItem;
            set => Set(ref _AddingItem, value);
        }
    }
}