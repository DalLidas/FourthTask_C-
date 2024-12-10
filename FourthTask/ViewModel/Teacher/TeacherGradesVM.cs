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
        public class SubjectViewWrapper : INotifyPropertyChanged
        {
            private int? _jornalId;
            private int? _examId;
            private string? _ratingMethod;
            private string? _subjectName;
            private string? _date;
            private string? _subjectGrade;

            public int? JornalId
            {
                get => _jornalId;
                set
                {
                    if (_jornalId != value)
                    {
                        _jornalId = value;
                        OnPropertyChanged(nameof(JornalId));
                    }
                }
            }
            public int? ExamId
            {
                get => _examId;
                set
                {
                    if (_examId != value)
                    {
                        _examId = value;
                        OnPropertyChanged(nameof(ExamId));
                    }
                }
            }

            public string? RatingMethod
            {
                get => _ratingMethod;
                set
                {
                    if (_ratingMethod != value)
                    {
                        _ratingMethod = value;
                        OnPropertyChanged(nameof(RatingMethod));
                    }
                }
            }

            public string? SubjectName
            {
                get => _subjectName;
                set
                {
                    if (_subjectName != value)
                    {
                        _subjectName = value;
                        OnPropertyChanged(nameof(SubjectName));
                    }
                }
            }

            public string? Date
            {
                get => _date;
                set
                {
                    if (_date != value)
                    {
                        _date = value;
                        OnPropertyChanged(nameof(Date));
                    }
                }
            }

            public string? SubjectGrade
            {
                get => _subjectGrade;
                set
                {
                    if (_subjectGrade != value)
                    {
                        _subjectGrade = value;
                        OnPropertyChanged(nameof(SubjectGrade));
                    }
                }
            }

            public SubjectViewWrapper(int? jornalId, int? examId, string? ratingMethod, string? subjectName, string? date, string? subjectGrade)
            {
                _jornalId = jornalId;
                _examId = examId;
                _ratingMethod = ratingMethod;
                _subjectName = subjectName;
                _date = date;
                _subjectGrade = subjectGrade;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class StudentGradeViewWrapper : INotifyPropertyChanged
        {
            private int? _id;
            private string? _fullName;
            private string? _birth;
            private int? _groupID;
            private ObservableCollection<SubjectViewWrapper>? _subjects;

            public int? Id
            {
                get => _id;
                set
                {
                    if (_id != value)
                    {
                        _id = value;
                        OnPropertyChanged(nameof(Id));
                    }
                }
            }

            public string? FullName
            {
                get => _fullName;
                set
                {
                    if (_fullName != value)
                    {
                        _fullName = value;
                        OnPropertyChanged(nameof(FullName));
                    }
                }
            }

            public string? Birth
            {
                get => _birth;
                set
                {
                    if (_birth != value)
                    {
                        _birth = value;
                        OnPropertyChanged(nameof(Birth));
                    }
                }
            }

            public int? GroupID
            {
                get => _groupID;
                set
                {
                    if (_groupID != value)
                    {
                        _groupID = value;
                        OnPropertyChanged(nameof(GroupID));
                    }
                }
            }

            public ObservableCollection<SubjectViewWrapper>? Subjects
            {
                get => _subjects;
                set
                {
                    if (_subjects != value)
                    {
                        _subjects = value;
                        OnPropertyChanged(nameof(Subjects));
                    }
                }
            }

            public StudentGradeViewWrapper(int? id, string? fullName, string? birth, int? groupID, List<SubjectViewWrapper>? subjects)
            {
                _id = id;
                _fullName = fullName;
                _birth = birth;
                _groupID = groupID;
                _subjects = new ObservableCollection<SubjectViewWrapper>(subjects ?? new List<SubjectViewWrapper>());
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ObservableCollection<StudentGradeViewWrapper> students { get; set; }

        public TeacherGradesVM()
        {
            students = new ObservableCollection<StudentGradeViewWrapper>();

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
                        var subjects = new List<SubjectViewWrapper?>();

                        foreach (Exam exam in buffexams.OrderBy(x => x.ID))
                        {
                            if (exam is null) continue;
                            if (exam.SpecializationID is null) continue;

                            var buffSpecialization = await Ioc.model.GetStudentSpecialization(exam.SpecializationID ?? -1) ?? null;

                            if (buffSpecialization is null) continue;

                            var buffTeacher = await Ioc.model.GetStudentTeacher(buffSpecialization.TeacherID ?? -1) ?? null;
                            var buffsubject = await Ioc.model.GetStudentSubject(buffSpecialization.SubjectID ?? -1) ?? null;

                            var buffJornal = await Ioc.model.GetStudentGrade(exam.ID, student.ID);

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
                                subjects.Add(new SubjectViewWrapper(
                                    buffJornal?.ID ?? 0,
                                    exam?.ID ?? 0,
                                    exam?.RatingMethod ?? "",
                                    buffsubject?.Name ?? "",
                                    exam?.Date ?? "",
                                    grade ?? ""
                                    ));
                            }
                        }


                        if (student is not null && subjects is not null)
                        {
                            students.Add(new StudentGradeViewWrapper(
                                student.ID,
                                student.FullName,
                                student.Birth,
                                student.GroupID,
                                subjects
                            ));
                        }
                    }
                }
            }
        }

        #region Команды

        #region Команда показа студентов моей группы
        public ICommand TeacherUpdateGradesCommand { get; }
        private bool CanTeacherUpdateGradesCommandExecute(object parameter) => true;
        private async void OnTeacherUpdateGradesCommandExecute(object parameter)
        {
            if (Ioc.model is null)
            {
                MessageBox.Show("Модель данных недоступна.");
                return;
            }

            // Перебираем всех студентов и их оценки для обновления в базе данных
            foreach (var student in students)
            {
                if (student.Subjects == null || student.Subjects.Count == 0) continue;

                foreach (var subject in student.Subjects)
                {
                    if (subject.JornalId == null)
                    {
                        MessageBox.Show($"Оценка по предмету '{subject.SubjectName}' отсутствует в журнале.");
                        continue;
                    }

                    // Преобразуем строковую оценку в целочисленную
                    int grade;
                    if (subject.RatingMethod == "Экзамен")
                    {
                        if (int.TryParse(subject.SubjectGrade, out grade))
                        {
                            if (grade < 0 || grade > 5)
                            {
                                MessageBox.Show($"Оценка по предмету '{subject.SubjectName}' для студента '{student.FullName}' некорректна: '{subject.SubjectGrade}'");
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (subject.RatingMethod == "Зачёт") 
                    {
                        if (subject.SubjectGrade == "Зачёт")
                        {
                            grade = 1;
                        }
                        else if (subject.SubjectGrade == "Не зачёт")
                        {
                            grade = 0;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Оценка по предмету '{subject.SubjectName}' для студента '{student.FullName}' некорректна: '{subject.SubjectGrade}'");
                        continue;
                    }


                    // Создаём запись для обновления в журнале
                    var updatedJournal = new Journal
                    {
                        ID = subject.JornalId ?? 0, // ID журнала
                        ExamID = subject.ExamId ?? 0, // ID экзамена
                        StudentID = student.Id ?? 0, // ID студента
                        Grade = grade // Преобразованная оценка
                    };

                    try
                    {
                        // Вызываем метод для обновления записи в базе данных
                        await Ioc.model.SetTeacherStudentGrade(updatedJournal);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении оценки по предмету '{subject.SubjectName}' для студента '{student.FullName}': {ex.Message}");
                    }

                }
            }

            MessageBox.Show($"Выставление проведено");

            GetData();
        }
        #endregion Команда показа студентов моей группы

        #endregion Команды

        private string _Title = "Технологический ВУЗ \"Сессия\"";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
    }
}
