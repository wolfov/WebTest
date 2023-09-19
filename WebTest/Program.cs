using WebTest;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebTest.AppContext>();
builder.Services.AddCors();
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "Person", pattern: "{controller=Person}");

await app.RunAsync();
await Task.Delay(-1);