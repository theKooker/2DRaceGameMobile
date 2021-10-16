# Custom purchasing integration for Android developers
## Overview
This guide covers integration for implementing a purchasing adapter in your Android game, to take advantage of Unity's optimization features.

* If you are a Unity developer using C#, [click here](MonetizationPurchasingIntegrationUnity.md) 
* If you are an iOS developer using Objective-C, [click here](MonetizationPurchasingIntegrationIos.md).

### Guide contents
1. [Configure your game](#configuring-your-game-for-unity-ads) for Unity Ads.
2. [Configure your Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html) on the [developer dashboard](https://operate.dashboard.unity3d.com). 
3. [Configure an IAP Promo](https://docs.unity3d.com/Manual/IAPPromoPromotions.html) on the developer dashboard. 
4. Implement a purchasing adapter in your game code, using the [`UnityPurchasing`](MonetizationResourcesApiAndroid.md#unitypurchasing) API.
5. [Implement game logic and purchase events](#implementing-the-purchasing-adapter) within the purchasing adapter.
6. [Initialize](#initializing-the-purchasing-adapter-and-sdk) the purchasing adapter and Ads SDK.

## Implementation
### Configuring your game for Unity Ads
To implement a purchasing delegate, you must integrate Unity Ads in your game. To do so, follow the steps in the [basic ads integration guide](MonetizationBasicIntegrationIos.md) that detail the following:

* [Creating a Project in the Unity developer dashboard](MonetizationBasicIntegrationAndroid.md#creating-a-project-in-the-unity-developer-dashboard)
* [Importing the Unity Ads framework](MonetizationBasicIntegrationAndroid.md#importing-the-unity-ads-framework)

**Important**: Custom purchase integration requires the Unity Ads SDK version 3.0 or higher.

Once your game is configured for Unity Ads, proceed to creating a Product Catalog.

### Configuring Product Catalogs on the developer dashboard
Before implementing your purchasing delegate, navigate to the Operate tab of the [developer dashboard](https://operate.dashboard.unity3d.com), then follow the manual configuration instructions for [populating a Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html).  

### Configuring Promotions on the developer dashboard
From the Operate tab of the [developer dashboard](https://operate.dashboard.unity3d.com), follow the instructions for [configuring an IAP Promo](https://docs.unity3d.com/Manual/IAPPromo.html).  

### Implementing the purchasing adapter
In your game script, include the `com.unity3d.services.purchasing.core.IPurchasingAdapter` interface, then create a class that implements a purchasing adapter. You will use two functions, `retrieveProducts` and `onPurchase`, to define the game logic you want the purchasing adapter to use. These methods require a class using [`IPurchasingAdapter`](MonetizationResourcesApiAndroid.md#ipurchasingadapter). You must implement them so the SDK can call them as needed when managing your Product transactions.

```
import com.unity3d.services.purchasing.core.IPurchasingAdapter;

private class UnityPurchasingAdapter implements IPurchasingAdapter {
    // Placeholder for retrieveProducts function
    // Placeholder for onPurchase function
}
```

#### Retrieving your Product Catalog
The SDK calls `retrieveProducts` to retrieve the list of available Products. The function must convert your IAP products into [`UnityMonetization.Product`](MonetizationResourcesApiAndroid.md#product) objects. This requires a minimum of the following for each Product:

* `productID`
* `localizedPriceString`
* `productType`
* `isoCurrencyCode`
* `localizedTitle` 

The function takes an [`IRetrieveProductsListener`](MonetizationResourcesApiAndroid.md#iretrieveproductslistener) listener. Call the listener’s `onProductsRetrieved` function to send the retrieved Products list back to the SDK. 

#### Processing a purchase
The SDK calls `onPurchase` when a user clicks the buy button for a promotional asset. The purchase’s success or failure handling depends on your in-app purchasing implementation. 

If the transaction succeeds, call the listener’s `onTransactionComplete` function using a [`TransactionDetails`](MonetizationResourcesApiAndroid.md#transactiondetails) object.  

If the transaction fails, call the listener’s `onTransactionError` function, using a [`TransactionError`](MonetizationResourcesApiAndroid.md#transactionerror) object.

#### Example IPurchasingAdapter implementation
```
import com.unity3d.services.purchasing.core.IPurchasingAdapter;

private class UnityPurchasingAdapter implements IPurchasingAdapter {
    @Override
    public void retrieveProducts (IRetrieveProductsListener listener) {

        // Query your Products here, convert them to Monetization.Products, then populate the Product list with them:
        listener.onProductsRetrieved (Arrays.asList (Product.newBuilder ()
            .withProductId ("100bronzeCoins")
            .withLocalizedTitle ("100 Bronze Coins")
            .withLocalizedPriceString ("$1.99")
            .withIsoCurrencyCode ("USD")
            .withProductType ("Consumable")
            .build ()));
    }

    @Override
    public void onPurchase (String productID, ITransactionListener listener, Map<String, Object> extras) {
        thirdPartyPurchasing.purchase (productId); // Generic third-party purchasing function
        
        // If purchase succeeds:
        listener.onTransactionComplete (TransactionDetails.newBuilder ()
                .withTransactionId ("ABCDE")
                .withReceipt ("Parsed receipt string here")
                .build ());

        // If purchase fails:
        listener.onTransactionError (TransactionErrorDetails.newBuilder ()
            .withTransactionError (TransactionError.NETWORK_ERROR)
            .withExceptionMessage ("Example: network connection dropped")
            .withStore (Store.GOOGLE_PLAY)
            .withStoreSpecificErrorCode ("Example: Google Play lost network connection")
            .build ());
        }
    }
}
```

#### Initializing the purchasing adapter and SDK
After you’ve implemented the purchasing adapter, you must provide a reference to it using the [`setAdapter`](MonetizationResourcesApiAndroid.md#setadapter) method. Finally, you must [initialize](MonetizationBasicIntegratioAndroid.md#initialization) the SDK using your Project’s Game ID for the appropriate platform. You can locate the ID on the **Operate** tab of the [Developer Dashboard](https://operate.dashboard.unity3d.com/) by selecting the Project, then selecting **Settings** > **Project Settings** from the left navigation bar (see the [Dashboard guide](MonetizationResourcesDashboardGuide.md#project-settings) section on **Project Settings** for details). 

To avoid errors, implement these calls as early as possible in your game’s run-time life cycle. For example:

```
@Override
protected void onCreate (Bundle savedInstanceState) {
    super.onCreate (savedInstanceState);
    setContentView (R.layout.activity_main);

     UnityPurchasing.setAdapter (new UnityPurchasingAdapter ());

     UnityServices.initialize (this, unityGameID, this);
}
```

## What's next?
Learn how to [implement custom IAP Promo assets](MonetizationNativePromoAndroid.md) in your game, or return to the [Monetization](Monetization.md) hub.