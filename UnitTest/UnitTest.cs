using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using WebTest.Contexts;
using WebTest.Models;
using WebTest.Services;
using Xunit;

namespace UnitTest
{
    public class PersonControllerTest : IDisposable
    {
        private readonly IPersonService _personService;
        private AppDbContext _context;

        public PersonControllerTest()
        {
            GetAppDbContext().Database.EnsureCreated();
        }

        public void Dispose()
        {
            GetAppDbContext().Database.EnsureDeleted();
        }
        private AppDbContext GetAppDbContext() => new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("TestDb").Options);
        private IPersonService GetPersonService() => new PersonService(GetAppDbContext());

        [Fact]
        public async void NewPerson()
        {
            Person addedPerson = TestPersons().First();

            Person result = await GetPersonService().NewPerson(addedPerson);
            Person check = await GetPersonService().GetPerson(addedPerson.Id);

            Assert.True(JsonConvert.SerializeObject(result) == JsonConvert.SerializeObject(addedPerson));
            Assert.True(JsonConvert.SerializeObject(check) == JsonConvert.SerializeObject(addedPerson));
        }
        [Fact]
        public async void GetPersons()
        {
            List<Person> addedPersons = new List<Person>();
            foreach (var person in TestPersons())
                addedPersons.Add(await GetPersonService().NewPerson(person));

            ICollection<Person> result = await GetPersonService().GetPersons();

            foreach (var person in result)
                Assert.True(addedPersons.Any(x=>x.Id == person.Id));
        }

        [Fact]
        public async void GetPerson()
        {
            Person addedPerson = await GetPersonService().NewPerson(TestPersons().First());

            Person result = await GetPersonService().GetPerson(addedPerson.Id);

            Assert.True(JsonConvert.SerializeObject(result) == JsonConvert.SerializeObject(addedPerson));
        }
        [Fact]
        public async void DeletePerson()
        {
            Person addedPerson = await GetPersonService().NewPerson(TestPersons().First());

            bool result = await GetPersonService().DeletePerson(addedPerson.Id);
            Person check = await GetPersonService().GetPerson(addedPerson.Id);

            Assert.True(result);
            Assert.Null(check);
        }
        [Fact]
        public async void ChangePerson()
        {
            Person addedPerson = await GetPersonService().NewPerson(TestPersons().First());
            addedPerson.Name = "Changed";
            addedPerson.Skills.Add(new Skill() { Name = "Dance", Level = 3 });

            Person result = await GetPersonService().ChangePerson(addedPerson.Id, addedPerson);
            Person check = await GetPersonService().GetPerson(addedPerson.Id);

            Assert.True(JsonConvert.SerializeObject(result) == JsonConvert.SerializeObject(addedPerson));
            Assert.True(JsonConvert.SerializeObject(check) == JsonConvert.SerializeObject(addedPerson));
        }
        private IEnumerable<Person> TestPersons()
        {
            return new[]
            {
                new Person()
                {
                    Name = "TST1",
                    DisplayName = "tst1",
                    Skills = { new Skill() { Name = "draw", Level = 1 } }
                },
                new Person
                {
                    Name = "TST2",
                    DisplayName = "tst2"
                }
            };
        }
    }
}
