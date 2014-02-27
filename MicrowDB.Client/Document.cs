namespace MicrowDB.Client
{
    public class Document : IDocument
    {
        public string Id { get; set; }
        public string TypeName { get; set; }
        public string Json { get; set; }
    }
}