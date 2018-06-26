using Autofac;
using Autofac.Integration.Mvc;
using EF.DI.Autofac;
using System.Web.Mvc;

namespace EF.DI.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // 在控制器中注册依赖
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // 在过滤中注册依赖
            builder.RegisterFilterProvider();

            // 在自定义视图中注册依赖
            builder.RegisterSource(new ViewRegistrationSource());

            // 注册我们自定义依赖
            builder.RegisterModule(new AutofacModule());

            var container = builder.Build();

            // 设置Autofac容器到MVC依赖关系中
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}