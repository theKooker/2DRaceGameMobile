# Mediation guide
## Integrating Unity Ads in a mediation stack
While Unity Ads delivers the most value through [Unified Auction](https://unity.com/solutions/unity-ads), it also seamlessly integrates with mediation solutions to fit your workflow.

Your mediation partner dictates the integration process for Unity Ads. You can find integration guides and SDK adapters for the following widely used mediation partners below:

* [MoPub](https://developers.mopub.com/docs/mediation/networks/unityads/)
* [AdMob](https://developers.google.com/admob/unity/mediation/unity)
* [IronSource](https://developers.ironsrc.com/ironsource-mobile/unity/unityads-mediation-guide/#step-1)
* [Fyber](https://unity.fyber.com/docs/)

### Using IAP Promo in mediation
If you use mediated ads with [Unity IAP](https://docs.unity3d.com/Manual/UnityIAP.html) and wish to use [IAP Promos](https://docs.unity3d.com/Manual/IAPPromo.html) or [Personalized Placements](MonetizationPersonalizedPlacementsUnity.md), you will need an adaptor that allows the Unity Ads SDK to receive purchase and conversion events while in mediation. 

#### Installation
Download the [Unity Ads Mediation Purchasing Adapter](https://assetstore.unity.com/packages/add-ons/services/unity-ads-mediation-purchasing-adaptor-151050) from the Asset store, then install it to your Project in the Unity Editor. For more information, see documentation on [Asset packages](https://docs.unity3d.com/Manual/AssetPackages.html).

This package provides a purchase adapter for Unity developers that connects Unity Ads and Unity IAP to enable purchasing to work in mediation.

#### Requirements
The Promo Mediation Adapter requires the following:

* Your game is built in Unity, using version 2017.1 or higher.
* Your game uses Unity Ads in a mediation stack (verified compatible with AdMob, MoPub, and ironSource; see section on **Compatibility**, below).
* Your game uses Unity IAP version 1.16 or higher.

#### Using the adapter
Integrate Unity Ads as specified by your mediation partner, but initialize the SDK with the adapter’s new method signature:

`public static void Initialize (IStoreListener listener, ConfigurationBuilder builder)`

To use this method, you must implement an [`IStoreListener`](https://docs.unity3d.com/2017.3/Documentation/ScriptReference/Purchasing.IStoreListener.html) interface, and create a [`ConfigurationBuilder`](https://docs.unity3d.com/2017.3/Documentation/ScriptReference/Purchasing.ConfigurationBuilder.html) object. Both are requirements for Unity IAP, and covered in the [Unity IAP initialization documentation](https://docs.unity3d.com/Manual/UnityIAP.html).

#### Compatibility
The Promo Mediation Adapter is verified as compatible with AdMob, MoPub, and ironSource. However, as ironSource repackages Unity’s native binaries and renames the files, ironSource customers building to iOS must follow these additional steps:

1. Open the _UnityAdsPurchasingWrapper.mm_ file via your Project’s file folder, or the Unity Editor’s **Project** window (_Plugins/UnityAdsMediationAdapter/UnityAdsPurchaseWrapper.mm_).
2. Edit the top two imports as follows:
    * Change `#import "UnityAds/UADSPurchasing.h"` to `#import <ISUnityAdsAdaptor/UASDPurchasing.h>.`
    * Change `#import "UnityAds/UADSMetaData.h"` to `#import <ISUnityAdsAdaptor/UADSMetaData.h>`. 

#### Known issues
Importing the Unity Ads C# code in your Project will result in duplicate definition errors. Any use of this adapter in a custom manner is not supported, and could result in errors or bugs in your integration.

## Frequently asked questions
#### Does Unity Ads integrate with all mediation vendors?
Unity Ads integrates with most trusted mediation partners. Unity highly recommends using one of the partners listed above to ensure success, as they offer full integration guides specific to Unity. If you choose a different partner, Unity recommends confirming that they have integration resources to ensure compatibility with Unity Ads. 

#### Are there any performance issues to consider when selecting a mediation vendor?
Unity recommends integrating with an open source mediation partner that offers customizable SDK adapters. This ensures that the mediator and the ad source (Unity) have minimal data discrepancies and drive better results. 

#### Do I need the Unity Ads SDK in order to run Unity Ads in mediation?
Yes. For your mediation partner to call Unity’s network, you need to install the Unity Monetization SDK. For more information on how to do this, see documentation on [basic integration](MonetizationBasicIntegration.md) with Unity Monetization. You can download and import the latest SDK for your platform here:

* [Unity (C#)](https://assetstore.unity.com/packages/add-ons/services/unity-ads-66123)
* [iOS (Objective-C)](https://github.com/Unity-Technologies/unity-ads-ios/releases)
* [Android (Java)](https://github.com/Unity-Technologies/unity-ads-android/releases)

#### Can I run Unity Ads in parallel with my mediation stack?
You can run Unity ads separately from your mediation stack. If your game is made with Unity, you can download the Asset package from the [Unity Asset Store](https://assetstore.unity.com/packages/add-ons/services/unity-ads-66123). If your game is not made with Unity, you can download the SDK for [iOS](https://github.com/Unity-Technologies/unity-ads-ios/releases) or [Android](https://github.com/Unity-Technologies/unity-ads-android/releases) and access all the same features. 

#### If I run Unity Ads through a mediation partner, can I still leverage all of the Unity Monetization SDK features?
Nearly all of the Monetization SDK features are compatible with mediation. However, its most advanced monetization feature, [Personalized Placements](https://unity.com/solutions/mobile-business/monetize-your-game/personalized-placements), performs best when all advertising touchpoints run through Unity. This is because Personalized Placements combine your ads and in-app purchasing revenue systems by making automated decisions about which to serve, based on Unity’s machine learning algorithm’s highest predictive player LTV.    

#### Can I place Unity Ads in multiple positions of my mediation stack?
Yes. You can integrate Unity Ads into multiple price tiers of your waterfall.

#### Are my earnings based on numbers reported by Unity or my mediation partner?
Any earnings generated through Unity Ads are based on Unity’s reported billing numbers. While there should not be significant differences between Unity’s numbers and a mediator’s numbers, using an open source mediation partner will help ensure the two sources match as closely as possible.

## What's next?
View documentation on [rewarded ads best practices](MonetizationResourcesBestPracticesAds.md) to get the most out of your implementation.