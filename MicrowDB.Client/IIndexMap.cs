using System;

namespace MicrowDB.Client
{
    public interface IIndexMap
    {
        Type EntityType { get; }
        Type ResultType { get; }

        object ApplyMap(IDocument entity);
    }
}