# Native Promo integration for Unity developers
## Overview
This guide covers integration for implementing a Native Promo adapter in your made-with-Unity game, to use custom assets for your [IAP Promos](https://docs.unity3d.com/2018.1/Documentation/Manual/IAPPromo.html).

* If you are an iOS developer using Objective-C, [click here](MonetizationNativePromoIos.md). 
* If you are an Android developer using Java, [click here](MonetizationNativePromoAndroid.md).

## Implementation
### Configuring your Project for Unity Ads
To implement a Native Promo adapter, you must integrate Unity Ads in your Project. To do so, follow the steps in the [basic ads integration guide](MonetizationBasicIntegrationUnity.md) that detail the following:

* [Setting build targets](MonetizationBasicIntegrationUnity.md#setting-build-targets)
* [Installing Unity Ads](MonetizationBasicIntegrationUnity.md#installing-unity-ads)

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
The [`Monetization`](../api/UnityEngine.Monetization.html) API provides a Native Promo interface with methods for handling promotional asset interactions. Use these methods in your game scripts to inform the SDK when the Promo begins, finishes, and initiates the purchase flow. Include the `UnityEngine.Monetization` namespace in your script header, then implement an [`INativePromoAdapter`](..api/UnityEngine.Monetization.INativePromoAdapter.html) interface with the `OnShow`, `OnClicked`, and `OnClosed` methods.

Every developer’s Native Promo implementation varies greatly, depending on the nature of their assets. The following abstract sample code illustrates an implementation. 

```
using UnityEngine.Monetization;

public class NativePromoDisplay : MonoBehaviour {    

    PlacementContent placementContent = Monetization.GetPlacementContent (placementId);

    PromoAdPlacementContent promoContent = placementContent as PromoAdPlacementContent;

    INativePromoAdapter adapter = Monetization.CreateNativePromoAdapter (promoContent);

    void ShowPromo () {

        LogPromoInfo ();

        // Use promoContent’s associated Product ID (e.g. adapter.metadata.premiumProduct.productID) to determine which assets to show

        // Call adapter.OnShown () to tell the SDK the Promo has started, then execute your custom display for those assets

        // Call adapter.OnClicked () to tell the SDK the player clicked the purchase button and to initiate the purchase flow

        // Call adapter.OnClosed () to tell the SDK the Promo has ended  	

    }

    void LogPromoInfo () {

        Debug.LogFormat ("Product ID: \t{0}", adapter.metadata.premiumProduct.productId);

        Debug.LogFormat ("Localized Title: \t{0}", adapter.metadata.premiumProduct.localizedTitle);

        Debug.LogFormat ("Localized Description: \t{0}", adapter.metadata.premiumProduct.localizedDescription);

        Debug.LogFormat ("ISO Currency Code: \t{0}", adapter.metadata.premiumProduct.isoCurrencyCode);

        Debug.LogFormat ("Localized Price: \t{0}", adapter.metadata.premiumProduct.localizedPrice);

        Debug.LogFormat ("Localized Price String: \t{0}", adapter.metadata.premiumProduct.localizedPriceString);

    }
}
```

## What's next?
Review additional [resources](MonetizationResources.md) to help get the most out of your monetization, or return to the [Monetization hub](Monetization.md).