using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trippin;

namespace AB.OData.App
{
    public class PeopleService : IPeopleService
    {
        private readonly Container _context;

        public PeopleService()
        {
            //_context = new Container(new Uri("https://services.odata.org/TripPinRESTierService/"));
            _context = new Container(new Uri("https://services.odata.org/TripPinRESTierService/(S(wv45u1vyh024l5ffsfvcp3tw))/"));
        }

        public async Task<Person[]> GetAllPeople()
        {
            IEnumerable<Person> people = _context.People.GetAllPages();
            return await Task.FromResult(people.ToArray());
        }

        public async Task<Person[]> GetPeopleByName(string name)
        {
            IEnumerable<Person> people = _context.People.Where(x =>
                x.FirstName.ToLower().Contains(name) ||
                x.LastName.ToLower().Contains(name));

            return await Task.FromResult(people.ToArray());
        }

        public async Task<Person> GetPersonByUsername(string username)
        {
            Person person = _context.People.Where(x =>
                x.UserName.ToLower() == username).FirstOrDefault();

            //Person person = _context.People.ByKey(userName: username).GetValue();

            return await Task.FromResult(person);
        }

        public async Task UpdatePerson(Person person)
        {
            //person.UpdateLastName(person.LastName);
            _context.UpdateObject(person);
            var res = _context.SaveChangesAsync(Microsoft.OData.Client.SaveChangesOptions.ReplaceOnUpdate);
        }
    }
}
