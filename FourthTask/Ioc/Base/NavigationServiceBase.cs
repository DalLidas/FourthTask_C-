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

        void NavigateToAdminMainPage();

        void NavigateToTeacherMainPage();

        void NavigateToStudentMainPage();

        void NavigateToNoneMainPage();

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

    public interface AdminNavigationServiceBase
    {
        public void NavigateToAdminUserPage();
        public void NavigateToAdminStaffPage();
        public void NavigateToAdminGroupPage();
        public void NavigateToAdminStudentPage();
        public void NavigateToAdminSubjectPage();
        public void NavigateToAdminSpecializationPage();
        public void NavigateToAdminExamPage();
        public void NavigateToAdminJournalPage();
    }

}
