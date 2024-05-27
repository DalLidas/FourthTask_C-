using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthTask.DataBase
{
    /// <summary>
    /// Общий шаблон таблицы
    /// </summary>
    [Table("___")]
    public class TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public virtual int ID { get; set; }
    }

    /// <summary>
    /// Таблица с пользователями
    /// </summary>
    [Table("User")]
    public class User : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_User")]
        public override int ID { get; set; }

        [NotNull]
        public string? Login { get; set; }

        [NotNull]
        public string? Password { get; set; }

        [NotNull]
        public string? Email { get; set; }

        [NotNull]
        public string? Privilages {  get; set; }
    }

    /// <summary>
    /// Таблица с Путями поездов
    /// </summary>
    [Table("TrainRoat")]
    public class TrainRoat : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_TrainRoat")]
        public override int ID { get; set; }

        [NotNull]
        public string? Name { get; set; }
        [NotNull]
        public string? Date { get; set ; }
        [NotNull]
        public string? Places { get; set; }
    }
}
