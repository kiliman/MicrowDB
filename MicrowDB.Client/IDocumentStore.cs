using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MicrowDB.Client
{
    public interface IDocumentStore : IDisposable
    {
        ISQLiteConnection Connection { get; set; }
        IDocumentStore Initialize();
        IDocumentSession OpenSession();
        void RegisterIndex<TEntity, TResult>(IndexMap<TEntity, TResult> map);
        IIndexMap GetIndexMapByEntityType(Type entiyType);
    }
}