using Microsoft.EntityFrameworkCore;
using WebTest;
using Db = WebTest.AppContext;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/api/v1/persons", GetPersons);
app.MapPost("/api/v1/persons", NewPersone);
app.MapGet("/api/v1/persons/{id:long}", GetPerson);
app.MapPut("/api/v1/persons/{id:long}", ChangePerson);
app.MapDelete("/api/v1/persons/{id:long}", DeletePersone);

app.MapGet("/", () => "Hello World!");

await app.RunAsync();
await Task.Delay(-1);

async Task NewPersone(HttpContext context)
{
    try
    {
        Person newPerson = await context.Request.ReadFromJsonAsync<Person>();
        if (newPerson == null)
        {
            context.Response.StatusCode = 500;
            return;
        }
        using (Db db = new Db())
        {
            db.Persons.Add(newPerson);
            db.SaveChanges();
        }
        await context.Response.WriteAsJsonAsync(newPerson);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        LogException(ex);
    }
}
async Task GetPersons(HttpContext context)
{
    try
    {
        using (Db db = new Db())
        {
            var pers = db.Persons.AsNoTracking().Include(x => x.skills).ToArray();
            await context.Response.WriteAsJsonAsync(pers);
        }
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        LogException(ex);
    }
}
async Task GetPerson(long id, HttpContext context)
{
    try
    {
        Person person;
        using (Db db = new Db())
        {
            person = await db.Persons.FindAsync(id);
            if (person == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { message = "Сотрудник не найден" });
                return;
            }
            db.Persons.Entry(person).Collection(x => x.skills).Load();
        }
        await context.Response.WriteAsJsonAsync(person);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        LogException(ex);
    }
}
async Task ChangePerson(long id, HttpContext context)
{
    try
    {
        Person changePerson = await context.Request.ReadFromJsonAsync<Person>();
        if (changePerson == null)
        {
            context.Response.StatusCode = 500;
            return;
        }
        Person person;
        using (Db db = new Db())
        {
            person = await db.Persons.FindAsync(id);
            if (person == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { message = "Сотрудник не найден" });
                return;
            }
            db.Persons.Entry(person).Collection(x => x.skills).Load();
            person.skills = changePerson.skills;
            changePerson.id = id;
            db.Persons.Entry(person).CurrentValues.SetValues(changePerson);
            db.SaveChanges();
        }
        await context.Response.WriteAsJsonAsync(changePerson);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        LogException(ex);
    }
}
async Task DeletePersone(long id, HttpContext context)
{
    try
    {
        Person person;
        using (Db db = new Db())
        {
            person = await db.Persons.FindAsync(id);
            if (person == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { message = "Сотрудник не найден" });
                return;
            }
            db.Persons.Remove(person);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        LogException(ex);
    }
}

void LogException(Exception ex)
{
    Logger.LogToFile(AppDomain.CurrentDomain.BaseDirectory + "log.txt", ex.Message);
}
