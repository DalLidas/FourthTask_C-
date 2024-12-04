using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Threading;
using System.Threading.Tasks;
using System.Printing;


namespace FourthTask.ViewModels
{
    internal class StudentExamsVM : ViewModelBase
    {
        public struct ExamView
        {
            public string? subjectName { get; set; }
            public string? examDate { get; set; }
            public string? ratingMethod { get; set; }
            public string? grade { get; set; }
            public string? teacherName { get; set; }

            public ExamView(string? subjectName, string? examDate, string? ratingMethod, string? grade, string? teacherName)
            {
                this.subjectName = subjectName;
                this.examDate = examDate;
                this.ratingMethod = ratingMethod;
                this.grade = grade;
                this.teacherName = teacherName;
            }
        }


        public ObservableCollection<ExamView> Exams { get; set; }

        public StudentExamsVM()
        {
            Exams = new ObservableCollection<ExamView>();

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Exam> buffExams = await Ioc.model.GetExams();

                if (buffExams is not null)
                {
                    Exams.Clear();

                    foreach (Exam exam in buffExams.OrderBy(x => x.ID))
                    {
                        if (exam is null) continue;
                        if (exam.SpecializationID is null) continue;

                        var buffSpecialization = await Ioc.model.GetSpecialization(exam.SpecializationID ?? -1) ?? null;

                        if (buffSpecialization is null) continue;

                        var buffTeacher = await Ioc.model.GetTeacher(buffSpecialization.TeacherID ?? -1) ?? null;
                        var buffsubject = await Ioc.model.GetSubject(buffSpecialization.SubjectID ?? -1) ?? null;

                        var buffJornal = await Ioc.model.GetGrade(exam.ID);

                        string grade = "";

                        if (exam.RatingMethod == "Экзамен") grade = buffJornal.Grade.ToString() ?? "";
                        else if (exam.RatingMethod == "Зачёт")
                        {
                            if (buffJornal.Grade == 0)
                            {
                                grade = "Не зачёт";
                            }
                            else if (buffJornal.Grade == 1)
                            {
                                grade = "Зачёт";

                            }
                            else
                            {
                                grade = "";
                            }
                        }
                        else
                        {
                            grade = "";
                        }

                        Exams.Add(new ExamView(
                                 buffsubject?.Name ?? "",
                                 exam.Date,
                                 exam.RatingMethod,
                                 grade,
                                 buffTeacher?.FullName ?? ""
                                 ));
                    }


                    //SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
                    //List<Task> tasks = new List<Task>();

                    //foreach (var exam in buffExams)
                    //{
                    //    var task = Task.Run(async () =>
                    //    {
                    //        if (exam is null) return;
                    //        if (exam.SpecializationID is null) return;

                    //        var buffGroup = await Ioc.model.GetGroup(exam.GroupID ?? -1);

                    //        var buffSpecialization = await Ioc.model.GetSpecialization(exam.SpecializationID ?? -1) ;

                    //        if (buffSpecialization is null) return;

                    //        var buffTeacher = await Ioc.model.GetTeacher(buffSpecialization.TeacherID ?? -1) ;
                    //        var buffsubject = await Ioc.model.GetSubject(buffSpecialization.SubjectID ?? -1) ;

                    //        await semaphore.WaitAsync();
                    //        try
                    //        {
                    //            Exams.Add(new ExamView(
                    //                exam.ID,
                    //                exam.Date,
                    //                exam.RatingMethod,
                    //                exam.GroupID,
                    //                //"buffGroup",
                    //                //"buffsubject",
                    //                //"buffTeacher"
                    //                buffGroup.Name,
                    //                buffsubject?.Name ?? "",
                    //                buffTeacher?.FullName ?? ""
                    //                ));
                    //        }
                    //        finally
                    //        {
                    //            semaphore.Release();
                    //        }
                    //    });

                    //    tasks.Add(task);
                    //}

                    // Выполнение всех тасков
                    //await Task.WhenAll(tasks.ToArray());
                }
            }
        }


        private string _Title = "Технологический ВУЗ \"Сессия\"";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }


        private ExamView? _SelectedItem;
        public ExamView? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }
    }
}

