using System.Windows.Controls;
using Autofac;
using FourthTask.PageNavigation.Base;
using FourthTask.PageNavigation;
using FourthTask.Models.Model;

namespace FourthTask.PageNavigation.Ioc
{
    public static class Ioc
    {
        private static IContainer? _container;

        public static Model? model;

        public static NavigationServiceBase? NavigationService
        {
            get { return _container?.Resolve<NavigationServiceBase>(); }
        }

        public static void InitPages(Frame frame)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<NavigationService>()
                .As<NavigationServiceBase>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Frame), frame));


            _container = builder.Build();
        }

        public static async Task InitModel(string dbPath, string login, string password)
        {
            model = new Model();
            await model.InitModel(dbPath, login, password);
        }
    }
}
