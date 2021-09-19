using System.Threading.Tasks;
using Trippin;

namespace AB.OData.App
{
    public interface IPeopleService
    {
        Task<Person[]> GetAllPeople();
        Task<Person[]> GetPeopleByName(string name);
        Task<Person> GetPersonByUsername(string username);
        Task UpdatePerson(Person person);
    }
}