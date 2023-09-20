using WebTest;
using WebTest.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebTest.AppContext>();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddTransient<IPersonService, PersonService>();

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "Person", pattern: "{controller=Person}");

await app.RunAsync();
await Task.Delay(-1);