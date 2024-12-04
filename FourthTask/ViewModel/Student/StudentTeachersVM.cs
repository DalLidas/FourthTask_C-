using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Threading;
using System.Threading.Tasks;


namespace FourthTask.ViewModels
{
    class StudentTeachersVM: ViewModelBase
    {
        public struct TeacherView
        {
            public string? fullName { get; set; }
            public string? birth { get; set; }
            public string? job { get; set; }
            public string? subject { get; set; }
            public string? Merit { get; set; }
            public int? Internship { get; set; }

            public TeacherView(string? fullName, string? birth, string? job,  string? subject, string? Merit, int? Internship)
            {
                this.fullName = fullName;
                this.birth = birth;
                this.job = job;
                this.subject = subject;
                this.Merit = Merit;
                this.Internship = Internship;
            }
        }


        public ObservableCollection<TeacherView> Teachers { get; set; }

        public StudentTeachersVM()
        {
            Teachers = new ObservableCollection<TeacherView>();

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Exam> buffExams = await Ioc.model.GetExams();

                if (buffExams is not null)
                {
                    Teachers.Clear();

                    List<int> uniqueTeachers = new List<int>();

                    foreach (Exam exam in buffExams.OrderBy(x => x.ID))
                    {
                        if (exam is null) continue;
                        if (exam.SpecializationID is null) continue;

                        var buffSpecialization = await Ioc.model.GetSpecialization(exam.SpecializationID ?? -1) ?? null;

                        if (buffSpecialization is null) continue;

                        var buffTeacher = await Ioc.model.GetTeacher(buffSpecialization.TeacherID ?? -1) ?? null;
                        var buffsubject = await Ioc.model.GetSubject(buffSpecialization.SubjectID ?? -1) ?? null;

                        if (buffTeacher?.ID is null) continue;
                        if (uniqueTeachers.Contains(buffTeacher?.ID ?? 0)) continue;

                        uniqueTeachers.Add(buffTeacher.ID);
                        Teachers.Add(new TeacherView(
                                 buffTeacher?.FullName ?? "",
                                 buffTeacher?.Birth ?? "",
                                 buffTeacher?.Job ?? "",
                                 buffsubject?.Name ?? "",
                                 buffTeacher?.Merit ?? "",
                                 buffTeacher?.Internship ?? 0
                                 ));
                    }
                }
            }
        }


        private TeacherView? _SelectedItem;
        public TeacherView? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }
    }
}
