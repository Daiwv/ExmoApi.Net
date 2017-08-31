using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExmoApi.Extensions;

namespace ExmoApi
{
    /// <summary>
    /// Provides Exmo Wallet API functionality
    /// </summary>
    public class ExmoWalletApi : ExmoApiBase, IEquatable<ExmoWalletApi>
    {
        /// <summary>
        /// Api source for wallet Api
        /// </summary>
        public ExmoAuthenticatedApi AuthenticatedApi { get; private set; }

        /// <summary>
        /// Initializes Exmo Wallet API provider
        /// </summary>
        /// <param name="key">Public key that can be found in user’s profile settings. Example: K-7cc97c89aed2a2fd9ed7792d48d63f65800c447b</param>
        /// <param name="secret">Private key that can be found in user’s profile settings. Example: S-7cc97c89aed2a2fd9ed7792d48d63f65800c447b</param>
        /// <param name="apiAddress">ExmoAPI base url</param>
        public ExmoWalletApi(string key, string secret, string apiAddress = "https://api.exmo.com/") : base(apiAddress)
        {
            AuthenticatedApi = new ExmoAuthenticatedApi(key, secret, apiAddress);
        }

        /// <summary>
        /// Initializes Exmo Wallet API provider
        /// </summary>
        /// <param name="authenticatedAPI">This provider will be used</param>
        public ExmoWalletApi(ExmoAuthenticatedApi authenticatedAPI) : base(authenticatedAPI.ApiAddress.ToString())
        {
            AuthenticatedApi = authenticatedAPI;
        }

        /// <summary>
        /// Get history of wallet
        /// </summary>
        /// <param name="date">Timestamp of the day</param>
        /// <returns></returns>
        public async Task<dynamic> WalletHistoryAsync(DateTime date)
        {
            var content = AuthenticatedApi.GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("date", date.ToUnixTimestamp().ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("wallet_history", content));
        }

        public bool Equals(ExmoWalletApi other) => AuthenticatedApi.Equals(other.AuthenticatedApi);
    }
}
