using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ExmoApi
{
    /// <summary>
    /// Provides Exmo Public API functionality
    /// </summary>
    public class ExmoPublicApi : ExmoApiBase
    {
        /// <summary>
        /// Initializes Exmo Public API provider
        /// </summary>
        /// <param name="apiAddress">ExmoAPI url</param>
        public ExmoPublicApi(string apiAddress = "https://api.exmo.com/") : base(apiAddress) { }

        /// <summary>
        /// List of the deals on currency pairs
        /// </summary>
        /// <param name="pair">One or various currency pairs separated by commas (example: BTC_USD,BTC_EUR)</param>
        /// <returns></returns>
        public async Task<dynamic> TradesAsync(string pair) =>
            JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("trades",
                new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("pair", pair)
                })));

        /// <summary>
        /// The book of current orders on the currency pair
        /// </summary>
        /// <param name="pair">One or various currency pairs separated by commas (example: BTC_USD,BTC_EUR)</param>
        /// <param name="limit">The number of displayed positions (default: 100, max: 1000)</param>
        /// <returns></returns>
        public async Task<dynamic> OrderBookAsync(string pair, int limit = 100) =>
            JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("order_book",
                new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("pair", pair),
                    new KeyValuePair<string, string>("limit", limit.ToString())
                })));

        /// <summary>
        /// Statistics on prices and volume of trades by currency pairs
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> TickerAsync() => JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("ticker"));

        /// <summary>
        /// Currency pairs settings
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> PairSettingsAsync() => JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("pair_settings"));

        /// <summary>
        /// Currencies list
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> CurrencyAsync() => JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("currency"));
    }
}
