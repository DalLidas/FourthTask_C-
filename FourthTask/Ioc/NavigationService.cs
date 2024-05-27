using FourthTask.PageNavigation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FourthTask.PageNavigation
{
    class NavigationService : NavigationServiceBase
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToAuthorizationPage()
        {
            _frame.Navigate(new AuthorizationPage());
        }

        public void NavigateToRegistrationPage()
        {
            _frame.Navigate(new RegistrationPage());
        }

        public void NavigateToMainPage()
        {
            _frame.Navigate(new MainPage());
        }
    }
}
