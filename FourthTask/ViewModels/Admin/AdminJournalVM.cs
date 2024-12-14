using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;
using System.Windows;

namespace FourthTask.ViewModels
{
    class AdminJournalVM : ViewModelBase
    {
        public ObservableCollection<Journal> Journals { get; set; }
        public ObservableCollection<Journal> AddingJournals { get; set; }

        public AdminJournalVM()
        {
            Journals = new ObservableCollection<Journal>();
            AddingJournals = new ObservableCollection<Journal>() { new Journal() };

            DeleteSelectedCommand = new LambdaCommand(OnDeleteSelectedCommandExecute, CanDeleteSelectedCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecute, CanRefreshCommandExecute);
            AddNewItemCommand = new LambdaCommand(OnAddNewItemCommandExecute, CanAddNewItemCommandExecute);

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Journal> buffJournals = await Ioc.model.GetAdminJournal();

                if (buffJournals is not null)
                {
                    Journals.Clear();

                    foreach (Journal exam in buffJournals.OrderBy(x => x.ID))
                    {
                        Journals.Add(exam);
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
                    if (await Ioc.model.DeleteAdminJournal(_SelectedItem) == 0)
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

            var request = await Ioc.model.GetAdminJournal();

            if (request.Count != Journals.Count) { MessageBox.Show("Ошибка размерности таблиц полученной и изменённой"); return; }

            for (int i = 0; i < request.Count; ++i)
            {
                if (Journals.ElementAt(i) != request[i])
                {
                    await Ioc.model.SetAdminJournal(Journals.ElementAt(i));
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
                if (_AddingItem.ExamID is null) { errString += "ID экзамена не коректно;\n"; }
                if (_AddingItem.StudentID is null) { errString += "ID студента не коректено;\n"; }
                if (_AddingItem.Grade is null) { errString += "Оценка не коректена;\n"; }
               
                if (errString.Length != 0) { MessageBox.Show("Некоректная запись:\n" + errString); return; }

                if (Ioc.model is not null)
                {
                    if (await Ioc.model.SetAdminJournal(_AddingItem) == 0)
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


        private Journal? _SelectedItem;
        public Journal? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }


        private Journal? _AddingItem;
        public Journal? AddingItem
        {
            get => _AddingItem;
            set => Set(ref _AddingItem, value);
        }
    }
}