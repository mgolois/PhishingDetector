using PhishingDetector.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PhishingDetector.Services
{
    public class PhishingPredictor : IPhishingPredictor
    {
        private HttpClient httpClient;
        private ILogger<PhishingPredictor> logger;

        public PhishingPredictor(IHttpClientFactory httpClientFactory, ILogger<PhishingPredictor> logger)
        {
            httpClient = httpClientFactory.CreateClient("AzureML");
            this.logger = logger;
        }

        public async Task<PredictionResult> Predict(RequestData reqData, string url)
        {
            logger.LogInformation("Prediction processing");
            var result = new PredictionResult();
            result.Url = url;
            try
            {
                var requestString = JsonSerializer.Serialize(reqData);
                logger.LogInformation("Predicting data: " + requestString);

                var content = new StringContent(requestString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync("/score", content);
                logger.LogInformation($"POST t0 {httpClient.BaseAddress}/score completed");

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("Prediction successfull");
                    var responseData = await response.Content.ReadFromJsonAsync<ResponseData>();
                    if (responseData == null)
                    {
                        logger.LogError("Unable to serialized response on successful prediction");
                        throw new NullReferenceException(nameof(responseData));
                    }
                    result.IsSuccess = true;
                    result.Data = responseData;
                    logger.LogInformation("Serialized successful response: " + result.Data);
                }
                else
                {
                    logger.LogError("Prediction not successfull");
                    result.ErrorMessage = "Sorry an error occurred, please try again";
                    result.IsSuccess = false;
                    var data = await response.Content.ReadAsStringAsync();
                    logger.LogError("Prediction failed message: " + data);
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.IsSuccess = false;
                logger.LogError(ex, "Exception thrown while predicting: " + ex.Message);
            }

            return result;

        }
    }
}
