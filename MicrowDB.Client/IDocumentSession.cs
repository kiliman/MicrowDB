using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MicrowDB.Client
{
    public interface IDocumentSession : IDisposable
    {
        T Load<T>(string id) where T : IDocument;
        ITableQuery<T> Query<T>() where T : new();
        void Store(IDocument document);
        void Delete<T>(string id) where T : IDocument;
        void Delete(IDocument document);
        void SaveChanges();
    }
}