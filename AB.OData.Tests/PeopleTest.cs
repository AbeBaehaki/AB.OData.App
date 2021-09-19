using AB.OData.App;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AB.OData.Tests
{
    /// <summary>
    /// Integration tests for TripPin People service
    /// </summary>
    public class PeopleTest
    {
        [Fact]
        public async Task People_ListAllPeople()
        {
            IPeopleService peopleService = new PeopleService();
            var people = await peopleService.GetAllPeople();

            Assert.NotNull(people);
            Assert.True(people.Length > 0);
        }

        [Theory]
        [InlineData("ar")]
        [InlineData("Joni")]
        [InlineData("ruSSel")]
        public async Task People_GetByName(string query)
        {
            IPeopleService peopleService = new PeopleService();
            var people = await peopleService.GetPeopleByName(query);

            Assert.True(people.All(x =>
                x.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                x.LastName.Contains(query, StringComparison.OrdinalIgnoreCase)));
        }

        [Theory]
        [InlineData("jonirosales")]
        public async Task People_UpdatePerson(string username)
        {
            IPeopleService peopleService = new PeopleService();
            var person = await peopleService.GetPersonByUsername(username);
            person.FirstName = "Test";

            await peopleService.UpdatePerson(person);

            var updatedPerson = await peopleService.GetPersonByUsername(username);

            Assert.Equal("Test", updatedPerson.FirstName);
        }
    }
}
