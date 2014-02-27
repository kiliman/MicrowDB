using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using MicrowDB.Client;

namespace MicrowDB.Tests.Models
{
    public class PersonIndex : IndexResult
    {
        [Indexed, Collation("nocase")]
        public string LastName { get; set; }
        
        [Indexed]
        public int Age { get; set; }
        
        [Indexed, Collation("nocase")]
        public string Home_City { get; set; }
    }
}