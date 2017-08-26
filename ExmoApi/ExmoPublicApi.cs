using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ExmoApi
{
    public class ExmoPublicApi : ExmoApiBase
    {
        public ExmoPublicApi(string apiAddress = "https://api.exmo.com/") : base(apiAddress) { }

        public async Task<dynamic> TradesAsync(string pair) =>
            JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("trades",
                new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("pair", pair)
                })));

        public async Task<dynamic> OrderBookAsync(string pair, int limit = 100) =>
            JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("order_book",
                new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("pair", pair),
                    new KeyValuePair<string, string>("limit", limit.ToString())
                })));

        public async Task<dynamic> TickerAsync() => JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("ticker"));

        public async Task<dynamic> PairSettingsAsync() => JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("pair_settings"));

        public async Task<dynamic> CurrencyAsync() => JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("currency"));
    }
}
