using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebTest.Models;

namespace WebTest
{
    public class PersonController : Controller
    {
        private readonly AppContext _context;
        public PersonController(AppContext context)
        {
            _context = context;
        }

        [HttpGet("/")]
        public async Task<IResult> Index()
        {
            return Results.Ok();
        }

        [HttpGet("/api/v1/persons")]
        public async Task<IResult> GetPersons()
        {
            try
            {
                var persons = _context.Persons.AsNoTracking().Include(x => x.Skills).ToArray();
                return Results.Json(persons);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return Results.StatusCode(500);

            }
        }

        [HttpPost("/api/v1/persons")]
        public async Task<IResult> NewPersone([FromBody] string body)
        {
            try
            {
                Person person = JsonSerializer.Deserialize<Person>(body);
                if (person == null)
                    return Results.StatusCode(500);

                await _context.Persons.AddAsync(person);
                await _context.SaveChangesAsync();

                return Results.Json(person);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return Results.StatusCode(500);
            }
        }

        [HttpGet("/api/v1/persons/{id:long}")]
        public async Task<IResult> GetPerson(long id)
        {
            try
            {
                Person person = await _context.Persons.AsNoTracking()
                    .Include(x => x.Skills)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (person == null)
                    return Results.BadRequest(new { message = "Сотрудник не найден" });

                return Results.Json(person);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return Results.StatusCode(500);
            }
        }

        [HttpPut("/api/v1/persons/{id:long}")]
        public async Task<IResult> ChangePerson(long id, [FromBody] string body)
        {
            try
            {
                Person changePerson = JsonSerializer.Deserialize<Person>(body);
                if (changePerson == null)
                    return Results.StatusCode(500);

                Person person = await _context.Persons
                    .Include(x => x.Skills)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (person == null)
                    return Results.BadRequest(new { message = "Сотрудник не найден" });

                person.Skills = changePerson.Skills;
                changePerson.Id = id;
                _context.Persons.Entry(person).CurrentValues.SetValues(changePerson);
                await _context.SaveChangesAsync();

                return Results.Json(changePerson);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return Results.StatusCode(500);
            }
        }

        [HttpDelete("/api/v1/persons/{id:long}")]
        public async Task<IResult> DeletePersone(long id)
        {
            try
            {
                Person person = await _context.Persons
                    .Include(x => x.Skills)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (person == null)
                    return Results.BadRequest(new { message = "Сотрудник не найден" });
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();

                return Results.Ok();
            }
            catch (Exception ex)
            {
                LogException(ex);
                return Results.StatusCode(500);
            }
        }

        void LogException(Exception ex)
        {
            Logger.LogToFile(AppDomain.CurrentDomain.BaseDirectory + "log.txt", ex.Message);
        }
    }
}
