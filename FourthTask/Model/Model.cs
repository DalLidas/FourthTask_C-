using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FourthTask.DataBase.Connector;
using System.Security;
using System.Data.Common;
using FourthTask.DataBase;
using Newtonsoft.Json.Linq;
using FourthTask.PageNavigation.Ioc;


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

            if (DEBUG_MOD) MessageBox.Show($"Создался контейнер для базы данных");
            return DBConnector is not null;
        }


        #region Admin
        #endregion Admin

        #region deanWorkman
        #endregion deanWorkman

        #region teacher
        #endregion teacher

        #region student
        #endregion student


        /// <summary>
        /// Запуск сессии для работы с моделью
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public async Task<bool> StartSession(string login, string password)
        {
            if (DEBUG_MOD) MessageBox.Show($"Текущий login = {login},  Текущий password = {password}");
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
                                    MessageBox.Show($"Добро пожаловать id:{sessionUser?.ID}, Login:{sessionUser?.Login}, Privilages:{sessionUser?.Privilages}");

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

            
            if (DEBUG_MOD) MessageBox.Show($"Конец инициализации базы данных");
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
                        var task = Task.Run(() => subjects.Add(item));

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
                                groupmates.Add(item);
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


        private Privilages SetPrivilagesLevel(string privilages)
        {
            switch(privilages)
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


        public bool GetSessionStatus(){
            return sessionStartedFlag;
        }


        public Privilages GetSessionPrivilages()
        {
            return sessionPrivilages;
        }
    }
}
