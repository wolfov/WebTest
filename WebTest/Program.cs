using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using WebTest.Contexts;
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

            builder.Services.AddCors();
            builder.Services.AddControllers();
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(name: "Person", pattern: "{controller=Person}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/internal/swagger.json", "v1"));
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/internal/swagger.json", async context =>
                    {
                        await context.Response.WriteAsync(await File.ReadAllTextAsync("swagger.json"));
                    });
                });
            }

            await app.RunAsync();
            await Task.Delay(-1);
        }
    }
}
