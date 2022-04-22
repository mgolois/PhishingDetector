using PhishingDetector.Models;

namespace PhishingDetector.Services
{
    public interface IPhishingPredictor
    {
        Task<PredictionResult> Predict(RequestData reqData, string url);
    }
}