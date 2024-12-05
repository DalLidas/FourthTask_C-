using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Linq;

namespace FourthTask.ViewModels
{
    internal class TeacherGroupsVM : ViewModelBase
    {
        public ObservableCollection<Group> groups { get; set; }

        public TeacherGroupsVM()
        {
            groups = new ObservableCollection<Group>();

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


        private string _Title = "Технологический ВУЗ \"Сессия\"";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private bool _EditButtonIsEnableFlag = false;
        public bool EditButtonIsEnableFlag
        {
            get => _EditButtonIsEnableFlag;
            set => Set(ref _EditButtonIsEnableFlag, value);
        }


        private Group? _SelectedItem;
        public Group? SelectedItem
        {
            get => _SelectedItem;
            set {
                if (value != null)
                {
                    _EditButtonIsEnableFlag = true;
                    Set(ref _EditButtonIsEnableFlag, true);
                }
                Set(ref _SelectedItem, value);
            }


        }
    }
}