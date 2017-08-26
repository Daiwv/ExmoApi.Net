using System;

namespace ExmoApi.Sample
{
    class Program
    {
        private static ExmoPublicApi publicApi;
        private static ExmoAuthenticatedAPI authenticatedAPI;
        private static ExmoWalletApi walletApi;

        static void Main(string[] args)
        {
            publicApi = new ExmoPublicApi(ExmoApiBase.ExmoMeApiAddress);
            authenticatedAPI = new ExmoAuthenticatedAPI(
                Properties.Resources.ResourceManager.GetObject("Key").ToString(),
                Properties.Resources.ResourceManager.GetObject("Secret").ToString(),
                ExmoApiBase.ExmoMeApiAddress);
            walletApi = new ExmoWalletApi(authenticatedAPI);

            var pairSettings = publicApi.PairSettingsAsync().GetAwaiter().GetResult();
            var userInfo = authenticatedAPI.UserInfoAsync().GetAwaiter().GetResult();
            var orderCreate = authenticatedAPI.OrderCreateAsync("ZEC_USD", 1000, 10000000, "sell").GetAwaiter().GetResult();
            var walletHistory = walletApi.WalletHistoryAsync(DateTime.Now).GetAwaiter().GetResult();

            Console.ReadKey();
        }
    }
}
