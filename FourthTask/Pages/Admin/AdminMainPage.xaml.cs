using System.Windows.Controls;
using FourthTask.PageNavigation.Ioc;

namespace FourthTask
{
    /// <summary>
    /// Логика взаимодействия для MainAdminPage.xaml
    /// </summary>
    public partial class MainAdminPage : Page
    {
        public MainAdminPage()
        {
            InitializeComponent();

            Ioc.InitAdminPages(_mainStudentPageFrame);
            Ioc.AdminNavigationService?.NavigateToAdminUserPage();
        }
    }
}
