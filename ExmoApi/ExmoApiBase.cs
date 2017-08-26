using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace ExmoApi
{
    public abstract class ExmoApiBase
    {
        public static string ExmoComApiAddress = "https://api.exmo.com/";
        public static string ExmoMeApiAddress = "https://api.exmo.me/";

        public Uri ApiAddress { get; private set; }

        public ExmoApiBase(string apiAddress)
        {
            ApiAddress = new Uri(apiAddress);
        }
        
        protected async Task<string> NativeMethodCallAsync(string methodName, FormUrlEncodedContent content = null)
        {
            string response;
            using (var handler = new HttpClientHandler())
            {
                handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var http = new HttpClient(handler))
                {
                    var url = $"{ApiAddress}v1/{methodName}";
                    response = await (await http.PostAsync(url, content))
                        .Content.ReadAsStringAsync();
                }
            }

            return response;
        }
    }
}
