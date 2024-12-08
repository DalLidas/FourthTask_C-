using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Linq;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;

namespace FourthTask.ViewModels
{
    internal class TeacherGroupsVM : ViewModelBase
    {
        public ObservableCollection<Group> groups { get; set; }

        public TeacherGroupsVM()
        {
            groups = new ObservableCollection<Group>();

            TeacherEditGradesCommand = new LambdaCommand(OnTeacherEditGradesCommandExecute, CanTeacherEditGradesCommandExecute);

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                var buffgroups = await Ioc.model.GetTeacherGroups();

                if (buffgroups is not null)
                {
                    groups.Clear();

                    List<int> uniqueGroup = new List<int>();

                    foreach (Group group in buffgroups.OrderBy(x => x.ID))
                    {
                        if (group is not null && !uniqueGroup.Contains(group.ID)) {
                            uniqueGroup.Add(group.ID);
                            groups.Add(group);
                        }
                    }
                }
            }
        }

        #region Команды

        #region Команда показа студентов моей группы
        public ICommand TeacherEditGradesCommand { get; }
        private bool CanTeacherEditGradesCommandExecute(object parameter) => true;
        private void OnTeacherEditGradesCommandExecute(object parameter)
        {
            Ioc.StudentNavigationService?.NavigateToStudentGroupmatesPage();
        }
        #endregion Команда показа студентов моей группы

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
    }
}