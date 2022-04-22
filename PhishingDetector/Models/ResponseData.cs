namespace PhishingDetector.Models
{
    public class ResponseData
    {
        public int[] Results { get; set; }

        public string Prediction
        {
            get
            {
                var data = Results?.FirstOrDefault();

                return data switch
                {
                    1 => "Legitimate url",
                    0 => "Suspicious url",
                    -1 => "Phishing url",
                    _ => "N/A",
                };
            }
        }

        public string PredictionClassName
        {
            get
            {
                var data = Results?.FirstOrDefault();

                return data switch
                {
                    1 => "alert-success",
                    0 => "alert-warning",
                    -1 => "alert-danger",
                    _ => "alert-secondary",
                };
            }
        }

    }
}
