# ExmoApi.Net
https://exmo.me/en/api (https://exmo.com/en/api) .NET implementation

## NuGet
[ExmoApi at NuGet](https://www.nuget.org/packages/ExmoApi)

    Install-Package ExmoApi
    
    
 Using Exmo Public API:
----------------------
    var publicApi = new ExmoPublicApi();
    var currencies = await publicApi.CurrencyAsync();

Using Exmo Authenticated API:
-----------------------------
    var authenticatedApi = new ExmoAuthenticatedAPI(key, secret);
    var userInfo = await authenticatedApi.UserInfoAsync();

Using Exmo Wallet API:
----------------------
    var walletApi = new ExmoWalletApi(key, secret);
    var history = walletApi.WalletHistoryAsync(DateTime.Today);

If https://api.exmo.com/ not working:
------------------------------------
    var publicApi = new ExmoPublicApi(ExmoApiBase.ExmoMeApiAddress); //or use another exmo mirror
