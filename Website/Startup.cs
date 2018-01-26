using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Website.Infrastructure.Data.Entities;
using Website.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Website {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services
            services.AddDbContext<DomainContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Website"), opts => opts.EnableRetryOnFailure());
            });

            // Add framework services.
            services.AddMvc();

            // Dependency injection.
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IDomainContext, DomainContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //if (env.IsDevelopment()) {
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();
            //}
            //else {
            //    app.UseExceptionHandler("/Home/Error");
            //}

            // TODO: Put this back
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();

            app.UseStaticFiles();

            //app.UseIdentity();
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = "/admin/account/login"
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "imagesRoute",
                    template: "images/{id:int?}",
                    defaults: new { controller = "Images", action = "Index" });

                routes.MapAreaRoute(
                    name: "adminRoute",
                    areaName: "admin",
                    template: "{area:exists}/{controller=Clocks}/{action=Index}/{id?}");
            });
        }
    }
}
