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

#if DEBUG
        const bool DEBUG_MOD = true;
#else
        const bool DEBUG_MOD = false;
#endif

        public string personeTableName { get; }
        public string trainRoatTableName { get; }

        private readonly SQLiteAsyncConnection? db;
        public TableHandleAsync<Person>? person;
        public TableHandleAsync<TrainRoat>? trainRoat;


        public DBConnectorManager(string dbPath)
        {
            personeTableName = "People";
            trainRoatTableName = "TrainRoat";

            db = new SQLiteAsyncConnection(dbPath);

            if (db is not null)
            {
                person = new TableHandleAsync<Person>(db);
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
                await db.CreateTableAsync<Person>();
                await db.CreateTableAsync<TrainRoat>();
            }
        }
    }
}