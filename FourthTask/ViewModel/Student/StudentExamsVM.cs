using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;


namespace FourthTask.ViewModels
{
    internal class StudentExamsVM : ViewModelBase
    {
        public struct ExamView
        {
            int? examId { get; set; }
            string? examDate { get; set; }
            string? ratingMethod { get; set; }
            int? groupNumber { get; set; }
            string? groupName { get; set; }
            string? teacherName { get; set; }

            public ExamView(int? examId, string? examDate, string? ratingMethod, int? groupNumber, string? groupName, string? teacherName)
            {
                this.examId = examId;
                this.examDate = examDate;
                this.ratingMethod = ratingMethod;
                this.groupNumber = groupNumber;
                this.groupName = groupName;
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

                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(buffExams, exam =>
                    {
                        var task = Task.Run(async () =>
                        {

                            if (exam is null) return;
                            if (exam.SpecializationID is null) return;

                            var buffGroup = await Ioc.model.GetSubject(exam.GroupID ?? -1);

                            var buffSpecialization = await Ioc.model.GetSpecialization(exam.SpecializationID ?? -1);

                            var buffTeacher = await Ioc.model.GetSubject(buffSpecialization.TeacherID ?? -1);

                            lock (Exams)
                            {
                                Exams.Add(new ExamView(
                                    exam.ID,
                                    exam.Date,
                                    exam.RatingMethod,
                                    exam.GroupID,
                                    buffGroup.Name,
                                    buffTeacher.Name
                                    ));
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }
        }


        private ExamView? _SelectedItem;
        public ExamView? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }
    }
}

