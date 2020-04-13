using Assignment.DataAccess.Abstractions;
using Assignment.DataAccess.Implementations;
using Assignment.DataAccess.Repository;
using Assignment.Model.Abstractions;
using Assignment.Model.Implementations;
using Assignment.Services.Abstractions;
using Assignment.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net;

namespace Assignment.API
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
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IConfigProvider, ConfigProvider>(provider => new ConfigProvider { ConnectionString = Configuration.GetConnectionString("DevConnection") });

            services.AddScoped<IProviderRepository,
                               HealthCareProviderRepository>();

            services.AddScoped<IProviderService,
                               ProviderService>();
            services.AddCors();
            services.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if(error != null)
                        {
                            await context.Response.WriteAsync(error.Error?.Message);
                        }
                    });
                });
            }

            app.UseRouting();

            //app.UseAuthorization();

            app.UseCors(p => p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
