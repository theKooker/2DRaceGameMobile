# Native Promo integration for iOS developers
## Overview
This guide covers integration for implementing a Native Promo adapter in your Android game, to use custom assets for your [IAP Promos](https://docs.unity3d.com/Manual/IAPPromo.html).

* If you are a Unity developer using C#, [click here](MonetizationNativePromoUnity.md). 
* If you are an iOS developer using Objective-C, [click here](MonetizationNativePromoIos.md).

## Implementation
### Configuring your game for Unity Ads
To implement a Native Promo adapter, you must integrate Unity Ads in your game. To do so, follow the steps in the [basic ads integration guide](MonetizationBasicIntegrationAndroid.md) that detail the following:

* [Creating a Project in the Unity developer dashboard](MonetizationBasicIntegrationAndroid.md#creating-a-project-in-the-unity-developer-dashboard)
* [Importing the Unity Ads framework](MonetizationBasicIntegrationAndroid.md#importing-the-unity-ads-framework)

**Important**: Native Promo requires the Unity Ads SDK version 3.0 or higher.

Once your Project is configured for Unity Ads, proceed to configuring IAP Promos.

### Configuring IAP Promos
Before changing your code, follow the [guide](https://docs.unity3d.com/Manual/IAPPromo.html) for setting up IAP Promos. If you haven't done so already, you will need to: 

* Create IAP-enabled [Placements](MonetizationPlacements.md)
* Create or import a [Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html)
* Create [Promos](https://docs.unity3d.com/Manual/IAPPromoPromotions.html) 

**Note**: When configuring your Products in the developer dashboard, do not include associated creative assets, as you will provide custom assets through the adapter.

Once your IAP Promos are configured, proceed to implementing the adapter in your code.

### Implementing a Native Promo adapter
The [`UnityPurchasing`](MonetizationResourcesApiAndroid.md#unitypurchasing) API provides a Native Promo delegate with methods for handling promotional asset interactions. Use these methods in your game scripts to inform the SDK when the Promo begins, finishes, and initiates the purchase flow. Implement a [`NativePromoAdapter`](MonetizationResourcesApiAndroid.md#nativepromoadapter) delegate with the `onShown`, `onClicked`, and `onClosed` methods.

Every developer’s Native Promo implementation varies greatly, depending on the nature of their assets. The following abstract sample code illustrates an implementation. 


```
UnityPurchasing.setAdapter (new UnityPurchasingAdapter ());

private class UnityPurchasingAdapter implements IPurchasingAdapter {

    @Override
    public void retrieveProducts (IRetrieveProductsListener listener) {
        // Retrieve your Products list (see purchasing integration docs)    
    }

    @Override
    public void onPurchase (String productID, ITransactionListener listener, Map<String, Object> extras) {
        // Insert logic for successful or failed product purchase (see purchasing integration docs)
    }

    private void showPromo (final PromoAdPlacementContent placementContent) {
        final NativePromoAdapter nativePromoAdapter = new NativePromoAdapter (placementContent);

        PromoMetadata metadata = placementContent.getMetadata ();
        Product product = metadata.getPremiumProduct ();
        String price = product == null ? "$0.99": product.getLocalizedPriceString ();

        final View root = getLayoutInflater ().inflate (R.layout.unitymonetization_native_promo, (ViewGroup) findViewById (R.id.unityads_example_layout_root));

        Button buyButton = root.findViewById(R.id.native_promo_buy_button);
        Button closeButton = root.findViewById (R.id.native_promo_close_button);
        buyButton.setText ("Buy now for only " + price + "!");

        nativePromoAdapter.onShown();
        buyButton.setOnClickListener(new View.OnClickListener() {
        @Override
        public void onClick (View v) {
            // Do purchase then call
            nativePromoAdapter.onClosed ();
            ((ViewGroup)root).removeView (findViewById (R.id.native_promo_root));
        }
    });

    closeButton.setOnClickListener (new View.OnClickListener () {
        @Override
        public void onClick(View v) {
            nativePromoAdapter.onClosed ();
            ((ViewGroup)root).removeView (findViewById (R.id.native_promo_root));
        }
    });
}
```

## What's next?
Review additional [resources](MonetizationResources.md) to help get the most out of your monetization, or return to the [Monetization hub](Monetization.md).