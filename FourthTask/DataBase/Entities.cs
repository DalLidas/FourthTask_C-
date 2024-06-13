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
    /// Таблица с работниками вуза. Деканат + преподаватели
    /// </summary>
    [Table("Staff")]
    public class Staff : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_Staff")]
        public override int ID { get; set; }

        [NotNull] 
        public string? FullName { get; set; }

        [NotNull]
        public string? Birth { get; set; }

        [NotNull]
        public string? Job { get; set; }

        [NotNull]
        public string? Merit { get; set; }

        [NotNull]
        public int? Internship { get; set; }
    }

    /// <summary>
    /// Таблица со студентами
    /// </summary>
    [Table("Student")]
    public class Student : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_Student")]
        public override int ID { get; set; }

        [NotNull]
        public string? FullName { get; set; }

        [NotNull]
        public string? Birth { get; set; }

        [NotNull]
        public int? GroupID { get; set; }

        [NotNull]
        public string? AdmissionDate { get; set; }

        [NotNull]
        public string? Data { get; set; }
    }

    /// <summary>
    /// Таблица с группами
    /// </summary>
    [Table("Group")]
    public class Group : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_Group")]
        public override int ID { get; set; }

        [NotNull]
        public string? Faculty { get; set; }

        [NotNull]
        public string? Name { get; set; }
    }

    /// <summary>
    /// Таблица с предметом 
    /// </summary>
    [Table("Subject")]
    public class Subject : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_Subject")]
        public override int ID { get; set; }

        [NotNull]
        public string? Name { get; set; }
    }

    /// <summary>
    /// Таблица со специализацией
    /// </summary>
    [Table("Specialization")]
    public class Specialization : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_Specialization")]
        public override int ID { get; set; }

        [NotNull]
        public int? TeacherID { get; set; }

        [NotNull]
        public int? SubjectID { get; set; }
    }

    /// <summary>
    /// Таблица с экзаменами
    /// </summary>
    [Table("Exam")]
    public class Exam : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_Exam")]
        public override int ID { get; set; }

        [NotNull]
        public string? Date { get; set; }

        [NotNull]
        public string? RatingMethod { get; set; }

        [NotNull]
        public int? GroupID { get; set; }

        [NotNull]
        public int? SpecializationID { get; set; }
    }

    /// <summary>
    /// Таблица с экзаменами
    /// </summary>
    [Table("Journal")]
    public class Journal : TableBase
    {
        [PrimaryKey, AutoIncrement, Column("ID_Journal")]
        public override int ID { get; set; }

        [NotNull]
        public int? ExamID { get; set; }

        [NotNull]
        public int? StudentID { get; set; }

        [NotNull]
        public int? Grade { get; set; }
    }
}
