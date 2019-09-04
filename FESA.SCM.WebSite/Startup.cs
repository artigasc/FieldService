using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FESA.SCM.WebSite.Services.Implementation;
using FESA.SCM.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Solution.Ferreyros
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddCors();
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession(opts =>
                {
                    opts.Cookie.Name = ".Ferreyros.Session";
                    opts.IdleTimeout = TimeSpan.FromMinutes(10);
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });


        }
    }
}
