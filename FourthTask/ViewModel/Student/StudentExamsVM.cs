using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;


namespace FourthTask.ViewModels
{
    internal class StudentExamsVM: ViewModelBase
    {
        public ObservableCollection<ExamsView> Exams { get; set; }

        public StudentExamsVM()
        {
            Exams = new ObservableCollection<ExamsView>();

            GetData();
        }


        private async void GetData()
        {
            //if (Ioc.model is not null)
            //{
            //    List<Student> buffStudent = await Ioc.model.GetStudentsGroupmates();
            //    if (buffStudent is not null)
            //    {
            //        Groupmates.Clear();
            //        foreach (Student groupmate in buffStudent.OrderBy(x => x.ID))
            //        {
            //            Groupmates.Add(groupmate);
            //        }
            //    }
            //}
        }


        private Student? _SelectedItem;
        public Student? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }
    }

    public struct ExamsView
    {

    }
}

