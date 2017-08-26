using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using ExmoApi.Extensions;

namespace ExmoApi
{
    public class ExmoAuthenticatedAPI : ExmoApiBase, IEquatable<ExmoAuthenticatedAPI>
    {
        public string Key { get; private set; }
        public string Secret { get; private set; }

        public ExmoAuthenticatedAPI(string key, string secret, string apiAddress = "https://api.exmo.com/") : base(apiAddress)
        {
            this.Key = key;
            this.Secret = secret;
        }

        public async Task<dynamic> UserInfoAsync()
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[] { });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("user_info", content));
        }

        public async Task<dynamic> OrderCreateAsync(string pair, double quantity, double price, string type)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("pair", pair),
                new KeyValuePair<string, string>("quantity", quantity.ToString()),
                new KeyValuePair<string, string>("price", price.ToString()),
                new KeyValuePair<string, string>("type", type)
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("order_create", content));
        }

        public async Task<dynamic> OrderCancelAsync(long orderId)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("order_id", orderId.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("order_cancel", content));
        }

        public async Task<dynamic> UserOpenOrdersAsync()
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[] { });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("user_open_orders", content));
        }

        public async Task<dynamic> UserTradesAsync(string pair, int offset = 0, int limit = 100)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("pair", pair),
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("limit", limit.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("user_trades", content));
        }

        public async Task<dynamic> UserCancelledOrders(int offset = 0, int limit = 100)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("limit", limit.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("user_cancelled_orders", content));
        }

        public async Task<dynamic> OrderTradesAsync(long orderId)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("order_id", orderId.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("order_trades", content));
        }

        public async Task<dynamic> RequiredAmountAsync(string pair, double quantity)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("pair", pair.ToString()),
                new KeyValuePair<string, string>("quantity", quantity.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("required_amount", content));
        }

        public async Task<dynamic> WithdrawCryptAsync(double amount, string currency, string address)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("amount", amount.ToString()),
                new KeyValuePair<string, string>("currency", currency),
                new KeyValuePair<string, string>("address", address.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("withdraw_crypt", content));
        }

        public async Task<dynamic> WithdrawGetTxId(long taskId)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("taskId", taskId.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("withdraw_get_txid", content));
        }

        internal FormUrlEncodedContent GenerateAuthenticatedContent(IEnumerable<KeyValuePair<string, string>> methodParams)
        {
            var paramList = methodParams.ToList();
            paramList.Add(new KeyValuePair<string, string>("nonce", DateTime.Now.ToUnixTimestamp().ToString()));

            var content = new FormUrlEncodedContent(paramList);
           
            content.Headers.Add("Key", Key);
            content.Headers.Add("Sign", CalculateSign(paramList));

            return content;
        }
        internal string CalculateSign(IEnumerable<KeyValuePair<string, string>> methodParams)
        {
            var data = MethodParamsQueryString();
            IEnumerable<string> bytesArray;
            using (var hma = new HMACSHA512(Encoding.UTF8.GetBytes(Secret)))
            {
                bytesArray = hma.ComputeHash(Encoding.UTF8.GetBytes(data)).Select(b => b.ToString("X2"));
            }
            var r = string.Join(string.Empty, bytesArray).ToLower();
            return r;

            string MethodParamsQueryString()
            {
                var result = string.Empty;
                var list = new List<string>();
                foreach(var pair in methodParams)
                {
                    list.Add($"{pair.Key}={pair.Value}");
                }
                return string.Join("&", list);
            }
        }

        public bool Equals(ExmoAuthenticatedAPI other) => Key.Equals(other.Key) && Secret.Equals(other.Secret);
    }
}
