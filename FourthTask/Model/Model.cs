using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FourthTask.DataBase.Connector;
using System.Collections.Concurrent;
using System.Security;
using System.Data.Common;
using FourthTask.DataBase;
using Newtonsoft.Json.Linq;
using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels;


namespace FourthTask.Models
{
    public enum Privilages
    {
        None = 0,
        admin = 1,
        deanWorkman = 2,
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

            if (createNewDBFlag is true && DBConnector is not null && DBConnector.person is not null)
            {
                User rootUser = new User()
                {
                    Login = "root",
                    Password = "root",
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
        #endregion Admin

        #region deanWorkman
        #endregion deanWorkman

        #region teacher
        #endregion teacher

        #region students

        public Student? GetCurrentStudent()
        {
            return this.currStudent;
        }
        
           
        public async Task<List<Subject>> GetSubjects()
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


        public async Task<List<Student>> GetStudentsGroupmates()
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


        public async Task<List<Exam>> GetExams()
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
        public async Task<Specialization> GetSpecialization(int specializationId)
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
        public async Task<Group> GetGroup(int groupId)
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
        public Group GetGroupSync(int groupId)
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
        public async Task<Staff> GetTeacher(int teacherId)
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
        public async Task<Subject> GetSubject(int subjectId)
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
        public async Task<Journal> GetGrade(int examtId)
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
                            if (item.ExamID == examtId)
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
                var users = await DBConnector.person.GetItemsAsync().ConfigureAwait(false);
                List<Task> tasks = new List<Task>();

                if (users is not null)
                {
                    Parallel.ForEach(users, (user, state) =>
                    {
                        var task = Task.Run(() =>
                        {
                            if (user.Login == login && user.Password == password)
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
                else if (sessionPrivilages == Privilages.deanWorkman || sessionPrivilages == Privilages.teacher)
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
                        Password = password,
                        Email = email,
                        Privilages = "Student"
                    };

                    await DBConnector.person.SaveItemAsync(user);

                    //if (DEBUG_MOD) MessageBox.Show($"Создался  Login = {login}, Password = {password}");
                    MessageBox.Show($"Создался  Login = {login}, Password = {password}");
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
                case "DeanWorkman":
                    return Privilages.deanWorkman;
                case "Teacher":
                    return Privilages.teacher;
                case "Student":
                    return Privilages.student;
                default:
                    return Privilages.None;
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
