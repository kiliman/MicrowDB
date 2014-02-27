using System;

namespace MicrowDB.Client
{
    public interface IDocumentSession : IDisposable
    {
        T Load<T>(string id) where T : IDocument;
        void Store(IDocument document);
        void Delete<T>(string id) where T : IDocument;
        void Delete(IDocument document);
        void SaveChanges();
    }
}