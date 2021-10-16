# Custom purchasing integration for Unity developers
## Overview
This guide covers integration for implementing a purchasing adapter in your made-with-Unity game, to take advantage of Unity's optimization features without using Unity IAP.

* If you are an iOS developer using Objective-C, [click here](MonetizationPurchasingIntegrationIos.md). 
* If you are an Android developer using Java, [click here](MonetizationPurchasingIntegrationAndroid.md).

### Guide contents
1. [Configure your Project](#configuring-your-project-for-unity-ads) for Unity Ads.
2. Manually [configure your Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html) on the [developer dashboard](https://operate.dashboard.unity3d.com). 
3. [Configure an IAP Promo](https://docs.unity3d.com/Manual/IAPPromoPromotions.html) on the developer dashboard. 
4. Implement a purchasing adapter in your game code, using the [`Monetization.IPurchasingAdapter`](../api/UnityEngine.Monetization.IPurchasingAdapter.html) interface API.
5. [Implement game logic and purchase events](#implementing-the-purchasing-adapter) within the purchasing adapter.
6. [Initialize](#initializing-the-purchasing-adapter-and-sdk) the purchasing adapter and Unity Ads SDK.

## Implementation
### Configuring your Project for Unity Ads
To implement a purchasing adapter, you must integrate Unity Ads in your Project. To do so, follow the steps in the [basic ads integration guide](MonetizationBasicIntegrationUnity.md) that detail the following:

* [Setting build targets](MonetizationBasicIntegrationUnity.md#setting-build-targets)
* [Installing Unity Ads](MonetizationBasicIntegrationUnity.md#installing-unity-ads)

**Important**: Custom purchase integration requires the Unity Ads SDK version 3.0 or higher.

Once your Project is configured for Unity Ads, proceed to creating a Product Catalog.

### Configuring Product Catalogs on the developer dashboard
Before implementing your purchasing adapter, navigate to the **Operate** [tab of the developer dashboard](https://operate.dashboard.unity3d.com), then follow the manual configuration instructions for [populating a Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html).  

### Configuring Promotions on the developer dashboard
From the **Operate** [tab of the developer dashboard](https://operate.dashboard.unity3d.com), follow the instructions for [configuring an IAP Promo](https://docs.unity3d.com/Manual/IAPPromo.html). 

### Implementing the purchasing adapter
A purchasing adapter acts as the hook for the `Monetization` API to retrieve the information it needs from your custom IAP implementation. 

In your game script, include the `UnityEngine.Monetization` namespace, then create a class that implements a purchasing adapter. You will use two methods (`RetrieveProducts` and `Purchase`) to define the game logic you want the purchasing adapter to use. These methods require a class using the [`IPurchasingAdapter`](../api/UnityEngine.Monetization.IPurchasingAdapter.html) interface. You must implement them so the SDK can call them as needed when managing your Product transactions.

```
using UnityEngine.Monetization;

public class IAPManager: MonoBehaviour, IPurchasingAdapter {
    // Implement RetrieveProducts and OnPurchase functions here
}
```

#### Retrieving your Product Catalog
The SDK calls `RetrieveProducts` to retrieve the list of available Products. The function must convert your IAP products into [`Monetization.Product`](../api/UnityEngine.Monetization.Product.html) objects. This requires a minimum of the following properties defined for each product: 

* `productID`
* `localizedPriceString`
* `productType`
* `isoCurrencyCode`
* `localizedTitle` 

The function takes an [`IRetrieveProductsListener`](../api/UnityEngine.Monetization.IRetrieveProductsListener.html) listener. Call the listener’s `OnProductsRetrieved` function to send the retrieved Products list back to the SDK. 

#### Processing a purchase
The SDK calls `Purchase` when a user clicks the buy button for a promotional asset. The purchase’s success or failure handling depends on your in-app purchasing implementation. 

The function takes an [`ITransactionListener`](../api/UnityEngine.Monetization.ITransactionListener.html) listener.  

* If the transaction succeeds, call the listener’s `OnTransactionComplete` method using a [`TransactionDetails`](../api/UnityEngine.Monetization.TransactionDetails.html) object.   
* If the transaction fails, call the listener’s `OnTransactionError` method, using a [`TransactionErrorDetails`](/api/UnityEngine.Monetization.TransactionErrorDetails.html) object.

#### Example adapter implementation
```
using UnityEngine.Monetization;

public class IAPAdapter: MonoBehaviour, IPurchasingAdapter {

    // Retrieve and provide your Product Catalog for the SDK:    
    public void RetrieveProducts (IRetrieveProductsListener listener) {    

        // Query your Products here, convert them to Monetization.Products, then populate the Product list with them:        
        List<Product> products = new List<Product> ();
        products.Add (new Product) {
            productId = "100bronzeCoins",
            localizedTitle = "100 Bronze Coins",
            localizedDescription = "Awesome Bronze Coins for a new low price!",
            localizedPriceString = "$1.99",
            isoCurrencyCode = "USD",
            productType = "Consumable",
            localizedPrice = 1.99m
        });

        // provide the retrieved Products list:
        listener.OnProductsRetrieved (products);
    }

    // Define game logic for handling purchases:    
    public void Purchase (string productId, ITransactionListener listener, IDictionary<string, object> dict) {
        // Example third-party purchasing function:
        ThirdPartyPurchasing.purchaseProduct (productId);

        // When ThirdPartyPurchasing succeeds:
        listener.OnTransactionComplete (new TransactionDetails {
            currency = "USD",
            price = 1.99m,
            productId = "100bronzeCoins",
            transactionId = ThirdPartyPurchasing.transactionId,
            receipt =
                "{\n\"data\": \"{\\\"Store\\\":\\\"fake\\\",\\\"TransactionID\\\":\\\"ce7bb1ca-bd34-4ffb-bdee-83d2784336d8\\\",\\\"Payload\\\":\\\"{ \\\\\\\"this\\\\\\\": \\\\\\\"is a fake receipt\\\\\\\" }\\\"}\"\n}"
        });

        // When ThirdPartyPurchasing fails:
        listener.OnTransactionError (new TransactionErrorDetails {
            transactionError = TransactionError.NetworkCancelled,
            exceptionMessage = "Test exception message",
            store = Store.GooglePlay,
            storeSpecificErrorCode = "Example: Google Play lost connection",
        });
    }
}    
```

### Initializing the purchasing adapter and SDK
After you’ve implemented the purchasing adapter, you must provide a reference to it using `SetPurchasingAdapter`. Finally, you must [initialize](MonetizationBasicIntegrationUnity.md#initializing-the-sdk) the SDK using your Project’s Game ID for the appropriate platform. You can locate the ID on the **Operate** tab of the [Developer Dashboard](https://operate.dashboard.unity3d.com/) by selecting the Project, then selecting **Settings** > **Project Settings** from the left navigation bar (see the [Dashboard guide](MonetizationResourcesDashboardGuide.md#project-settings) section on **Project Settings** for details). 

To avoid errors, implement these calls as early as possible in your game’s run-time life cycle. For example:

```
Start () {
    Monetization.SetPurchasingAdapter (GameObject.Find ("IAPManager").GetComponent<IAPAdapter> ()); 
    Monetization.Initialize ("1234567", false);
}
```

In this example, `"IAPManager"` references a GameObject containing a script component called `IAPAdapter`, which extends the purchasing adapter. 

## What's next?
Learn how to [implement custom IAP Promo assets](MonetizationNativePromoUnity.md) in your game, or return to the [Monetization](Monetization.md) hub.