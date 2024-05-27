using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthTask.PageNavigation.Base
{
    public interface NavigationServiceBase
    {
        void NavigateToAuthorizationPage();

        void NavigateToRegistrationPage();

        void NavigateToMainPage();
    }
    
}
