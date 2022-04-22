using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhishingDetector.Models;
using PhishingDetector.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PhishingDetector.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public PredictionResult PredictionResult;
        private readonly IPhishingPredictor phishingPredictor;

        public IndexModel(ILogger<IndexModel> logger, IPhishingPredictor phishingPredictor)
        {
            _logger = logger;
            this.phishingPredictor = phishingPredictor;
        }

        public void OnGet()
        {
            var json = this.HttpContext.Session.GetString("ValidationResult");
            if (!string.IsNullOrWhiteSpace(json))
            {
                PredictionResult = JsonSerializer.Deserialize<PredictionResult>(json);
            }
        }

        [BindProperty]
        public string UrlToValidate { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation($"User submited {UrlToValidate} for prediction");
                var isValid = Uri.TryCreate(UrlToValidate, UriKind.RelativeOrAbsolute, out Uri uri);
                if (!isValid || uri == null)
                {
                    PredictionResult = new PredictionResult(UrlToValidate, false, "Sorry invalid URL: " + UrlToValidate);
                    _logger.LogError($"{UrlToValidate} is invalid");
                    
                }
                else
                {
                    _logger.LogInformation($"{UrlToValidate} is valid");
                    var resquest = new RequestData(uri);
                    PredictionResult = await phishingPredictor.Predict(resquest, UrlToValidate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown onPost while prediction URL");
                PredictionResult = new PredictionResult(UrlToValidate, false, "Sorry error occurred, please try again");
            }
            finally
            {
                var json = JsonSerializer.Serialize(PredictionResult);
                _logger.LogInformation($"{UrlToValidate} result {json}");
                this.HttpContext.Session.SetString("ValidationResult", json);
            }

            return RedirectToPage();
        }

    }
}

