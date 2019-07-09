namespace AngularPerformanceSamples.Models
{
    public class DocumentObject : IDocumentObject
    {
        public DocumentObject(string id, string data)
        {
            Id = id;
            DataId = id;
            Data = data;
        }
        public string Id { get; set; }
        public string Data { get; set; }
        public string DataId { get; set; }
    }
}
