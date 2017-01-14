using Autofac;
using Autofac.Features.Variance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ng2Aa_demo
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterSource(new ContravariantRegistrationSource());

            //builder.RegisterType<Mediator>()
            //    .AsImplementedInterfaces()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsSelf().AsImplementedInterfaces();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            //register all pre handlers
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //    .As(type => type.GetInterfaces()
            //        .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof(IAsyncPreRequestHandler<>))))
            //    .InstancePerLifetimeScope();

            //register all post handlers
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //    .As(type => type.GetInterfaces()
            //        .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof(IAsyncPostRequestHandler<,>))))
            //    .InstancePerLifetimeScope();

            //register all asynchandlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .As(type => type.GetInterfaces()
                   .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>))))
               .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces();

            //.Select(interfaceType => new KeyedService("asyncRequestHandler", interfaceType)))


            //register pipeline decorators
            //builder.RegisterGenericDecorator(typeof(AsyncMediatorPipeline<,>), typeof(IAsyncRequestHandler<,>), "asyncRequestHandler")
            //    .InstancePerLifetimeScope();
        }
    }

}
