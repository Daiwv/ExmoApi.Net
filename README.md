# ExmoApi.Net
http://api.exmo.me (http://api.exmo.com) api .net implementation

 Using Exmo Public API:
----------------------
    var publicApi = new ExmoPublicApi(ExmoApiBase.ExmoMeApiAddress);
    var currencies = await publicApi.CurrencyAsync();

Using Exmo Authenticated API:
-----------------------------
    var authenticatedApi = new ExmoAuthenticatedAPI(key, secret);
    var userInfo = await authenticatedApi.UserInfoAsync();

Using Exmo Wallet API:
----------------------
    var walletApi = new ExmoWalletApi(key, secret);
    var history = walletApi.WalletHistoryAsync(DateTime.Today);
