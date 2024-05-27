using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FourthTask.Models.Model;
using FourthTask.PageNavigation.Ioc;

namespace FourthTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Ioc.InitPages(_mainFrame);
            Ioc.NavigationService.NavigateToAuthorizationPage();
        }
    }
}