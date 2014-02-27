using System;
using System.Collections.Generic;

namespace MicrowDB.Client
{
    public class IndexMap<TEntity, TResult> : IIndexMap
    {
        public Func<TEntity, TResult> Map { get; set; }
        public Type EntityType { get { return typeof(TEntity); } }
        public Type ResultType { get { return typeof(TResult); } }
        public object ApplyMap(IDocument entity)
        {
            return Map.Invoke((TEntity) entity);
        }

    }
}