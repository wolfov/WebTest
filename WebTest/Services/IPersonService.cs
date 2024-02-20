using WebTest.Models;

namespace WebTest.Services
{
    public interface IPersonService
    {
        Task<ICollection<Person>> GetPersons();
        Task<Person> GetPerson(long id);
        Task<Person> NewPerson(Person person);
        Task<Person> ChangePerson(long id, Person person);
        Task<bool> DeletePerson(long id);
    }
}
