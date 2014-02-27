using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MicrowDB.Client
{
    public class InMemoryDocumentStore : DocumentStore
    {
        public InMemoryDocumentStore(ISQLiteConnectionFactory factory) : base(factory)
        {
        }

        public override IDocumentStore Initialize()
        {
            Connection = Factory.CreateInMemory();
            InitializeInternal();

            return this;
        }
    }
}