using System.Windows.Controls;
using Autofac;
using FourthTask.PageNavigation.Base;
using FourthTask.PageNavigation;
using FourthTask.Models;

namespace FourthTask.PageNavigation.Ioc
{
    public static class Ioc
    {
        private static IContainer? _container;
        private static IContainer? _studentContainer;
        private static IContainer? _teacherContainer;
        private static IContainer? _adminContainer;

        public static Model? model;

        public static MainNavigationServiceBase? MainNavigationService
        {
            get { return _container?.Resolve<MainNavigationServiceBase>(); }
        }


        public static StudentNavigationServiceBase? StudentNavigationService
        {
            get { return _studentContainer?.Resolve<StudentNavigationServiceBase>(); }
        }


        public static TeacherNavigationServiceBase? TeacherNavigationService
        {
            get { return _teacherContainer?.Resolve<TeacherNavigationServiceBase>(); }
        }


        public static void InitPages(Frame frame)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainNavigationService>()
                .As<MainNavigationServiceBase>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Frame), frame));


            _container = builder.Build();
        }


        public static void InitStudentPages(Frame frame)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<StudentNavigationService>()
                .As<StudentNavigationServiceBase>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Frame), frame));


            _studentContainer = builder.Build();
        }


        public static void InitTeacherPages(Frame frame)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TeacherNavigationService>()
                .As<TeacherNavigationServiceBase>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Frame), frame));


            _teacherContainer = builder.Build();
        }


        public static void InitAdminPages(Frame frame)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainNavigationService>()
                .As<AdminNavigationServiceBase>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Frame), frame));


            _adminContainer = builder.Build();
        }


        public static async Task InitModel(string dbPath)
        {
            model = new Model();
            await model.InitModel(dbPath);
        }
    }
}
