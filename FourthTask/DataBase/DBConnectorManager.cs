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
        private readonly SQLiteAsyncConnection? db;

        public TableHandleAsync<User>? person;
        public TableHandleAsync<Staff>? staff;
        public TableHandleAsync<Student>? student;
        public TableHandleAsync<Group>? group;
        public TableHandleAsync<Subject>? subject;
        public TableHandleAsync<Specialization>? specialization;
        public TableHandleAsync<Exam>? exam;
        public TableHandleAsync<Journal>? journal;


        public DBConnectorManager(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);

            if (db is not null)
            {
                person = new TableHandleAsync<User>(db);
                staff = new TableHandleAsync<Staff>(db);
                student = new TableHandleAsync<Student>(db);
                group = new TableHandleAsync<Group>(db);
                subject = new TableHandleAsync<Subject>(db);
                specialization = new TableHandleAsync<Specialization>(db);
                exam = new TableHandleAsync<Exam>(db);
                journal = new TableHandleAsync<Journal>(db);
            }
        }

        ~DBConnectorManager()
        {
            db?.CloseAsync();
            person = null;
            staff = null;
            student = null;
            group = null;
            subject = null;
            specialization = null;
            exam = null;
            journal = null;
        }

        public async Task InitAsync()
        {
            if (db is not null)
            {
                await db.CreateTableAsync<User>();
                await db.CreateTableAsync<Staff>();
                await db.CreateTableAsync<Student>();
                await db.CreateTableAsync<Group>();
                await db.CreateTableAsync<Subject>();
                await db.CreateTableAsync<Specialization>();
                await db.CreateTableAsync<Exam>();
                await db.CreateTableAsync<Journal>();
            }
        }
    }
}