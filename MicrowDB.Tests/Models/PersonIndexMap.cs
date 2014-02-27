using System.Linq;
using MicrowDB.Client;

namespace MicrowDB.Tests.Models
{
    public class PersonIndexMap : IndexMap<Person, PersonIndex>
    {
        public PersonIndexMap()
        {
            Map = person => new PersonIndex
                {
                    LastName = person.LastName,
                    Age = person.Age,
                    Home_City = person.Home.City
                };
        }
    }
}