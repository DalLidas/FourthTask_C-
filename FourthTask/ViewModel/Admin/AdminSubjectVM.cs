using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;
using System.Windows;

namespace FourthTask.ViewModels
{
    class AdminSubjectVM : ViewModelBase
    {
        public ObservableCollection<Subject> Subjects { get; set; }
        public ObservableCollection<Subject> AddingSubjects { get; set; }

        public AdminSubjectVM()
        {
            Subjects = new ObservableCollection<Subject>();
            AddingSubjects = new ObservableCollection<Subject>() { new Subject() };

            DeleteSelectedCommand = new LambdaCommand(OnDeleteSelectedCommandExecute, CanDeleteSelectedCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecute, CanRefreshCommandExecute);
            AddNewItemCommand = new LambdaCommand(OnAddNewItemCommandExecute, CanAddNewItemCommandExecute);

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Subject> buffSubjects = await Ioc.model.GetAdminSubject();

                if (buffSubjects is not null)
                {
                    Subjects.Clear();

                    foreach (Subject exam in buffSubjects.OrderBy(x => x.ID))
                    {
                        Subjects.Add(exam);
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
                    if (await Ioc.model.DeleteAdminSubject(_SelectedItem) == 0)
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

            var request = await Ioc.model.GetAdminSubject();

            if (request.Count != Subjects.Count) { MessageBox.Show("Ошибка размерности таблиц полученной и изменённой"); return; }

            for (int i = 0; i < request.Count; ++i)
            {
                if (Subjects.ElementAt(i) != request[i])
                {
                    await Ioc.model.SetAdminSubject(Subjects.ElementAt(i));
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
                if (_AddingItem.Name is null) { errString += " Название не коректно;\n"; }

                if (errString.Length != 0) { MessageBox.Show("Некоректная запись:\n" + errString); return; }

                if (Ioc.model is not null)
                {
                    if (await Ioc.model.SetAdminSubject(_AddingItem) == 0)
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


        private Subject? _SelectedItem;
        public Subject? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }


        private Subject? _AddingItem;
        public Subject? AddingItem
        {
            get => _AddingItem;
            set => Set(ref _AddingItem, value);
        }
    }
}