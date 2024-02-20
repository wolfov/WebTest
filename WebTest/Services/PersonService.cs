using Microsoft.EntityFrameworkCore;
using WebTest.Models;
using WebTest.Contexts;

namespace WebTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly AppDbContext _context;
        public PersonService(AppDbContext context)
        {
            _context = context;
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
            await _context.SaveChangesAsync();
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
