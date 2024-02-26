using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.WebSockets;
using WebTest.Contexts;
using WebTest.Logger;
using WebTest.Services;

namespace WebTest
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectString = builder.Configuration.GetConnectionString("DefaultConnection").Replace("[appPath]", AppDomain.CurrentDomain.BaseDirectory);
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connectString);
            });


            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddFile("log.txt");
            var logger = loggerFactory.CreateLogger("FileLogger");
            builder.Services.AddSingleton(logger);
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddLogging();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.MapControllerRoute(name: "Person", pattern: "{controller=Person}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/internal/swagger.json", "v1"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/internal/swagger.json", async context =>
                {
                    await context.Response.WriteAsync(await File.ReadAllTextAsync("swagger.json"));
                });
            });

            await app.RunAsync();
            await Task.Delay(-1);
        }
    }
}
