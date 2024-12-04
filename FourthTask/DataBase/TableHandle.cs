using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FourthTask.DataBase;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FourthTask.DataBase
{
    /// <summary>
    /// Класс реализующий доступ к таблицам базы данных 
    /// </summary>
    /// <typeparam name="T">Класс преставляющий таблицу унаследованный от TableBase</typeparam>
    public class TableHandleAsync<T> where T : TableBase, new()
    {
        /// <summary>
        /// Соединение с базой данных
        /// </summary>
        private readonly SQLiteAsyncConnection database;

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
            try
            {
                if (tableName == null || database == null)
                {
                    return false;
                }

                var query = await database.QueryScalarsAsync<int>($"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{tableName}';");
                return query.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Создание таблицы асинхронно
        /// </summary>
        public async Task CreateTableAsync()
        {
            try
            {
                await database.CreateTableAsync<T>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Получение всех элементов таблицы асинхронно
        /// </summary>
        public async Task<List<T>> GetItemsAsync()
        {
            try
            {
                return await database.Table<T>().ToListAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new List<T>();
            }
        }

        /// <summary>
        /// Получение элемента таблицы синхронно
        /// </summary>
        public List<T> GetItemsSync()
        {
            try
            {
                return database.GetConnection().Table<T>().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new List<T>();
            }

        }

        /// <summary>
        /// Получение элемента таблицы асинхронно
        /// </summary>
        public async Task<T> GetItemAsync(int id)
        {
            try 
            {
                return await database.GetAsync<T>(id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new T();
            }
        }



        /// <summary>
        /// Получение элемента таблицы синхронно
        /// </summary>
        public T GetItemSync(int id)
        {
            try
            {
                return database.GetConnection().Get<T>(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new T();
            }

        }

        /// <summary>
        /// Удаление элемента таблицы асинхронно
        /// </summary>
        public async Task<int> DeleteItemAsync(T item)
        {
            try 
            {
                return await database.DeleteAsync(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
            
        }

        /// <summary>
        /// Добавление нового элемента (если ID == 0) или изменение старого элемента асинхронно
        /// </summary>
        public async Task<int> SaveItemAsync(T item)
        {
            try
            {
                if (item.ID != 0)
                {
                    await database.UpdateAsync(item);
                    return item.ID;
                }
                else
                {
                    return await database.InsertAsync(item);
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
            
        }
    }
}
