using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace ExmoApi
{
    /// <summary>
    /// Base ExmoApi class
    /// </summary>
    public abstract class ExmoApiBase
    {
        /// <summary>
        /// Link to https://api.exmo.com/
        /// </summary>
        public static string ExmoComApiAddress { get; private set; } = "https://api.exmo.com/";
        /// <summary>
        /// Link to https://api.exmo.me/
        /// </summary>
        public static string ExmoMeApiAddress { get; private set; } = "https://api.exmo.me/";

        /// <summary>
        /// ExmoAPI base url
        /// </summary>
        public Uri ApiAddress { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="apiAddress">ExmoAPI base url</param>
        public ExmoApiBase(string apiAddress)
        {
            ApiAddress = new Uri(apiAddress);
        }
        
        /// <summary>
        /// Executes method by name
        /// </summary>
        /// <param name="methodName">Method name which will be called</param>
        /// <param name="content">Contains method parameters which will be sent to server</param>
        /// <returns></returns>
        protected async Task<string> NativeMethodCallAsync(string methodName, FormUrlEncodedContent content = null)
        {
            string response;
            using (var handler = new HttpClientHandler())
            {
                handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var http = new HttpClient(handler))
                {
                    var url = $"{ApiAddress}v1/{methodName}";

                    var httpResponse = await http.PostAsync(url, content);
                    response = await httpResponse.Content.ReadAsStringAsync();
                }
            }

            return response;
        }
    }
}
