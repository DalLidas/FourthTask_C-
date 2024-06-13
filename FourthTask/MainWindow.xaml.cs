using System.IO;
using System.Windows;
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

            string dbPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location ?? "") ?? "", "MyData.db");

            _ = Ioc.InitModel(dbPath);
            Ioc.InitPages(_mainFrame);
            Ioc.MainNavigationService?.NavigateToAuthorizationPage();
        }
    }
}