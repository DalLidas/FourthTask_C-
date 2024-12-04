using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FourthTask.PageNavigation.Base;
using FourthTask.Pages.StudentPages;

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

        public void NavigateToMainStudentPage()
        {
            _frame.Navigate(new MainStudentPage());
        }

        public void NavigateToMainTeacherPage()
        {
            _frame.Navigate(new MainTeacherPage());
        }

        public void NavigateToMainDeanWorkmanPage()
        {
            _frame.Navigate(new MainDeanWorkmanPage());
        }

        public void NavigateToMainAdminPage()
        {
            _frame.Navigate(new MainAdminPage());
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
}
