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
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.DataProtection;

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

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAppBuilder(appBuilder =>
            {
                appBuilder.SetDataProtectionProvider(new MachineKeyProtectionProvider());

                appBuilder.Map("/signalr", map =>
                {
                    // Setup the CORS middleware to run before SignalR.
                    // By default this will allow all origins. You can 
                    // configure the set of origins and/or http verbs by
                    // providing a cors options with a different policy.
                    
                    //http://ng2a-hneu-web-ui.azurewebsites.net
                    var corsOptions = new Microsoft.Owin.Cors.CorsOptions
                    {
                        PolicyProvider = new CorsPolicyProvider
                        {
                            PolicyResolver = context => ResolvePolicy()
                        }
                    };
                    map.UseCors(corsOptions);
                    //map.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
                    var hubConfiguration = new HubConfiguration
                    {
                        // You can enable JSONP by uncommenting line below.
                        // JSONP requests are insecure but some older browsers (and some
                        // versions of IE) require JSONP to work cross domain
                        // EnableJSONP = true
                        EnableDetailedErrors = true
                    };
                    // Run the SignalR pipeline. We're not using MapSignalR
                    // since this branch already runs under the "/signalr"
                    // path.
                    map.RunSignalR(hubConfiguration);
                });
                //var options = new CorsOptions();
                //options.AddPolicy("", new )
                //appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            });

            
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();


            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());

            app.UseStaticFiles();
        }

        private Task<System.Web.Cors.CorsPolicy> ResolvePolicy() {

            var corsPolicy = new System.Web.Cors.CorsPolicy()
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                SupportsCredentials = true
            };
            
            corsPolicy.Origins.Add("http://ng2a-hneu-web-ui.azurewebsites.net");
            corsPolicy.Origins.Add("http://localhost:3000");

            return Task.FromResult(corsPolicy);
        }

        internal class MachineKeyProtectionProvider : IDataProtectionProvider
        {
            public IDataProtector Create(params string[] purposes)
            {
                return new MachineKeyDataProtector(purposes);
            }
        }

        internal class MachineKeyDataProtector : IDataProtector
        {
            private readonly string[] _purposes;

            public MachineKeyDataProtector(string[] purposes)
            {
                _purposes = purposes;
            }

            public byte[] Protect(byte[] userData)
            {
                //return MachineKey.Protect(userData, _purposes);
                return userData;
            }

            public byte[] Unprotect(byte[] protectedData)
            {
                //return System.Web.Security.MachineKey.Unprotect(protectedData, _purposes);
                return protectedData;
            }
        }




    }
}
