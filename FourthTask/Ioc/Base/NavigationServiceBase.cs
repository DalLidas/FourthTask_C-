using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FourthTask.PageNavigation.Base
{
    public interface MainNavigationServiceBase
    {
        void NavigateToAuthorizationPage();

        void NavigateToRegistrationPage();

        void NavigateToMainStudentPage();

        void NavigateToMainTeacherPage();

        void NavigateToMainAdminPage();
    }

    public interface StudentNavigationServiceBase
    {
        public void NavigateToStudentGroupmatesPage();

        public void NavigateToStudentTeachersPage();

        public void NavigateToStudentExamsPage();
    }


    public interface TeacherNavigationServiceBase
    {
        public void NavigateToTeacherGroupsPage();
        public void NavigateToTeacherGradesPage();
    }
    public interface AdminNavigationServiceBase;

}
