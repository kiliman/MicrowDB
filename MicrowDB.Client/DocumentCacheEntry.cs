namespace MicrowDB.Client
{
    public class DocumentCacheEntry
    {
        public IDocument Entity { get; set; }
        public string Json { get; set; }
        public bool IsDeleted { get; set; }
    }
}