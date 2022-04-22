using System.Net.Http.Headers;

namespace PhishingDetector.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddHttpClient("AzureML", httpClient =>
             {
                 var baseUrl = configuration["AzureML:URL"];
                 var apiKey = configuration["AzureML:PrimaryKey"];
                 httpClient.BaseAddress = new Uri(baseUrl);
                 httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

             });
            services.AddScoped<IPhishingPredictor, PhishingPredictor>();
        }
    }
}
