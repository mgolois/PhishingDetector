namespace PhishingDetector.Models
{
    public class PredictionResult
    {
        public PredictionResult()
        {
        }

        public PredictionResult(string url, bool issuccess, string errorMsg = "", ResponseData data = null)
        {
            IsSuccess = issuccess;
            ErrorMessage = errorMsg;
            Data = data;
            Url = url;
        }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public ResponseData Data { get; set; }
        public string Url { get; set; }
    }
}
