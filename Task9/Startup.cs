﻿using System.IO;
using System.Net.Mime;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;


namespace Task9 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {


            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();

            services.AddDbContext<Task9Context>(
                options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString("Task9Context"),
                     x => x.MigrationsAssembly("Data")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            using (var scope = app.ApplicationServices.CreateScope()) {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<Task9Context>();
                context.Database.Migrate();
            }

            env.EnvironmentName = "Production";

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Courses/Error");
                app.UseHsts();
            }

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Courses}/{action=Index}/{id?}");
            });
        }
    }
}
