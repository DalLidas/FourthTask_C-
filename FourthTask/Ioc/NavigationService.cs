using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FourthTask.PageNavigation.Base;
using FourthTask.Pages.Admin;
using FourthTask.Pages.StudentPages;
using FourthTask.Pages.TeacherPages;

namespace FourthTask.PageNavigation
{
    class MainNavigationService : MainNavigationServiceBase
    {
        private readonly Frame _frame;

        public MainNavigationService(Frame frame)
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

        public void NavigateToAdminMainPage()
        {
            _frame.Navigate(new MainAdminPage());
        }

        public void NavigateToTeacherMainPage()
        {
            _frame.Navigate(new TeacherMainPage());
        }

        public void NavigateToStudentMainPage()
        {
            _frame.Navigate(new MainStudentPage());
        }

        public void NavigateToNoneMainPage()
        {
            _frame.Navigate(new NoneMainPage());
        }
    }

    class StudentNavigationService : StudentNavigationServiceBase
    {
        private readonly Frame _frame;

        public StudentNavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToStudentGroupmatesPage()
        {
            _frame.Navigate(new StudentsGroupmatePage());
        }

        public void NavigateToStudentTeachersPage()
        {
            _frame.Navigate(new StudentsTeacherPage());
        }

        public void NavigateToStudentExamsPage()
        {
            _frame.Navigate(new StudentsExamsPage());
        }
    }

    class TeacherNavigationService : TeacherNavigationServiceBase
    {
        private readonly Frame _frame;
        public TeacherNavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToTeacherGroupsPage()
        {
            _frame.Navigate(new TeacherGroupsPage());
        }

        public void NavigateToTeacherGradesPage()
        {
            _frame.Navigate(new TeacherGradesPage());
        }
    }

    class AdminNavigationService : AdminNavigationServiceBase
    {
        private readonly Frame _frame;
        public AdminNavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToAdminUserPage()
        {
            _frame.Navigate(new AdminUserPage());
        }

        public void NavigateToAdminStaffPage()
        {
            _frame.Navigate(new AdminStaffPage());
        }

        public void NavigateToAdminGroupPage()
        {
            _frame.Navigate(new AdminGroupPage());
        }

        public void NavigateToAdminStudentPage()
        {
            _frame.Navigate(new AdminStudentPage());
        }

        public void NavigateToAdminSubjectPage()
        {
            _frame.Navigate(new AdminSubjectPage());
        }

        public void NavigateToAdminSpecializationPage()
        {
            _frame.Navigate(new AdminSpecializationPage());
        }

        public void NavigateToAdminExamPage()
        {
            _frame.Navigate(new AdminExamPage());
        }

        public void NavigateToAdminJournalPage()
        {
            _frame.Navigate(new AdminJournalPage());
        }
    }
}