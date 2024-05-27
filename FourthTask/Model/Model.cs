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


namespace FourthTask.Models.Model
{
    public enum Privilages
    {
        None = 0,
        admin = 1,
        user = 2,
    }

    public class Model
    {
        #if DEBUG 
        const bool DEBUG_MOD = true;
        #else
        const bool DEBUG_MOD = false;
        #endif

        public DBConnectorManager? DBConnector;
        private User? sessionUser;

        private bool sessionStartedFlag;
        private Privilages sessionPrivilages;

        public async Task<bool> InitModel(string dbPath)
        {            
            DBConnector = new DBConnectorManager(dbPath);
            if (DBConnector is null) return false;
            await DBConnector.InitAsync();

            sessionUser = null;
            sessionStartedFlag = false;
            sessionPrivilages = Privilages.None;

            if (DEBUG_MOD) MessageBox.Show($"Создался контейнер для базы данных");
            return DBConnector is not null;
        }

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

            if (DEBUG_MOD) MessageBox.Show($"Конец инициализации базы данных");
            return sessionStartedFlag;
        }

        public async Task<bool> RegistrateUser(string login, string password, string email)
        {
            if (DBConnector is not null && DBConnector.person is not null)
            {
                bool LoginAlreadyTaken = false;
                var users = await DBConnector.person.GetItemsAsync().ConfigureAwait(false);
                List<Task> tasks = new List<Task>();

                if (users is not null)
                {
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
                        Privilages = "User"
                    };

                    await DBConnector.person.SaveItemAsync(user);

                    if (DEBUG_MOD) MessageBox.Show($"Создался  Login = {login}, Password = {password}");
                    return true;
                }
            }

            return false;
        }

        private Privilages SetPrivilagesLevel(string privilages)
        {
            switch(privilages)
            {
                case "Admin":
                    return Privilages.admin;
                case "User":
                    return Privilages.user;
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
