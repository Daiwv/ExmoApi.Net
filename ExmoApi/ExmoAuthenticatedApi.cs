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
    /// <summary>
    /// Provides Exmo Authenticated API functionality
    /// </summary>
    public class ExmoAuthenticatedApi : ExmoApiBase, IEquatable<ExmoAuthenticatedApi>
    {
        /// <summary>
        /// Public key that can be found in user’s profile settings. 
        /// Example: K-7cc97c89aed2a2fd9ed7792d48d63f65800c447b
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Private key that can be found in user’s profile settings. 
        /// Example: S-7cc97c89aed2a2fd9ed7792d48d63f65800c447b
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Initializes Exmo Authenticated API provider
        /// </summary>
        /// <param name="key">Public key that can be found in user’s profile settings. Example: K-7cc97c89aed2a2fd9ed7792d48d63f65800c447b</param>
        /// <param name="secret">Private key that can be found in user’s profile settings. Example: S-7cc97c89aed2a2fd9ed7792d48d63f65800c447b</param>
        /// <param name="apiAddress">ExmoAPI base url</param>
        public ExmoAuthenticatedApi(string key, string secret, string apiAddress = "https://api.exmo.com/") : base(apiAddress)
        {
            this.Key = key;
            this.Secret = secret;
        }

        /// <summary>
        /// Getting information about user's account
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> UserInfoAsync()
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[] { });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("user_info", content));
        }

        /// <summary>
        /// Order creation
        /// </summary>
        /// <param name="pair">Currency pair</param>
        /// <param name="quantity">Quantity for the order</param>
        /// <param name="price">Price for the order</param>
        /// <param name="type">Type of order, can have the following values: buy, sell, market_buy, market_sell, market_buy_total, market_sell_total</param>
        /// <returns></returns>
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

        /// <summary>
        /// Order cancellation
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns></returns>
        public async Task<dynamic> OrderCancelAsync(long orderId)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("order_id", orderId.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("order_cancel", content));
        }

        /// <summary>
        /// Getting the list of user’s active orders
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> UserOpenOrdersAsync()
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[] { });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("user_open_orders", content));
        }

        /// <summary>
        /// Getting the list of user’s deals
        /// </summary>
        /// <param name="pair">One or various currency pairs separated by commas (example: BTC_USD,BTC_EUR)</param>
        /// <param name="offset">Last deal offset (default: 0)</param>
        /// <param name="limit">The number of returned deals (default: 100, мmaximum: 10 000)</param>
        /// <returns></returns>
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

        /// <summary>
        /// Getting the list of user’s cancelled orders
        /// </summary>
        /// <param name="offset">Last deal offset (default: 0)</param>
        /// <param name="limit">The number of returned deals (default: 100, мmaximum: 10 000)</param>
        /// <returns></returns>
        public async Task<dynamic> UserCancelledOrders(int offset = 0, int limit = 100)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("limit", limit.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("user_cancelled_orders", content));
        }

        /// <summary>
        /// Getting the history of deals with the order
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns></returns>
        public async Task<dynamic> OrderTradesAsync(long orderId)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("order_id", orderId.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("order_trades", content));
        }

        /// <summary>
        /// Calculating the sum of buying a certain amount of currency for the particular currency pair
        /// </summary>
        /// <param name="pair">Currency pair</param>
        /// <param name="quantity">Quantity to buy</param>
        /// <returns></returns>
        public async Task<dynamic> RequiredAmountAsync(string pair, double quantity)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("pair", pair.ToString()),
                new KeyValuePair<string, string>("quantity", quantity.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("required_amount", content));
        }

        /// <summary>
        /// Creation of the task for cryptocurrency withdrawal. ATTENTION!!! This API function is available only after request to the Technical Support.
        /// </summary>
        /// <param name="amount">Amount of currency to be withdrawn</param>
        /// <param name="currency">Name of the currency to be withdrawn</param>
        /// <param name="address">Withdrawal adress</param>
        /// <returns></returns>
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

        /// <summary>
        /// Getting the transaction ID in order to keep track of it on blockchain
        /// </summary>
        /// <param name="taskId">Withdrawal task identifier</param>
        /// <returns></returns>
        public async Task<dynamic> WithdrawGetTxIdAsync(long taskId)
        {
            var content = GenerateAuthenticatedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("task_id", taskId.ToString())
            });
            return JsonConvert.DeserializeObject(await this.NativeMethodCallAsync("withdraw_get_txid", content));
        }

        /// <summary>
        /// Generate content using key, private key, method parameters
        /// </summary>
        /// <param name="methodParams">Method parameters</param>
        /// <returns></returns>
        internal FormUrlEncodedContent GenerateAuthenticatedContent(IEnumerable<KeyValuePair<string, string>> methodParams)
        {
            var paramList = methodParams.ToList();
            paramList.Add(new KeyValuePair<string, string>("nonce", DateTime.Now.ToUnixTimestamp().ToString()));

            var content = new FormUrlEncodedContent(paramList);
           
            content.Headers.Add("Key", Key);
            content.Headers.Add("Sign", CalculateSign(paramList));

            return content;
        }

        /// <summary>
        /// Calculate signature by key, private key and method parameters
        /// </summary>
        /// <param name="methodParams">Method parameters</param>
        /// <returns></returns>
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

        public bool Equals(ExmoAuthenticatedApi other) => Key.Equals(other.Key) && Secret.Equals(other.Secret);
    }
}
