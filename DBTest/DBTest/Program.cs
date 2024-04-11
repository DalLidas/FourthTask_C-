using Autofac;
using Autofac.Core;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Diagnostics;
using static DBTest.Program;


namespace DBTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string DATABASE_NAME = "DB.db";

            SQLiteAsyncConnection database = new SQLiteAsyncConnection(DATABASE_NAME);

            TableHandleAsync<Person> persons_table = new TableHandleAsync<Person>(database);
            TableHandleAsync<TrainRoute> trainRoutes_table = new TableHandleAsync<TrainRoute>(database);
            TableHandleAsync<Ticket> tickets_table = new TableHandleAsync<Ticket>(database);
            TableHandleAsync<Price> prices_table = new TableHandleAsync<Price>(database);

            await persons_table.CreateTableAsync();
            await trainRoutes_table.CreateTableAsync();
            await tickets_table.CreateTableAsync();
            await prices_table.CreateTableAsync();

            //Person person = new Person();
            //person.id = 0;
            //person.name = "Danil";

            //TrainRoute trainRoute = new TrainRoute();
            //trainRoute.id = 0;
            //trainRoute.name = "Fast way";

            //Ticket ticket = new Ticket();
            //ticket.id = 0;
            //ticket.person_id = 1000;
            //ticket.trainRoute_id = 1000;
            //ticket.price_id = 1000;

            //Price price = new Price();
            //price.id = 0;
            //price.name = "Elite";

            //await persons_table.SaveItemAsync(person);
            //await trainRoutes_table.SaveItemAsync(trainRoute);
            //await tickets_table.SaveItemAsync(ticket);
            //await prices_table.SaveItemAsync(price);

            var p = persons_table.GetItemsAsync().Result;

            for (int i = 0; i < p.Count; ++i)
            {
                Console.WriteLine(p[i].id + " " + p[i].name + " " + p[i].password + " " + p[i].privileges);
                Console.WriteLine(p[i].tickets);
            }

        }

        /// <summary>
        /// Общий шаблон таблицы
        /// </summary>
        [Table("___")]
        public class TableBase
        {
            [PrimaryKey, AutoIncrement, Column("_id_person")]
            public virtual int id { get; set; }
        }

        /// <summary>
        /// Пользователи
        /// </summary>
        [Table("Persons")]
        public class Person: TableBase
        {
            [PrimaryKey, AutoIncrement, Column("_id_person")]
            public override int id { get; set; }
            public string? name { get; set; }
            public string? password { get; set; }
            public string? privileges { get; set; }

            [OneToMany(CascadeOperations = CascadeOperation.All)]
            public List<Ticket>? tickets { get; set; }
        }

        /// <summary>
        ///  Маршруты поездов
        /// </summary>
        [Table("TrainRoutes")]
        public class TrainRoute: TableBase
        {
            [PrimaryKey, AutoIncrement, Column("_id_trainRoutes")]
            public override int id { get; set; }
            public string? name { get; set; }
            public string? startPoint { get; set; }
            public string? endPoint { get; set; }
            public string? stopPoint { get; set; }
            public int? startTime { get; set; }
            public int? endTime { get; set; }
            public bool? isValid { get; set; }

            [OneToMany(CascadeOperations = CascadeOperation.All)]
            public List<Ticket>? tickets { get; set; }
        }

        /// <summary>
        /// Билеты
        /// </summary>
        [Table("Tickets")]
        public class Ticket: TableBase
        {
            [PrimaryKey, AutoIncrement, Column("_id_ticket")]
            public override int id { get; set; }

            [ForeignKey(typeof(Person))]
            public int? person_id { get; set; }

            [ForeignKey(typeof(TrainRoute))]
            public int? trainRoute_id { get; set; }

            [ForeignKey(typeof(Price))]
            public int? price_id { get; set; }
            public int? orderTime { get; set; }
        }

        /// <summary>
        /// Цена билетов
        /// </summary>
        [Table("Prices")]
        public class Price: TableBase
        {
            [PrimaryKey, AutoIncrement, Column("_id_price")]
            public override int id { get; set; }
            public string? name { get; set; }
            public int? price { get; set; }

            [OneToMany(CascadeOperations = CascadeOperation.All)]
            public List<Ticket>? tickets { get; set; }
        }


        public class DBConnectionControler
        {
            TableHandleAsync<Person>? persons_table = null;
            TableHandleAsync<TrainRoute>? trainRoutes_table = null;
            TableHandleAsync<Ticket>? tickets_table = null;
            TableHandleAsync<Price>? prices_table = null;

            public DBConnectionControler(TableHandleAsync<Person> _persons_table, TableHandleAsync<TrainRoute> _trainRoutes_table, TableHandleAsync<Ticket> _tickets_table, TableHandleAsync<Price> _prices_table)
            {
                InitializeDB(_persons_table, _trainRoutes_table, _tickets_table, _prices_table);
            }

            public void InitializeDB(TableHandleAsync<Person> _persons_table, TableHandleAsync<TrainRoute> _trainRoutes_table, TableHandleAsync<Ticket> _tickets_table, TableHandleAsync<Price> _prices_table)
            {
                this.persons_table = _persons_table;
                this.trainRoutes_table = _trainRoutes_table;
                this.tickets_table = _tickets_table;
                this.prices_table = _prices_table;
            }




        }


        /// <summary>
        /// Класс реализующий доступ к таблицам базы данных 
        /// </summary>
        /// <typeparam name="T">Класс преставляющий таблицу унаследованный от TableBase</typeparam>
        public class TableHandleAsync<T> where T : TableBase, new()
        {
            /// <summary>
            /// Соединение с базой данных
            /// </summary>
            SQLiteAsyncConnection database;

            /// <summary>
            /// Конструктор для задания соединения с базой данных
            /// </summary>
            public TableHandleAsync(SQLiteAsyncConnection db)
            {
                database = db;
            }

            /// <summary>
            /// Проверка на существрования таблицы в базе данных
            /// </summary>
            public async Task<bool> CheckTableExists(string tableName)
            {
                var query = await database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM sqlite_master WHERE type=\'" + tableName + "\' AND name=?");
                return query > 0;
            }

            /// <summary>
            /// Создание таблицы асинхронно
            /// </summary>
            public async Task CreateTableAsync()
            {
                await database.CreateTableAsync<T>();
            }

            /// <summary>
            /// Получение всех элементов таблицы асинхронно
            /// </summary>
            public async Task<List<T>> GetItemsAsync()
            {
                return await database.Table<T>().ToListAsync();
            }

            /// <summary>
            /// Получение элемента таблицы асинхронно
            /// </summary>
            public async Task<T> GetItemAsync(int id)
            {
                return await database.GetAsync<T>(id);
            }

            /// <summary>
            /// Удаление элемента таблицы асинхронно
            /// </summary>
            public async Task<int> DeleteItemAsync(T item)
            {
                return await database.DeleteAsync(item);
            }

            /// <summary>
            /// Добавление нового элемента (если id == 0) или изменение старого элемента асинхронно
            /// </summary>
            public async Task<int> SaveItemAsync(T item)
            {
                if (item.id != 0)
                {
                    await database.UpdateAsync(item);
                    return item.id;
                }
                else
                {
                    return await database.InsertAsync(item);
                }
            }
        }
    }

    //Main
    #region AutoFac
    //var builder = BuildContainer();

    //using (var scope = builder.BeginLifetimeScope())
    //{
    //    MyClass printer = scope.Resolve<MyClass>();
    //    MyClass printer1 = scope.Resolve<MyClass>();

    //    printer.Show("Ты");
    //            printer1.Show("Ты не");
    //}
    //
    //private static IContainer BuildContainer()
    //{
    //    var builder = new ContainerBuilder();
    //    builder.RegisterType<MyClass>().SingleInstance();
    //    builder.RegisterType<Printer1>().SingleInstance();
    //    builder.RegisterType<Printer2>().SingleInstance();
    //    builder.RegisterType<Printer3>().SingleInstance();

    //    return builder.Build();
    //}
    #endregion AutoFac

    //Classes
    #region AutoFac
    //public class MyClass
    //{
    //    Printer1 printer1;
    //    Printer2 printer2;
    //    Printer3 printer3;


    //    public MyClass(Printer1 printer1, Printer2 printer2, Printer3 printer3)
    //    {
    //        this.printer1 = printer1;
    //        this.printer2 = printer2;
    //        this.printer3 = printer3;
    //    }

    //    public void Show(string msg)
    //    {
    //        printer1.print(msg);
    //        printer2.print(msg);
    //        printer3.print(msg);
    //    }
    //}

    //public class Printer1
    //{
    //    public void print(string msg)
    //    {
    //        Console.WriteLine("Printer1: " + msg);
    //    }
    //}

    //public class Printer2
    //{
    //    public void print(string msg)
    //    {
    //        Console.WriteLine("Printer2: " + msg);
    //    }
    //}

    //public class Printer3
    //{
    //    public void print(string msg)
    //    {
    //        Console.WriteLine("Printere: " + msg);
    //    }
    //}

    #endregion AutoFac
}
