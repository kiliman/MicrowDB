using MicrowDB.Client;
using SyncTest.Console;

namespace MicrowDB.Tests.Models
{
    public class Person : IDocument
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Address Home { get; set; }
    }
}
