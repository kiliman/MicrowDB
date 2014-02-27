using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MicrowDB.Client
{
    public class IndexResult
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string DocumentId { get; set; }
    }
}