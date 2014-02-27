using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MicrowDB.Client
{
    public class DocumentStore : IDocumentStore
    {
        protected readonly ISQLiteConnectionFactory Factory;
        protected readonly IDictionary<string, IIndexMap> IndexMap = new Dictionary<string, IIndexMap>();

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

        public void RegisterIndex<TEntity, TResult>(IndexMap<TEntity, TResult> map)
        {
            IndexMap.Add(typeof(TEntity).FullName, map);
            Connection.CreateTable(typeof (TResult), CreateFlags.AutoIncPK | CreateFlags.AllImplicit);
        }

        public IIndexMap GetIndexMapByEntityType(Type entiyType)
        {
            return IndexMap[entiyType.FullName];
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
