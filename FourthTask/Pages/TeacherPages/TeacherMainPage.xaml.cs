using FourthTask.PageNavigation.Ioc;
using System.Windows.Controls;


namespace FourthTask
{
    /// <summary>
    /// Логика взаимодействия для MainTeacherPage.xaml
    /// </summary>
    public partial class TeacherMainPage : Page
    {
        public TeacherMainPage()
        {
            InitializeComponent();

            Ioc.InitTeacherPages(_mainStudentPageFrame);
            Ioc.TeacherNavigationService?.NavigateToTeacherGroupsPage();
        }
    }
}
