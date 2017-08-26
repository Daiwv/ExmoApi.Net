using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExmoApi.Extensions;

namespace ExmoApi
{
    public class ExmoWalletApi : ExmoApiBase, IEquatable<ExmoWalletApi>
    {
        private ExmoAuthenticatedAPI apiSource;

        public ExmoWalletApi(string key, string secret, string apiAddress = "https://api.exmo.com/") : base(apiAddress)
        {
            apiSource = new ExmoAuthenticatedAPI(key, secret, apiAddress);
        }

        public ExmoWalletApi(ExmoAuthenticatedAPI authenticatedAPI) : base(authenticatedAPI.ApiAddress.ToString())
        {
            apiSource = authenticatedAPI;
        }

        public async Task<dynamic> WalletHistoryAsync(DateTime date)
        {
            var content = apiSource.GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("date", date.ToUnixTimestamp().ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("wallet_history", content));
        }

        public bool Equals(ExmoWalletApi other) => apiSource.Equals(other.apiSource);
    }
}
