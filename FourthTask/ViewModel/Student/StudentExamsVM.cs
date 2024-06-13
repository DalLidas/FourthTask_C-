//using FourthTask.DataBase;
//using FourthTask.PageNavigation.Ioc;
//using FourthTask.ViewModels.Base;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FourthTask.ViewModel.Student
//{
//    class StudentExamsVM
//    {
//        public ObservableCollection<ExamsView> Exams { get; set; }

//        public StudentExamsVM()
//        {
//            Exams = new ObservableCollection<ExamsView>();

//            GetData();
//        }


//        private async void GetData()
//        {
//            if (Ioc.model is not null)
//            {
//                List<Student> buffStudent = await Ioc.model.GetStudentsGroupmates();
//                if (buffStudent is not null)
//                {
//                    Groupmates.Clear();
//                    foreach (Student groupmate in buffStudent.OrderBy(x => x.ID))
//                    {
//                        Groupmates.Add(groupmate);
//                    }
//                }
//            }
//        }


//        private Student? _SelectedItem;
//        public Student? SelectedItem
//        {
//            get => _SelectedItem;
//            set => Set(ref _SelectedItem, value);
//        }
//    }

//    public struct ExamsView
//    {

//    } 
//}

