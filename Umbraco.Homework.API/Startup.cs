using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Microsoft.EntityFrameworkCore;
using Umbraco.Homework.API.Data;
using Umbraco.Homework.API.Services;

namespace Umbraco.Homework.API
{
    public class Startup
    {
        private const String API_NAME = "Acme Corporation Prize Draw API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = API_NAME });
            });

            services.AddDbContext<PrizeDrawDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddScoped<IPrizeDrawService, PrizeDrawService>();
            services.AddScoped<ISerialNumberService, SerialNumberService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
                .WithOrigins(this.Configuration.GetValue<String>("AllowedCorsDomains"))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetPreflightMaxAge(new TimeSpan(0, 10, 0))
            ); ;

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", API_NAME);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Generate the DB on Startup (if not there already)
            // Wouldn't use this for production, but it's nice for prototypes and demos
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PrizeDrawDbContext>();

                context.Database.EnsureCreated();
            }
        }
    }
}
