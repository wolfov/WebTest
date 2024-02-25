using Microsoft.EntityFrameworkCore;
using WebTest.Models;
using WebTest.Contexts;
using Microsoft.Extensions.Logging;

namespace WebTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _loggeer;
        public PersonService(AppDbContext context, ILogger loggeer)
        {
            _context = context;
            _loggeer = loggeer;
        }
        public async Task<ICollection<Person>> GetPersons()
        {
            return await _context.Persons.Include(x => x.Skills).ToListAsync();
        }
        public async Task<Person> GetPerson(long id)
        {
            return await _context.Persons.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Person> NewPerson(Person person)
        {
            await _context.Persons.AddAsync(person);
            if (person != null)
                await _context.SaveChangesAsync();
            else
                _loggeer?.Log(LogLevel.Trace, 0, this, new NullReferenceException(), (Cl, Ex)=> $"{Cl.GetType().FullName}.{nameof(NewPerson)}: {Ex.Message}");
            return person;
        }
        public async Task<Person> ChangePerson(long id, Person changePerson)
        {
            if (!_context.Persons.Any(x => x.Id == id))
                return null;
            changePerson.Id = id;
            _context.Persons.Update(changePerson);
            await _context.SaveChangesAsync();
            return changePerson;
        }
        public async Task<bool> DeletePerson(long id)
        {
            Person person = await GetPerson(id);
            if (person == null)
                return false;
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
