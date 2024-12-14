using System.IO;
using System.Windows;
using FourthTask.DataBase.Connector;
using FourthTask.DataBase;
using FourthTask.Utilities;


namespace FourthTask.Models
{
    public enum Privilages
    {
        Error = 0,
        None = 1,
        admin = 2,
        teacher = 3,
        student = 4,
    }

    public class Model
    {

#if RELISE // DEBUG // RELISE
        const bool DEBUG_MOD = true;
#else
        const bool DEBUG_MOD = false;
#endif

        public DBConnectorManager? DBConnector;
        private User? sessionUser;
        private Student? currStudent;
        private Staff? currStaff;

        private int? teacherSelectedGroup;

        private bool sessionStartedFlag;
        private Privilages sessionPrivilages;

        /// <summary>
        /// Инициализация базы данных
        /// </summary>
        /// <param name="dbPath">Путь к базе данных</param>
        /// <returns></returns>
        public async Task<bool> InitModel(string dbPath)
        {
            bool createNewDBFlag = false;
            if (!File.Exists(dbPath)) createNewDBFlag = true;

            DBConnector = new DBConnectorManager(dbPath);
            if (DBConnector is null) return false;
            await DBConnector.InitAsync();

            if (createNewDBFlag && DBConnector is not null && DBConnector.person is not null)
            {
                User rootUser = new User()
                {
                    Login = "root",
                    Password = EncryptionUtility.HashPassword("root"),
                    Email = "danil.mukhametov@mail.ru",
                    Privilages = "Admin"
                };

                await DBConnector.person.SaveItemAsync(rootUser);
            }

            sessionUser = null;
            sessionStartedFlag = false;
            sessionPrivilages = Privilages.None;

#pragma warning disable CS0162 // Обнаружен недостижимый код
            if (DEBUG_MOD) MessageBox.Show($"Создался контейнер для базы данных");
#pragma warning restore CS0162 // Обнаружен недостижимый код
            return DBConnector is not null;
        }


        #region Admin

        public User? GetCurrentSession()
        {
            return this.sessionUser;
        }


        // Получение таблицы пользователей
        public async Task<List<User>> GetAdminUsers()
        {
            var users = new List<User>();

            if (DBConnector is not null && DBConnector.person is not null && sessionUser is not null)
            {
                var requst = await DBConnector.person.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (users)
                            {
                                users.Add(item);
                            }

                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return users;
        }
        // Получение таблицы преподавателей
        public async Task<List<Staff>> GetAdminStaff()
        {
            var staff = new List<Staff>();

            if (DBConnector is not null && DBConnector.staff is not null && sessionUser is not null)
            {
                var requst = await DBConnector.staff.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (staff)
                            {
                                staff.Add(item);
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return staff;
        }
        // Получение таблицы групп
        public async Task<List<Group>> GetAdminGroup()
        {
            var groups = new List<Group>();

            if (DBConnector is not null && DBConnector.group is not null && sessionUser is not null)
            {
                var requst = await DBConnector.group.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (groups)
                            {
                                groups.Add(item);
                            }

                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return groups;
        }
        // Получение таблицы студентов
        public async Task<List<Student>> GetAdminStudent()
        {
            var students = new List<Student>();

            if (DBConnector is not null && DBConnector.student is not null && sessionUser is not null)
            {
                var requst = await DBConnector.student.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (students)
                            {
                                students.Add(item);
                            }

                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return students;
        }
        // Получение таблицы предметов
        public async Task<List<Subject>> GetAdminSubject()
        {
            var subjects = new List<Subject>();

            if (DBConnector is not null && DBConnector.subject is not null && sessionUser is not null)
            {
                var requst = await DBConnector.subject.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (subjects)
                            {
                                subjects.Add(item);
                            }

                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return subjects;
        }
        // Получение таблицы специализаций
        public async Task<List<Specialization>> GetAdminSpecialization()
        {
            var specializations = new List<Specialization>();

            if (DBConnector is not null && DBConnector.specialization is not null && sessionUser is not null)
            {
                var requst = await DBConnector.specialization.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (specializations)
                            {
                                specializations.Add(item);
                            }

                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return specializations;
        }
        // Получение таблицы экзаменов
        public async Task<List<Exam>> GetAdminExam()
        {
            var exams = new List<Exam>();

            if (DBConnector is not null && DBConnector.exam is not null && sessionUser is not null)
            {
                var requst = await DBConnector.exam.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (exams)
                            {
                                exams.Add(item);
                            }
                            
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return exams;
        }
        // Получение таблицы журнала
        public async Task<List<Journal>> GetAdminJournal()
        {
            var journal = new List<Journal>();

            if (DBConnector is not null && DBConnector.journal is not null && sessionUser is not null)
            {
                var requst = await DBConnector.journal.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (journal)
                            {
                                journal.Add(item);
                            }

                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return journal;
        }


        public async Task<int> SetAdminUser(User user)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.person is not null && user is not null)
                {
                    return await DBConnector.person.SaveItemAsync(user);
                }
            }

            return 0;
        }
        public async Task<int> SetAdminStaff(Staff staff)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.staff is not null && staff is not null)
                {
                    return await DBConnector.staff.SaveItemAsync(staff);
                }
            }

            return 0;
        }
        public async Task<int> SetAdminGroup(Group group)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.group is not null && group is not null)
                {
                    return await DBConnector.group.SaveItemAsync(group);
                }
            }

            return 0;
        }
        public async Task<int> SetAdminStudent(Student student)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.student is not null && student is not null)
                {
                    return await DBConnector.student.SaveItemAsync(student);
                }
            }

            return 0;
        }
        public async Task<int> SetAdminSubject(Subject subject)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.subject is not null && subject is not null)
                {
                    return await DBConnector.subject.SaveItemAsync(subject);
                }
            }

            return 0;
        }
        public async Task<int> SetAdminSpecialization(Specialization specialization)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.specialization is not null && specialization is not null)
                {
                    return await DBConnector.specialization.SaveItemAsync(specialization);
                }
            }

            return 0;
        }
        public async Task<int> SetAdminExam(Exam exam)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.exam is not null && exam is not null)
                {
                    return await DBConnector.exam.SaveItemAsync(exam);
                }
            }

            return 0;
        }
        public async Task<int> SetAdminJournal(Journal journal)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.journal is not null && journal is not null)
                {
                    return await DBConnector.journal.SaveItemAsync(journal);
                }
            }

            return 0;
        }


        public async Task<int> DeleteAdminUser(User user)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.person is not null && user is not null)
                {
                    return await DBConnector.person.DeleteItemAsync(user);
                }
            }

            return 0;
        }
        public async Task<int> DeleteAdminStaff(Staff staff)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.staff is not null && staff is not null)
                {
                    return await DBConnector.staff.DeleteItemAsync(staff);
                }
            }

            return 0;
        }
        public async Task<int> DeleteAdminGroup(Group group)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.group is not null && group is not null)
                {
                    return await DBConnector.group.DeleteItemAsync(group);
                }
            }

            return 0;
        }
        public async Task<int> DeleteAdminStudent(Student student)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.student is not null && student is not null)
                {
                    return await DBConnector.student.DeleteItemAsync(student);
                }
            }

            return 0;
        }
        public async Task<int> DeleteAdminSubject(Subject subject)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.subject is not null && subject is not null)
                {
                    return await DBConnector.subject.DeleteItemAsync(subject);
                }
            }

            return 0;
        }
        public async Task<int> DeleteAdminSpecialization(Specialization specialization)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.specialization is not null && specialization is not null)
                {
                    return await DBConnector.specialization.DeleteItemAsync(specialization);
                }
            }

            return 0;
        }
        public async Task<int> DeleteAdminExam(Exam exam)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.exam is not null && exam is not null)
                {
                    return await DBConnector.exam.DeleteItemAsync(exam);
                }
            }

            return 0;
        }
        public async Task<int> DeleteAdminJournal(Journal journal)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.journal is not null && journal is not null)
                {
                    return await DBConnector.journal.DeleteItemAsync(journal);
                }
            }

            return 0;
        }


        #endregion Admin

        #region teacher
        public Staff? GetCurrentStaff()
        {
            return this.currStaff;
        }

        public void setTeacherSelectedGroup(int? groupId)
        {
            this.teacherSelectedGroup = groupId;
        }
        public int? getTeacherSelectedGroup()
        {
            return this.teacherSelectedGroup;
        }

        // Получение группы преподавателя
        public async Task<List<Group>> GetTeacherGroups()
        {
            var groups = new List<Group>();


            if (DBConnector is not null && DBConnector.exam is not null)
            {
                var requst = await DBConnector.exam.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(async () => {
                            if (DBConnector is not null && DBConnector.specialization is not null)
                            {
                                var requst = await DBConnector.specialization.GetItemAsync(item.SpecializationID ?? -1);

                                if (requst is not null && requst.TeacherID == currStaff?.ID) 
                                {
                                    if (DBConnector is not null && DBConnector.group is not null)
                                    {
                                        var groupRequst = await DBConnector.group.GetItemAsync(item.GroupID ?? -1);

                                        lock (groups)
                                        {
                                            groups.Add(groupRequst);
                                        }
                                    }
                                }
                            }
                        }
                        );

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return groups;
        }



        // Получение группы преподавателя
        public async Task<List<Student>> GetTeacherStudents()
        {

            var students = new List<Student>();

            if (DBConnector is not null && DBConnector.student is not null && sessionUser is not null && teacherSelectedGroup is not null)
            {
                var requst = await DBConnector.student.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            if (item.GroupID == teacherSelectedGroup)
                            {
                                lock (students)
                                {
                                    students.Add(item);
                                }
                            }
                        });

                    tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return students;
        }


        public async Task<List<Exam>> GetTeacherStudentSubjects()
        {
            var studentExams = new List<Exam>();
            var teacherSpecialization = new List<int?>();

            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.specialization is not null && currStaff is not null)
                {
                    var teacherSpecializationRequst = await DBConnector.specialization.GetItemsAsync();

                    if (teacherSpecializationRequst is not null)
                    {
                        List<Task> tasks = new List<Task>();
                        Parallel.ForEach(teacherSpecializationRequst, item =>
                        {
                            var task = Task.Run(() => {
                                if (item.TeacherID == currStaff.ID)
                                {
                                    lock (teacherSpecialization)
                                    {
                                        teacherSpecialization.Add(item.ID);
                                    }
                                }
                            });

                            tasks.Add(task);
                        });

                        // Выполнение всех тасков
                        Task.WaitAll(tasks.ToArray());
                    }
                }

                //Запрос на экзамены сдаваемые данному преподавателю
                if (DBConnector.exam is not null && teacherSelectedGroup is not null)
                {
                    var requst = await DBConnector.exam.GetItemsAsync();

                    if (requst is not null)
                    {
                        List<Task> tasks = new List<Task>();
                        Parallel.ForEach(requst, item =>
                        {
                            var task = Task.Run(() => {
                                if (item.GroupID == teacherSelectedGroup && teacherSpecialization.Contains(item.SpecializationID))
                                {
                                    lock (studentExams)
                                    {
                                        studentExams.Add(item);
                                    }
                                }
                            });

                            tasks.Add(task);
                        });

                        // Выполнение всех тасков
                        Task.WaitAll(tasks.ToArray());
                    }
                }
            }

            return studentExams;
        }


        public async Task<int> SetTeacherStudentGrade(Journal journal)
        {
            if (DBConnector is not null && sessionUser is not null)
            {
                //Запрос на специализацию преподавателя
                if (DBConnector.journal is not null && currStaff is not null && journal is not null)
                {
                    return await DBConnector.journal.SaveItemAsync(journal);
                }
            }

            return 0;
        }

        #endregion teacher

        #region students

        public Student? GetCurrentStudent()
        {
            return this.currStudent;
        }
        
           
        public async Task<List<Subject>> GetStudentSubjects()
        {
            var subjects = new List<Subject>();


            if (DBConnector is not null && DBConnector.subject is not null)
            {
                var requst = await DBConnector.subject.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            lock (subjects)
                            {
                                subjects.Add(item);
                            }
                        }
                        );

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return subjects;
        }


        public async Task<List<Student>> GetStudentStudentsGroupmates()
        {
            var groupmates = new List<Student>();

            if (DBConnector is not null && DBConnector.student is not null && sessionUser is not null && currStudent is not null)
            {
                var requst = await DBConnector.student.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            if (item.GroupID == currStudent?.GroupID)
                            {
                                lock (groupmates)
                                {
                                    groupmates.Add(item);
                                }
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return groupmates;
        }


        public async Task<List<Exam>> GetStudentExams()
        {
            var exams = new List<Exam>();

            if (DBConnector is not null && DBConnector.exam is not null && sessionUser is not null)
            {
                var requst = await DBConnector.exam.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, item =>
                    {
                        var task = Task.Run(() => {
                            if (item.GroupID == currStudent?.GroupID)
                            {
                                lock (exams)
                                {
                                    exams.Add(item);
                                }
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return exams;
        }


        // Получение специализации по Id
        public async Task<Specialization> GetStudentSpecialization(int specializationId)
        {
            var specialization = new Specialization();

            if (DBConnector is not null && DBConnector.specialization is not null && sessionUser is not null)
            {
                var requst = await DBConnector.specialization.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, (item, state) =>
                    {
                        var task = Task.Run(() => {
                            if (item.ID == specializationId)
                            {
                                lock (specialization)
                                {
#pragma warning disable CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                    specialization = item;
#pragma warning restore CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                }

                                state.Break();
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return specialization;
        }


        // Получение группы по Id
        public async Task<Group> GetStudentGroup(int groupId)
        {
            var group = new Group();

            if (DBConnector is not null && DBConnector.group is not null && sessionUser is not null)
            {
                var requst = await DBConnector.group.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, (item, state) =>
                    {
                        var task = Task.Run(() => {
                            if (item.ID == groupId)
                            {
                                lock (group)
                                {
#pragma warning disable CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                    group = item;
#pragma warning restore CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                }

                                state.Break();
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return group;
        }

        // Получение группы по Id
        public Group GetStudentGroupSync(int groupId)
        {
            var group = new Group();

            if (DBConnector is not null && DBConnector.group is not null && sessionUser is not null)
            {
                var requst = DBConnector.group.GetItemsSync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(requst, (item, state) =>
                    {
                        var task = Task.Run(() => {
                            if (item.ID == groupId)
                            {
                                lock (group)
                                {
#pragma warning disable CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                    group = item;
#pragma warning restore CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                }

                                state.Break();
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return group;
        }


        //Получение преподавателя по Id
        public async Task<Staff> GetStudentTeacher(int teacherId)
        {
            var teacher = new Staff();

            if (DBConnector is not null && DBConnector.staff is not null && sessionUser is not null)
            {
                var requst = await DBConnector.staff.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();

                    Parallel.ForEach(requst, (item, state) =>
                    {
                        var task = Task.Run(() => {
                            if (item.ID == teacherId)
                            {
                                lock (teacher)
                                {
#pragma warning disable CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                    teacher = item;
#pragma warning restore CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                }

                                state.Break();
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return teacher;
        }


        //Получение предмета по Id
        public async Task<Subject> GetStudentSubject(int subjectId)
        {
            var subject = new Subject();

            if (DBConnector is not null && DBConnector.subject is not null && sessionUser is not null)
            {
                var requst = await DBConnector.subject.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();

                    Parallel.ForEach(requst, (item, state) =>
                    {
                        var task = Task.Run(() => {
                            if (item.ID == subjectId)
                            {
                                lock (subject)
                                {
#pragma warning disable CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                    subject = item;
#pragma warning restore CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                }

                                state.Break();
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return subject;
        }


        //Получение предмета по Id
        public async Task<Journal> GetStudentGrade(int examtId, int currStudentID)
        {
            var journal = new Journal();

            if (DBConnector is not null && DBConnector.journal is not null && sessionUser is not null)
            {
                var requst = await DBConnector.journal.GetItemsAsync();

                if (requst is not null)
                {
                    List<Task> tasks = new List<Task>();

                    Parallel.ForEach(requst, (item, state) =>
                    {
                        var task = Task.Run(() => {
                            if (item.ExamID == examtId && item.StudentID == currStudentID)
                            {
                                lock (journal)
                                {
#pragma warning disable CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                    journal = item;
#pragma warning restore CS0728 // Возможно, используется недопустимое назначение для локального параметра, который является аргументом оператора using или lock
                                }

                                state.Break();
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }

            return journal;
        }

        #endregion student


        /// <summary>
        /// Запуск сессии для работы с моделью
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public async Task<bool> StartSession(string login, string password)
        {
#pragma warning disable CS0162 // Обнаружен недостижимый код
            if (DEBUG_MOD) MessageBox.Show($"Текущий login = {login},  Текущий password = {password}");
#pragma warning restore CS0162 // Обнаружен недостижимый код
            if (DBConnector is not null && DBConnector.person is not null)
            {
                var users = await DBConnector.person.GetItemsAsync();
                List<Task> tasks = new List<Task>();

                if (users is not null)
                {
                    Parallel.ForEach(users, (user, state) =>
                    {
                        var task = Task.Run(() =>
                        {
                            if (user.Login == login && EncryptionUtility.VerifyPassword(password, user.Password ?? ""))
                            {
                                sessionUser = user;

                                sessionStartedFlag = true;
                                sessionPrivilages = SetPrivilagesLevel(sessionUser?.Privilages ?? "");

                                if (DEBUG_MOD)
#pragma warning disable CS0162 // Обнаружен недостижимый код
                                    MessageBox.Show($"Добро пожаловать id:{sessionUser?.ID}, Login:{sessionUser?.Login}, Privilages:{sessionUser?.Privilages}");
#pragma warning restore CS0162 // Обнаружен недостижимый код

                                state.Break();
                            }
                        });

                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }
            }
            else
            {
                sessionStartedFlag = false;
                sessionPrivilages = SetPrivilagesLevel(sessionUser?.Privilages ?? "");
            }


            if (sessionUser is null)
            {
                sessionStartedFlag = false;
                sessionPrivilages = SetPrivilagesLevel(sessionUser?.Privilages ?? "");
            }
            else if (DBConnector is not null && DBConnector.student is not null && DBConnector.staff is not null && sessionUser is not null)
            {
                if (sessionPrivilages == Privilages.student)
                {
                    currStudent = await DBConnector.student.GetItemAsync(sessionUser.ID);
                }
                else if (sessionPrivilages == Privilages.teacher)
                {
                    currStaff = await DBConnector.staff.GetItemAsync(sessionUser.ID);
                }
            }


#pragma warning disable CS0162 // Обнаружен недостижимый код
            if (DEBUG_MOD) MessageBox.Show($"Конец инициализации базы данных");
#pragma warning restore CS0162 // Обнаружен недостижимый код
            return sessionStartedFlag;
        }


        /// <summary>
        /// Закрывает сессию
        /// </summary>
        public void CloseSession()
        {
            sessionUser = null;
            sessionStartedFlag = false;
            sessionPrivilages = Privilages.None;
        }


        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="email">мейл</param>
        /// <returns></returns>
        public async Task<bool> RegistrateUser(string login, string password, string email)
        {
            if (DBConnector is not null && DBConnector.person is not null)
            {
                bool LoginAlreadyTaken = false;
                var users = await DBConnector.person.GetItemsAsync().ConfigureAwait(false);

                if (users is not null)
                {
                    List<Task> tasks = new List<Task>();
                    Parallel.ForEach(users, (user, state) =>
                    {
                        var task = Task.Run(() =>
                        {
                            if (user.Login == login)
                            {
                                sessionUser = user;
                                LoginAlreadyTaken = true;

                                state.Break();
                            }
                        });
                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());
                }

                if (LoginAlreadyTaken is false)
                {
                    var user = new User
                    {
                        Login = login,
                        Password = EncryptionUtility.HashPassword(password),
                        Email = email,
                        Privilages = "None"
                    };

                    await DBConnector.person.SaveItemAsync(user);

                    //if (DEBUG_MOD) MessageBox.Show($"Создался  Login = {login}, Password = {password}");
                    MessageBox.Show($"Создался Login = {login}, Password = {password}");
                    return true;
                }
            }

            return false;
        }


        private Privilages SetPrivilagesLevel(string privilages)
        {
            switch (privilages)
            {
                case "Admin":
                    return Privilages.admin;
                case "Teacher":
                    return Privilages.teacher;
                case "Student":
                    return Privilages.student;
                case "None":
                    return Privilages.None;
                default:
                    return Privilages.Error;
            }
        }


        public bool GetSessionStatus()
        {
            return sessionStartedFlag;
        }


        public Privilages GetSessionPrivilages()
        {
            return sessionPrivilages;
        }
    }
}
