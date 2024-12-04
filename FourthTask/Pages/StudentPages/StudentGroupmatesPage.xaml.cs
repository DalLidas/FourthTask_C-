using FourthTask.DataBase;
using FourthTask.PageNavigation.Ioc;
using System.Windows.Controls;
using System.Collections.ObjectModel;


namespace FourthTask.Pages.StudentPages
{
    /// <summary>
    /// Логика взаимодействия для StudentsGroupmatePage.xaml
    /// </summary>
    public partial class StudentsGroupmatePage : Page
    {
        public ObservableCollection<Student> groupmates { get; set; }

        public StudentsGroupmatePage()
        {
            InitializeComponent();

            groupmates = new ObservableCollection<Student>();
        }

        private async void Grid_Initialized(object sender, EventArgs e)
        {
            if (Ioc.model is not null)
            {
                List<Student> buffStudent = await Ioc.model.GetStudentsGroupmates();
                if (buffStudent is not null)
                {
                    groupmates.Clear();
                    foreach (Student groupmate in buffStudent.OrderBy(x => x.ID))
                    {
                        groupmates.Add(groupmate);
                    }
                }
            }
        }
    }
}
