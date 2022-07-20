using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Common.Architecture.Core.Utilities.Interceptors;
using Common.Architecture.Core.Utilities.Security.JWT;
using Common.Architecture.Infrastructure.Abstract;
using Common.Architecture.Infrastructure.Concrete;
using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Persistance.Concrete.EntityFramework;

namespace Common.Architecture.Infrastructure.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
