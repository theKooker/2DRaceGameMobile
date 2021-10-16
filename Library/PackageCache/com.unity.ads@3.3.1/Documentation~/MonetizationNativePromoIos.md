# Native Promo integration for iOS developers
## Overview
This guide covers integration for implementing a Native Promo adapter in your iOS game, to use custom assets for your [IAP Promos](https://docs.unity3d.com/Manual/IAPPromo.html).

* If you are a Unity developer using C#, [click here](MonetizationNativePromoUnity.md). 
* If you are an Android developer using Java, [click here](MonetizationNativePromoAndroid.md).

## Implementation
### Configuring your game for Unity Ads
To implement a Native Promo adapter, you must integrate Unity Ads in your game. To do so, follow the steps in the [basic ads integration guide](MonetizationBasicIntegrationIos.md) that detail the following:

* [Creating a Project in the Unity developer dashboard](MonetizationBasicIntegrationIos.md#creating-a-project-in-the-unity-developer-dashboard)
* [Importing the Unity Ads framework](MonetizationBasicIntegrationIos.md#importing-the-unity-ads-framework)

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
The [`USRVUnityPurchasingDelegate`](MonetizationResourcesApiIos.md#usrvunitypurchasingdelegate) API provides a Native Promo delegate with methods for handling promotional asset interactions. Use these methods in your game scripts to inform the SDK when the Promo begins, finishes, and initiates the purchase flow. Implement a [`UMONNativePromoAdapter`](MonetizationResourcesApiIos.md#umonnativepromoadapter) delegate with the `promoDidShow`, `promoDidClick`, and `promoDidClose` methods.

Every developer’s Native Promo implementation varies greatly, depending on the nature of their assets. The following abstract sample code illustrates an implementation. 

```
@interface ViewController: UIViewController <USRVUnityPurchasingDelegate>

-(void) showPromo: (UMONPromoAdPlacementContent *) placementContent {
    self.nativePromoAdapter = [[UMONNativePromoAdapter alloc] initWithPromo: placementContent];
    UMONPromoMetaData *metaData = placementContent.metadata;
    UPURProduct *product = metaData.premiumProduct;
    NSString *price = (product == nil || product.localizedPriceString == nil) ? @"$0.99": product.localizedPriceString;
    
    self.nativePromoView.hidden = NO;
    NSString *title = [NSString stringWithFormat: @"Buy for only %@", price];
    [self.purchaseButton setTitle: title forState: UIControlStateNormal];
    [self.nativePromoAdapter promoDidShow];    
}

// If the player clicked the purchase button:
(IBAction) purchaseButtonTapped: (id) sender {
    [self.nativePromoAdapter promoDidClick];
    [self.nativePromoAdapter promoDidClose];
    self.nativePromoView.hidden = YES;
}

// If the player closed the promotional asset:
-(IBAction) promoCloseButtonTapped: (id) sender {
    self.nativePromoView.hidden = YES;
    [self.nativePromoAdapter promoDidClose];
}

- (void) loadProducts: (UnityPurchasingLoadProductsCompletionHandler) completionHandler {
    // Retrieve your Products list (see purchasing integration docs) 
}

- (void) purchaseProduct: (NSString *) productId
     // Insert logic for successful or failed product purchase (see purchasing integration docs) 
}
```

## What's next?
Review additional [resources](MonetizationResources.md) to help get the most out of your monetization, or return to the [Monetization hub](Monetization.md).