using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebTest.Models;
using WebTest.Services;

namespace WebTest.Controllers
{
    [Route("/api/v1/")]
    public class PersonController : Controller
    {
        private readonly IPersonService _service;
        public PersonController(IPersonService service)
        {
            _service = service;
        }

        [HttpGet("persons")]
        public async Task<IResult> GetPersons()
        {
            return Results.Json(await _service.GetPersons());
        }

        [HttpPost("persons")]
        public async Task<IResult> NewPersone([FromBody] Person person)
        {
            if (person == null || !ModelState.IsValid)
                return Results.StatusCode(500);
            Person newPerson = await _service.NewPerson(person);
            if (newPerson == null)
                return Results.StatusCode(500);

            return Results.Json(newPerson);
        }

        [HttpGet("persons/{id:long}")]
        public async Task<IResult> GetPerson(long id)
        {
            Person person = await _service.GetPerson(id);
            if (person == null)
                return Results.BadRequest(new { message = "Сотрудник не найден" });
            return Results.Json(person);
        }

        [HttpPut("persons/{id:long}")]
        public async Task<IResult> ChangePerson(long id, [FromBody] Person changePerson)
        {
            if (changePerson == null || !ModelState.IsValid)
                return Results.StatusCode(500);
            changePerson = await _service.ChangePerson(id, changePerson);
            if (changePerson == null)
                return Results.BadRequest(new { message = "Сотрудник не найден" });
            return Results.Json(changePerson);
        }

        [HttpDelete("persons/{id:long}")]
        public async Task<IResult> DeletePersone(long id)
        {
            if (!await _service.DeletePerson(id))
                return Results.BadRequest(new { message = "Сотрудник не найден" });
            return Results.Ok();
        }
    }
}
