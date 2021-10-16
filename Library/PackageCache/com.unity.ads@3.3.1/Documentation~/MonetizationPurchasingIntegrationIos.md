# Custom purchasing integration for iOS developers
## Overview
This guide covers integration for implementing a purchasing delegate in your iOS game, to take advantage of Unity's optimization features.

* If you are a Unity developer using C#, [click here](MonetizationPurchasingIntegrationUnity.md) 
* If you are an Android developer using Java, [click here](MonetizationPurchasingIntegrationAndroid.md).

### Guide contents
1. [Configure your game](#configuring-your-game-for-unity-ads) for Unity Ads.
2. [Configure your Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html) on the [developer dashboard](https://operate.dashboard.unity3d.com). 
3. [Configure an IAP Promo](https://docs.unity3d.com/Manual/IAPPromoPromotions.html) on the developer dashboard. 
4. Implement a purchasing delegate in your game code, using the [`USRVUnityPurchasing`](MonetizationResourcesApiIos.md#usrvunitypurchasing) API.
5. [Implement game logic and purchase events](#implementing-the-purchasing-delegate) within the purchasing delegate.
6. [Initialize](#initializing-the-purchasing-delegate-and-sdk) the purchasing delegate and Ads SDK.

## Implementation
### Configuring your game for Unity Ads
To implement a purchasing delegate, you must integrate Unity Ads in your game. To do so, follow the steps in the [basic ads integration guide](MonetizationBasicIntegrationIos.md) that detail the following:

* [Creating a Project in the Unity developer dashboard](MonetizationBasicIntegrationIos.md#creating-a-project-in-the-unity-developer-dashboard)
* [Importing the Unity Ads framework](MonetizationBasicIntegrationIos.md#importing-the-unity-ads-framework)

**Important**: Custom purchase integration requires the Unity Ads SDK version 3.0 or higher.

Once your game is configured for Unity Ads, proceed to creating a Product Catalog.

### Configuring Product Catalogs on the developer dashboard
Before implementing your purchasing delegate, navigate to the Operate tab of the [developer dashboard](https://operate.dashboard.unity3d.com), then follow the manual configuration instructions for [populating a Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html).  

### Configuring Promotions on the developer dashboard
From the Operate tab of the [developer dashboard](https://operate.dashboard.unity3d.com), follow the instructions for [configuring an IAP Promo](https://docs.unity3d.com/Manual/IAPPromo.html).  

### Implementing the purchasing delegate
In your game script, include the `<UnityAds/USRVUnityPurchasingDelegate.h>` delegate, then implement a `USRVUnityPurchasing` class that manages the [`USRVUnityPurchasingDelegate`](MonetizationResourcesApiIos.md#usrvunitypurchasingdelegate) interface. You will use two methods (`loadProducts` and `purchaseProduct`) to define the game logic you want the purchasing adapter to use. You must implement them so the SDK can call them as needed when managing your Product transactions.

```
#import <UnityAds/USRVUnityPurchasingDelegate.h>

@interface MyPurchasing <USRVUnityPurchasingDelegate>
@end

// Placeholder for loadProducts function

// Placeholder for purchaseProduct function
```

#### Retrieving your Product Catalog
The SDK calls `loadProducts` to retrieve the list of available Products, using the `UnityPurchasingLoadProductsCompletionHandler` function to convert `UPURProduct` objects into `Monetization.Product` objects. This requires a minimum of the following properties for each Product: 

* `productID`
* `localizedPriceString`
* `productType`
* `isoCurrencyCode`
* `localizedTitle`

#### Processing a purchase
The SDK calls `purchaseProduct` when a user clicks the buy button for a promotional asset. The purchase’s success or failure handling depends on your in-app purchasing implementation. 

If the transaction succeeds, call the `UnityPurchasingTransactionCompletionHandler` function using a `UPURTransactionDetails` object. 

If the transaction fails, call the `UnityPurchasingTransactionErrorHandler` function, using a `UPURTransactionError` object.

Finally, set the delegate using `[USRVUnityPurchasing setDelegate: [[MyPurchasing alloc] init]]`.

##### Example delegate implementation
```
#import <UnityAds/USRVUnityPurchasingDelegate.h>

@interface MyPurchasing <USRVUnityPurchasingDelegate>
@end

@implementation MyPurchasing
-(void) loadProducts: (UnityPurchasingLoadProductsCompletionHandler) completionHandler {

    completionHandler (@[[UPURProduct build: ^(UPURProductBuilder *builder) {
        builder.productId = @"100BronzeCoins";
        builder.localizedTitle = @"100 Bronze Coins";
        builder.localizedPriceString = @"$1.99";
        builder.localizedPrice = [NSDecimalNumber decimalNumberWithString: @"1.99"];
        builder.isoCurrencyCode = @"USD";
        builder.localizedDescription = @"Awesome Bronze Coins available for a low price!";
        builder.productType = @"Consumable";
    }]]);
}

-(void) purchaseProduct: (NSString *) productId completionHandler: (UnityPurchasingTransactionCompletionHandler) completionHandler errorHandler: (UnityPurchasingTransactionErrorHandler) errorHandler userInfo: (nullable NSDictionary *) extras {
    thirdPartyPurchasing.purchase (productId); // Generic developer purchasing function
   
    // If purchase succeeds:
    completionHandler ([UPURTransactionDetails build: ^(UPURTransactionDetailsBuilder *builder) {
        builder.productId = productId;
        builder.transactionId = thirdPartyPurchasing.transactionId;
        builder.currency = @"USD";
        builder.price = [NSDecimalNumber decimalNumberWithString: @"1.99"];
        builder.receipt = @"{\n\"data\": \"{\\\"Store\\\":\\\"fake\\\",\\\"TransactionID\\\":\\\"ce7bb1ca-bd34-4ffb-bdee-83d2784336d8\\\",\\\"Payload\\\":\\\"{ \\\\\\\"this\\\\\\\": \\\\\\\"is a fake receipt\\\\\\\" }\\\"}\"\n}";
    }]);

    // If purchase fails:
    errorHandler (kUPURTransactionErrorNetworkError, nil);
}
@end
```

### Initializing the purchasing delegate and SDK
After you’ve implemented the purchasing adapter, you must provide a reference to it using the `setDelegate` method. Finally, you must [initialize](MonetizationBasicIntegrationIos.md#initializing-the-sdk) the SDK using your Project’s Game ID for the appropriate platform. You can locate the ID on the **Operate** tab of the [developer dashboard](https://operate.dashboard.unity3d.com/) by selecting the Project, then selecting **Settings** > **Project Settings** from the left navigation bar (see the [Dashboard guide](MonetizationResourcesDashboardGuide.md#project-settings) section on **Project Settings** for details). 

To avoid errors, implement these calls as early as possible in your game’s run-time life cycle. For example:

```
#import "ViewController.h"

@implementation ViewController

- (void) viewDidLoad {
    [super viewDidLoad];
    [USRVUnityPurchasing setDelegate: [[MyPurchasing alloc] init]];
    [UnityMonetization initialize: @"1234567" delegate: self testMode: true];
}
@end
```

## What's next?
Learn how to [implement custom IAP Promo assets](MonetizationNativePromoIos.md) in your game, or return to the [Monetization](Monetization.md) hub.