using Autofac;
using Autofac.Builder;
using System;

namespace CommonLibrary
{
    public class InjectionService
    {
        private static Lazy<InjectionService> service = new Lazy<InjectionService>(() => new CommonLibrary.InjectionService());
        public static InjectionService Service { get => service.Value; }
        private static ContainerBuilder Builder { get; set; } = new ContainerBuilder();

        private static IContainer Container { get; set; }

        public void InjectionSingleInstance<TService>(object serverImpl)
        {
            InjectionService.Builder.RegisterInstance(serverImpl).As<TService>().SingleInstance().PropertiesAutowired(PropertyWiringOptions.None);
        }

        public void InjectionSingleInstance<TService>(Func<TService> func) where TService : class
        {
            InjectionService.Builder.RegisterInstance(func()).As<TService>().SingleInstance().PropertiesAutowired(PropertyWiringOptions.None);
        }

        public void InjectionSingleInstance<TService, ServiceImpl>()
        {
            InjectionService.Builder.RegisterType<ServiceImpl>().As<TService>().SingleInstance().PropertiesAutowired(PropertyWiringOptions.None);
        }

        public void InjectionSingleInstance<TService>()
        {
            InjectionService.Builder.RegisterType<TService>().As<TService>().SingleInstance().PropertiesAutowired(PropertyWiringOptions.None);
        }

        public void InjectionType<TService, ServiceImpl>()
        {
            InjectionService.Builder.RegisterType<ServiceImpl>().As<TService>().PropertiesAutowired(PropertyWiringOptions.None);
        }

        public void InjectionType<TService>(Func<TService> func)
        {
            bool flag = func != null;
            if (flag)
            {
                object instance = func();
                InjectionService.Builder.RegisterInstance(instance).As<TService>().PropertiesAutowired(PropertyWiringOptions.None);
            }
        }

        public T Resolve<T>()
        {
            return InjectionService.Container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return InjectionService.Container.Resolve(type);
        }

        public void Start()
        {
            InjectionService.Container = InjectionService.Builder.Build(ContainerBuildOptions.None);
        }
    }
}
