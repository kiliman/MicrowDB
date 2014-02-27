using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MicrowDB.Client
{
    public class DocumentSession : IDocumentSession
    {
        private readonly IDocumentStore _store;
        private readonly IDictionary<string, DocumentCacheEntry> _documentCache = new Dictionary<string, DocumentCacheEntry>();

        public DocumentSession(IDocumentStore store)
        {
            _store = store;
        }

        public T Load<T>(string id) where T : IDocument
        {
            if (id == null)
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            // check document cache
            DocumentCacheEntry entry;
            if (_documentCache.TryGetValue(id, out entry))
            {
                return (T)entry.Entity;
            }
            var document = _store.Connection.Find<Document>(id);
            if (document == null)
            {
                return default(T);
            }

            var entity = JsonConvert.DeserializeObject<T>(document.Json);
            _documentCache.Add(id, new DocumentCacheEntry { Entity = entity, Json = document.Json });
            return entity;
        }

        public ITableQuery<T> Query<T>() where T : new()
        {
            return _store.Connection.Table<T>();
        }

        public void Store(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document", "document cannot be null");
            }

            if (document.Id == null)
            {
                document.Id = Guid.NewGuid().ToString();
            }

            DocumentCacheEntry entry;
            if (_documentCache.TryGetValue(document.Id, out entry))
            {
                if (entry.Entity != document)
                {
                    throw new InvalidOperationException("You cannot store a different document reference for the same id");
                }
                return;
            }
            _documentCache.Add(document.Id, new DocumentCacheEntry { Entity = document });
        }

        public void Delete<T>(string id) where T : IDocument
        {
            if (id == null)
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }
            // check document cache
            DocumentCacheEntry entry;
            if (_documentCache.TryGetValue(id, out entry))
            {
                if (entry.IsDeleted)
                {
                    throw new InvalidOperationException(string.Format("Document {0} of Type {1} is already deleted", id, typeof(T).FullName));
                }
                entry.IsDeleted = true;
                return;
            }
            _documentCache.Add(id, new DocumentCacheEntry { IsDeleted = true, Entity = default(T) });
        }

        public void Delete(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document", "document cannot be null");
            }
            // check document cache
            DocumentCacheEntry entry;
            if (_documentCache.TryGetValue(document.Id, out entry))
            {
                if (entry.IsDeleted)
                {
                    throw new InvalidOperationException(string.Format("Document {0} of Type {1} is already deleted", document.Id, document.GetType().FullName));
                }
                entry.IsDeleted = true;
                return;
            }
            _documentCache.Add(document.Id, new DocumentCacheEntry { IsDeleted = true, Entity = document });
        }

        public void SaveChanges()
        {
            if (_documentCache.Count == 0)
            {
                // no changes, so do nothing
                return;
            }

            try
            {
                _store.Connection.BeginTransaction();

                foreach (var id in _documentCache.Keys)
                {
                    var entry = _documentCache[id];
                    if (entry.IsDeleted)
                    {
                        _store.Connection.Delete<Document>(id);
                        continue;
                    }

                    var json = JsonConvert.SerializeObject(entry.Entity);
                    if (entry.Json != json)
                    {
                        // cache new json for subsequent change tracking
                        entry.Json = json;

                        var document = new Document
                        {
                            Id = entry.Entity.Id,
                            TypeName = entry.Entity.GetType().FullName,
                            Json = json
                        };
                        _store.Connection.InsertOrReplace(document);
                    }
                }

                _store.Connection.Commit();
            }
            catch
            {
                _store.Connection.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
        }
    }
}