/*
Title: Getting started
Sort: 1
*/


Created by the leading mobile game engine, the Unity Ads SDK provides a comprehensive monetization framework for your game, whether you develop in Unity, xCode, or Android Studio. It streamlines ads, in-app purchasing and analytics, using cutting edge machine learning to maximize your revenue while maintaining a great player experience. Getting started is simple.

This page illustrates the journey towards maximizing your revenue with Unity Ads. Follow step by step, or jump to the integration step that best fits your stage of development.

## Preparation
New to Unity Ads? Take care of these basics before implementation:

1. Download and import the latest Unity Ads Asset package or SDK: [Unity (C#)](https://assetstore.unity.com/packages/add-ons/services/unity-ads-66123) | [iOS (Objective-C)](https://github.com/Unity-Technologies/unity-ads-ios) | [Android (Java)](https://github.com/Unity-Technologies/unity-ads-android/releases)
2. [Register](https://id.unity.com/) a Unity Developer (UDN) account on the Developer Dashboard.
3. Use the [Developer Dashboard](https://operate.dashboard.unity3d.com) to [configure Placements](https://unityads.unity3d.com/help/monetization/placements) for monetization content. 
4. Review our [Best practices guide](https://unityads.unity3d.com/help/resources/best-practices), complete with case studies, to better understand your monetization strategy before diving in.

## Implementation
Integration may vary, depending on your development platform.

1. Integrate Unity Ads using the Monetization API: [Unity (C#)](https://unityads.unity3d.com/help/unity/integration-guide-unity#basic-implementation) | [iOS (Objective-C)](https://unityads.unity3d.com/help/ios/integration-guide-ios#basic-implementation) | [Android (Java)](https://unityads.unity3d.com/help/android/integration-guide-android#basic-implementation)
2. Expand your basic ads integration: 
  * Reward players for watching ads: [Unity (C#)](https://unityads.unity3d.com/help/unity/integration-guide-unity#implementing-rewarded-ads) | [iOS (Objective-C)](https://unityads.unity3d.com/help/ios/integration-guide-ios#implementing-rewarded-ads) | [Android (Java)](https://unityads.unity3d.com/help/android/integration-guide-android#implementing-rewarded-ads)
  * Incorporate banner ads: [Unity (C#)](https://unityads.unity3d.com/help/unity/integration-guide-unity#implementing-banner-ads) | [iOS (Objective-C)](https://unityads.unity3d.com/help/ios/integration-guide-ios#implementing-banner-ads) | [Android (Java)](https://unityads.unity3d.com/help/android/integration-guide-android#implementing-banner-ads)
  * Incorporate [Augmented Reality](https://unityads.unity3d.com/help/beta/ar-ads) (AR) ads.
3. If your game uses in-app purchasing, integrate [IAP Promo](https://docs.unity3d.com/2018.1/Documentation/Manual/IAPPromo.html).
  * If you use Unity IAP: [Unity (C#) ](https://docs.unity3d.com/2018.1/Documentation/Manual/IAPPromoIntegration.html)
  * If you use a custom IAP implementation: [Unity (C#)](https://unityads.unity3d.com/help/unity/purchasing-integration-unity) | [iOS (Objective-C)](https://unityads.unity3d.com/help/ios/purchasing-integration-ios) | [Android (Java)](https://unityads.unity3d.com/help/android/purchasing-integration-android)
  * (Optional) Customize your Promos with [Native Promo](https://unityads.unity3d.com/help/beta/native-promo) assets.
4. Convert your Ads and Promo Placements into [Personalized Placements](https://unityads.unity3d.com/help/monetization/placements#personalized-placements) to take advantage of Unity's revenue optimization features.

![Flow chart for optimizing revenue](https://github.com/Applifier/unity-ads/wiki/monetization/OptimizingRevenue.png)

## Manage, analyze, optimize
Beyond implementation, Unity empowers you to fine-tune your strategy:

* The [Unity Developer Dashboard](https://operate.dashboard.unity3d.com/) allows you to [manage your ads implementation](https://unityads.unity3d.com/help/resources/dashboard-guide). Use the dashboard's [robust metrics tools](https://unityads.unity3d.com/help/resources/statistics) to adopt a data-driven approach to fine-tuning your monetization strategy.
* Learn how to [filter your ads](https://unityads.unity3d.com/help/resources/dashboard-guide#ad-content-filters) to target your audience.
* Don’t miss out on revenue; be sure to [add your Store Details](https://unityads.unity3d.com/help/resources/dashboard-guide#platforms) once your game is live.
* Implement Standard Events to help Unity’s machine learning model optimize monetization based on your game’s economy. [Unity (C#)](https://docs.unity3d.com/2018.1/Documentation/Manual/UnityAnalyticsStandardEvents.html) | [iOS (Objective-C)](https://unityads.unity3d.com/help/ios/purchasing-integration-ios#unityanalytics) | [Android (Java)](https://unityads.unity3d.com/help/android/purchasing-integration-android#unityanalytics)
* Sign up for [automated payouts](https://unityads.unity3d.com/help/resources/revenue-and-payment#automated-payouts).

## Support
Have questions? We're here to help! The following resources can assist in addressing your issue:

* Browse the Unity Ads [community forums](https://forum.unity.com/forums/unity-ads.67/).
* Search the Unity Ads [monetization Knowledge Base](https://support.unity3d.com/hc/en-us/sections/201163835-Ads-Publishers).
* [Contact Unity Ads support](mailto:unityads-support@unity3d.com) with your inquiry.