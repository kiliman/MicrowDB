using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MicrowDB.Client
{
    public class DocumentStore : IDocumentStore
    {
        protected readonly ISQLiteConnectionFactory Factory;
        public ISQLiteConnection Connection { get; set; }
        public DocumentStore(ISQLiteConnectionFactory factory)
        {
            Factory = factory;
        }

        public virtual IDocumentStore Initialize()
        {
            Connection = Factory.Create(FileName);
            InitializeInternal();

            return this;
        }

        public string FileName { get; set; }

        protected void InitializeInternal()
        {
            Connection.CreateTable<Document>(CreateFlags.ImplicitPK);
        }

        public IDocumentSession OpenSession()
        {
            return new DocumentSession(this);
        }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
        }
    }
}
