using Microsoft.EntityFrameworkCore;
using System;
using WebTest.Models;

namespace WebTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly AppContext _context;
        public PersonService(AppContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Person>> GetPersons()
        {
            return await _context.Persons.Include(x=>x.Skills).ToListAsync();
        }
        public async Task<Person> GetPerson(long id)
        {
            return await _context.Persons.Include(x => x.Skills).FirstOrDefaultAsync(x=>x.Id==id);
        }
        public async Task<Person> NewPersone(Person person)
        {
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }
        public async Task<Person> ChangePerson(long id, Person changePerson)
        {
            Person currentPerson = await GetPerson(id);
            if (currentPerson == null)
                return null;
            currentPerson.Skills = changePerson.Skills;
            changePerson.Id = id;
            _context.Persons.Entry(currentPerson).CurrentValues.SetValues(changePerson); //без этого сущность не обновляется
            await _context.SaveChangesAsync();
            return changePerson;
        }
        public async Task<bool> DeletePersone(long id)
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
