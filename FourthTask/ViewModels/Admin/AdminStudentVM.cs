using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;
using System.Windows;

namespace FourthTask.ViewModels
{
    class AdminStudentVM : ViewModelBase
    {
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Student> AddingStudents { get; set; }

        public AdminStudentVM()
        {
            Students = new ObservableCollection<Student>();
            AddingStudents = new ObservableCollection<Student>() { new Student() };

            DeleteSelectedCommand = new LambdaCommand(OnDeleteSelectedCommandExecute, CanDeleteSelectedCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecute, CanRefreshCommandExecute);
            AddNewItemCommand = new LambdaCommand(OnAddNewItemCommandExecute, CanAddNewItemCommandExecute);

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Student> buffStudents = await Ioc.model.GetAdminStudent();

                if (buffStudents is not null)
                {
                    Students.Clear();

                    foreach (Student exam in buffStudents.OrderBy(x => x.ID))
                    {
                        Students.Add(exam);
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
                    if (await Ioc.model.DeleteAdminStudent(_SelectedItem) == 0)
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

            var request = await Ioc.model.GetAdminStudent();

            if (request.Count != Students.Count) { MessageBox.Show("Ошибка размерности таблиц полученной и изменённой"); return; }

            for (int i = 0; i < request.Count; ++i)
            {
                if (Students.ElementAt(i) != request[i])
                {
                    await Ioc.model.SetAdminStudent(Students.ElementAt(i));
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
                if (_AddingItem.FullName is null) { errString += "ФИО не коректно;\n"; }
                if (_AddingItem.Birth is null) { errString += "Год рождения не коректен;\n"; }
                if (_AddingItem.GroupID is null) { errString += "ID группы не коректен;\n"; }
                if (_AddingItem.AdmissionDate is null) { errString += "Дата зачисления не коректна;\n"; }
                if (_AddingItem.Data is null) { errString += "Информация не коректна;\n"; }

                if (errString.Length != 0) { MessageBox.Show("Некоректная запись:\n" + errString); return; }

                if (Ioc.model is not null)
                {
                    if (await Ioc.model.SetAdminStudent(_AddingItem) == 0)
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


        private Student? _SelectedItem;
        public Student? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }


        private Student? _AddingItem;
        public Student? AddingItem
        {
            get => _AddingItem;
            set => Set(ref _AddingItem, value);
        }
    }
}