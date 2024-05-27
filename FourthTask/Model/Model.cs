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
        private Person? sessionUser;

        private bool sessionStartedFlag;
        private Privilages sessionPrivilages;

        public async Task<bool> InitModel(string dbPath, string login, string password)
        {
            //const string DATABASE_NAME = "DB.db";
            if (DEBUG_MOD) MessageBox.Show($"dbPath = {dbPath}, login = {login},  password = {password}");
            
            DBConnector = new DBConnectorManager(dbPath);
            if (DBConnector is null) return false;
            await DBConnector.InitAsync();

            if (DEBUG_MOD) MessageBox.Show($"Создался контейнер для базы данных");


            //var person1 = new Person
            //{
            //    Login = login,
            //    Password = password,
            //    Email = "Mail@mail.ru",
            //    Privilages = "Admin"
            //};

            //if (DBConnector is not null && DBConnector.person is not null)
            //{
            //    await DBConnector.person.SaveItemAsync(person1);
            //}

            //if (DEBUG_MOD) MessageBox.Show($"Создался  Login = {login}, Password = {password}");

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
                            }

                            state.Break();
                        });
                        tasks.Add(task);
                    });

                    // Выполнение всех тасков
                    Task.WaitAll(tasks.ToArray());

                    if (DEBUG_MOD)
                        MessageBox.Show($"Добро пожаловать id:{sessionUser?.ID}, Login:{sessionUser?.Login}, Privilages:{sessionUser?.Privilages}");
                }
            }
            else
            {
                sessionStartedFlag = false;
                sessionPrivilages = SetPrivilagesLevel(sessionUser?.Privilages ?? "");
            }


            if (sessionUser is not null)
            {
                sessionStartedFlag = true;
                sessionPrivilages = SetPrivilagesLevel(sessionUser?.Privilages ?? "");
            }
            else
            {
                sessionStartedFlag = false;
                sessionPrivilages = SetPrivilagesLevel(sessionUser?.Privilages ?? "");
            }

            if (DEBUG_MOD) MessageBox.Show($"Конец инициализации базы данных");
            return sessionStartedFlag;
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
