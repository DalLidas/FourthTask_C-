using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Linq;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;
using System.Security.Policy;

namespace FourthTask.ViewModels
{
    class TeacherGradesVM : ViewModelBase
    {
        public struct SubjectView
        {
            public int? subjectId { get; set; }
            public string? subjectName { get; set; }
            public string? date { get; set; }
            public string? subjectGrade { get; set; }

            public SubjectView(int? subjectId, string? subjectName, string? date, string? subjectGrade)
            {
                this.subjectId = subjectId;
                this.subjectName = subjectName;
                this.date = date;
                this.subjectGrade = subjectGrade;
            }
        };

        public struct StudentGradeView
        {
            public int? id { get; set; }
            public string? fullName { get; set; }
            public string? birth { get; set; }
            public int? groupID { get; set; }
            public List<SubjectView?>? subjects { get; set; }

            public StudentGradeView(int? id, string? fullName, string? birth, int? groupID, List<SubjectView?>? subjects)
            {
                this.id = id;
                this.fullName = fullName;
                this.birth = birth;
                this.groupID = groupID;
                this.subjects = subjects;
            }
        }

        public ObservableCollection<StudentGradeView> students { get; set; }

        public TeacherGradesVM()
        {
            students = new ObservableCollection<StudentGradeView>();

            //TeacherEditGradesCommand = new LambdaCommand(OnTeacherEditGradesCommandExecute, CanTeacherEditGradesCommandExecute);

            GetData();
        }


        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                var buffStudents = await Ioc.model.GetTeacherStudents();
                var buffexams = await Ioc.model.GetTeacherStudentSubjects();

                if (buffStudents is not null && buffexams is not null)
                {
                    students.Clear();

                    foreach (Student student in buffStudents.OrderBy(x => x.ID))
                    {
                        var subjects = new List<SubjectView?>();

                        foreach (Exam exam in buffexams.OrderBy(x => x.ID))
                        {
                            if (exam is null) continue;
                            if (exam.SpecializationID is null) continue;

                            var buffSpecialization = await Ioc.model.GetStudentSpecialization(exam.SpecializationID ?? -1) ?? null;

                            if (buffSpecialization is null) continue;

                            var buffTeacher = await Ioc.model.GetStudentTeacher(buffSpecialization.TeacherID ?? -1) ?? null;
                            var buffsubject = await Ioc.model.GetStudentSubject(buffSpecialization.SubjectID ?? -1) ?? null;

                            var buffJornal = await Ioc.model.GetStudentGrade(exam.ID);

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

                            lock (subjects)
                            {
                                subjects.Add(new SubjectView(
                                    buffsubject?.ID,
                                    buffsubject?.Name,
                                    exam.Date,
                                    grade
                                    ));
                            }
                        }


                        if (student is not null)
                        {
                            students.Add(new StudentGradeView(
                                student.ID,
                                student.FullName,
                                student.Birth,
                                student.GroupID,
                                subjects.Count != 0 ? subjects : null
                            ));
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


        //private Group? _SelectedItem;
        //public Group? SelectedItem
        //{
        //    get => _SelectedItem;
        //    set
        //    {
        //        Ioc.model?.setTeacherSelectedGroup(value?.ID);
        //        Set(ref _SelectedItem, value);
        //    }
        //}
    }
}
