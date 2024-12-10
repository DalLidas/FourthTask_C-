using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;
using System.Linq;
using System.Windows.Input;
using FourthTask.ViewModels.Commands;
using System.Security.Policy;
using System.Windows;
using System.ComponentModel;

namespace FourthTask.ViewModels
{
    class TeacherGradesVM : ViewModelBase
    {
        public struct SubjectView
        {
            public int? jornalId { get; set; }
            public string? subjectName { get; set; }
            public string? date { get; set; }
            public string? subjectGrade { get; set; }

            public SubjectView(int? jornalId, string? subjectName, string? date, string? subjectGrade)
            {
                this.jornalId = jornalId;
                this.subjectName = subjectName;
                this.date = date;
                this.subjectGrade = subjectGrade;
            }
        }


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
        public ObservableCollection<StudentGradeView> studentsView { get; set; }

        public TeacherGradesVM()
        {
            students = new ObservableCollection<StudentGradeView>();
            studentsView = new ObservableCollection<StudentGradeView>();

            TeacherUpdateGradesCommand = new LambdaCommand(OnTeacherUpdateGradesCommandExecute, CanTeacherUpdateGradesCommandExecute);

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
                                    buffJornal?.ID,
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

                    studentsView.Clear();
                    for (int i = 0; i < students.Count; ++i)
                    {
                        studentsView.Add(students[i]);
                    }
                }
            }
        }

        #region Команды

        #region Команда показа студентов моей группы
        public ICommand TeacherUpdateGradesCommand { get; }
        private bool CanTeacherUpdateGradesCommandExecute(object parameter) => true;
        private void OnTeacherUpdateGradesCommandExecute(object parameter)
        {
            string str1;
            string str2;

            str1 = students[0].fullName + ": (";
            for (int i = 1; i < students[0].subjects.Count; ++i)
            {
                str1 += students[0].subjects[i]?.subjectName + " " + students[0].subjects[i]?.subjectGrade + "\n";
            }
            str1 += ")\n";

            str2 = students[0].fullName + ": (";
            for (int i = 0; i < studentsView[0].subjects.Count; ++i)
            {
                str2 += studentsView[0].subjects[i]?.subjectName + " " + studentsView[0].subjects[i]?.subjectGrade + "\n";
            }
            str2 += ")\n";


            MessageBox.Show("STR1: "+str1 + "\n\n\n" + "STR2: " + str2 + "\n\n\n");
        }
        #endregion Команда показа студентов моей группы

        #endregion Команды

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
