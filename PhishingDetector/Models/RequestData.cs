namespace PhishingDetector.Models
{
    public class RequestData
    {
        public RequestData() { }
        public RequestData(Uri uri) 
        {
            Inputs = new InputData();
            Inputs.Data = new List<UrlData> { new UrlData(uri) };
        }
        public InputData Inputs { get; set; }
        public GlobalParameters GlobalParameters { get; set; } = new GlobalParameters();
    }
}
