﻿using System;
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

        void NavigateToMainDeanWorkmanPage();

        void NavigateToMainAdminPage();
    }

    public interface StudentNavigationServiceBase
    {
        public void NavigateToStudentsGroupmatesPage();

        public void NavigateToStudentsSubjectsPage();

        public void NavigateToStudentsExamsPage();

        public void NavigateToDeanWorkmanPage();
    }


    public interface TeacherNavigationServiceBase;
    public interface DeanWorkmanNavigationServiceBase;
    public interface AdminNavigationServiceBase;

}
