using SQLite;
using System;
using System.Windows;


namespace FourthTask.DataBase.Connector
{
    /// <summary>
    ///  Общий контейнер для взаймодействия с таблицами
    /// </summary>
    public class DBConnectorManager
    {
        public string personeTableName { get; }
        public string trainRoatTableName { get; }

        private readonly SQLiteAsyncConnection? db;
        public TableHandleAsync<User>? person;
        public TableHandleAsync<TrainRoat>? trainRoat;


        public DBConnectorManager(string dbPath)
        {
            personeTableName = "People";
            trainRoatTableName = "TrainRoat";

            db = new SQLiteAsyncConnection(dbPath);

            if (db is not null)
            {
                person = new TableHandleAsync<User>(db);
                trainRoat = new TableHandleAsync<TrainRoat>(db);
            }
        }

        ~DBConnectorManager()
        {
            db?.CloseAsync();
            person = null;
            trainRoat = null;
        }

        public async Task InitAsync()
        {
            if (db is not null)
            {
                await db.CreateTableAsync<User>();
                await db.CreateTableAsync<TrainRoat>();
            }
        }
    }
}