using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Owin;
using System.IO;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNet.SignalR;
using Ng2Aa_demo.Hubs;
using Autofac;
using System.Reflection;
using Ng2Aa_demo.Domain.Avatar;
using Ng2Aa_demo.Controllers;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace Ng2Aa_demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
           
            

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            builder.Register(c => CreateActiveDirectoryClient()).AsSelf();
            builder.RegisterType<InMemoryAvatarCache>().AsSelf();
            builder.RegisterModule<MediatorModule>();
            //builder.RegisterType<GetAvatarByUserHandler>().AsSelf();
            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        private ActiveDirectoryClient CreateActiveDirectoryClient() {
            var settings = ActiveDirectory.GetProductionSettings();
            var client = new ActiveDirectoryClientProvider(settings).Get();
            return client;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //UseAutofac(app);

            app.UseDeveloperExceptionPage();

            app.UseAppBuilder(appBuilder =>
            {
                appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            });

            //app.UseApplicationInsightsRequestTelemetry();
            //app.UseApplicationInsightsExceptionTelemetry();
                        
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());

            app.UseStaticFiles();

            app.UseAppBuilder(appBuilder =>
            {
                //create SignalR JavaScript Library  and expose it over signalr/hubs url
                var hubConfiguration = new HubConfiguration();
                hubConfiguration.EnableDetailedErrors = true;
                appBuilder.MapSignalR(hubConfiguration);
            });



            // Make sure the Autofac lifetime scope is passed to Web API.
            //app.UseAutofacWebApi(config);
            app.UseMvc();

            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //app.UseAppBuilder(ab => ab.use)
            //builder.RegisterSource(new ContravariantRegistrationSource());
            //builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

            //// Register Web API controller in executing assembly.
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();


            //var lazyContainer = new Lazy<IContainer>(() => builder.Build());
            //var serviceLocatorProvider = new ServiceLocatorProvider(() => new AutofacServiceLocator(lazyContainer.Value));
            //builder.RegisterInstance(serviceLocatorProvider);
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(lazyContainer.Value);


            //// This should be the first middleware added to the IAppBuilder.
            //app.UseAutofacMiddleware(lazyContainer.Value);

            //// Make sure the Autofac lifetime scope is passed to Web API.
            //app.UseAutofacWebApi(config);

            //builder.RegisterType<AutofacServiceLocator>().AsImplementedInterfaces();
            //var container = builder.Build();

            //ServiceLocator.SetLocatorProvider(serviceLocatorProvider);            
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void UseAutofac(IApplicationBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<GetAvatarByUserHandler>().AsSelf();
           // builder.RegisterType<AvatarsController>().AsSelf();
            var container = builder.Build();
            app.UseAppBuilder(ab => ab.UseAutofacMiddleware(container));
        }

    }
}
