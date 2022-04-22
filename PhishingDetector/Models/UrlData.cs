using System.Net;
using System.Text.Json.Serialization;

namespace PhishingDetector.Models
{
    public class UrlData
    { //1 means legitimate 0 is suspicious -1 is phishing

        const int PHISHING = -1;
        const int SUSPICIOUS = 0;
        const int LEGITIMATE = 1;
        private Uri uri;
        public UrlData() 
        {
            uri = new Uri(string.Empty);
        }
        public UrlData(Uri uri)
        {
            this.uri = uri;
        }

        [JsonPropertyName("hasIpAddress")]
        public int HasIpAddress
        {
            get
            {
                var isValidIP = IPAddress.TryParse(uri.Host, out var _);
                return isValidIP ? PHISHING : LEGITIMATE;
            }
        }

        public int LongUrl
        {
            get
            {
                return uri.ToString().Length switch
                {
                    int n when n < 30 => LEGITIMATE,
                    int n when n >= 30 && n <= 50 => SUSPICIOUS,
                    _ => PHISHING,
                };
            }
        }

        public int ShorteningService
        {
            get
            {
                return uri.Host.Length < 5 ? PHISHING : LEGITIMATE;
            }
        }

        public int HasAtSymbol
        {
            get
            {
                return uri.ToString().Contains("@") ? 1 : -1;
            }
        }

        public int HasDoubleSlash
        {
            get
            {
                return uri.PathAndQuery.ToString().Contains("//") ? PHISHING : LEGITIMATE;
            }
        }

        public int HasDash
        {
            get { return uri.PathAndQuery.ToString().Contains('-') ? PHISHING : LEGITIMATE; }
        }

        public int HasSubdomain
        {
            get
            {
                var host = uri.Host;
                if (host.ToLower().StartsWith("www."))
                {
                    host = host.Remove(0, 4);
                }
                return host.Split('.').Length switch
                {
                    int n when n == 1 => LEGITIMATE,
                    int n when n > 2 => PHISHING,
                    _ => SUSPICIOUS,
                };
            }
        }

        public int IsHttps
        {
            get { return uri.Scheme.ToLower().Equals("https") ? LEGITIMATE : PHISHING; }
        }

        public int HasPort
        {
            get
            {
                var validPorts = new int[] { 443, 80 };
                return validPorts.Contains(uri.Port) ? LEGITIMATE : PHISHING;
            }
        }

        public int IncludeHttpsInUrl
        {
            get { return uri.Host.Contains("http") ? PHISHING : LEGITIMATE; }
        }

        public int LongDomainRegistered
        {
            get { return LEGITIMATE; }
        }
    }
}
