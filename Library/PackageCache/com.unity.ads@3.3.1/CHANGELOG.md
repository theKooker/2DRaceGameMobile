# Changelog

## [3.3.1] - 2019-09-26
### Unity (Editor, Asset Store, & Packman)
#### Updates
* No visible changes

## [3.3.0] - 2019-09-26
### Unity (Editor, Asset Store, & Packman)
#### Fixed
* Fixed an issue where callbacks would not be executed on the main thread
* Fixed an issue where calling RemoveListener in a callback would cause a crash
### SDK
### iOS
#### Added
* OS 13 update: 
    * Deprecated UI webview. Due to Apple's changes, Unity Ads no longer supports iOS 7 and 8. 
* Banner optimization:
	* New banner API, featuring the [`UADSBannerView`](../manual/MonetizationResourcesApiIos.html#uadsbannerview) class.
	* The new API supports multiple banners in a single Placement, with flexible positioning.

#### Fixed
* iOS 13 AppSheet crash fix

### Android
#### Added
* Banner optimization:
	* New banner API, featuring the [`BannerView`](../manual/MonetizationResourcesApiAndroid.html#bannerview) class.
	* The new API supports multiple banners in a single Placement, with flexible positioning.

#### Fixed
* WebView onRenderProcessGone crash fix

### Documentation updates
#### Monetization
* Added a FAQ section for [Authorized Sellers for Apps](../Manual/MonetizationResourcesFaq.html#authorized-sellers-for-apps-faqs) (`app-ads.txt`), which is now supported.

#### Advertising
* Added a section on [source bidding](../Manual/AdvertisingCampaignsConfiguration.html#source-bidding).
* Added a section on [app targeting](../Manual/AdvertisingCampaignsConfiguration.html#app-targeting).	
* Removed legacy dashboard and API guides, which are no longer supported.

#### Programmatic
* Added Open Measurement (OM) support fields, including:
    * [`source.omidpn`](../manual/ProgrammaticBidRequests.html#source-objects)
    * [`source.omidpv`](../manual/ProgrammaticBidRequests.html#source-objects)
    * [`imp.video.api`](../manual/ProgrammaticBidRequests.html#video-objects)
    * [`bid.api`](../manual/ProgrammaticBidResponses.html#bid-objects)
* Added [`app.publisher`](../manual/ProgrammaticBidRequests.html#app-objects) field.
* Added the [`bAge`](../manual/ProgrammaticOptimizationContextualData.html) (blocked age rating) field.

#### Legal
* Updated [GDPR compliance](../manual/LegalGdpr.html) to reflect Unity's opt-in approach to consent.

## [3.2.0] - 2019-07-22
### Unity (Editor, Asset Store, & Packman)
#### Added
* Added OMID viewability integration. Unity is now [IAB certified with VAST viewability](https://iabtechlab.com/blog/vast-4-1-open-measurement-the-long-awaited-video-verification-solution/).

#### Fixed
* In cases where you've installed both the package manager and Asset store versions of Unity Ads, the SDK now surfaces an error notifying you to remove one instance.
* Fixed an Android java proxy usage issue for Unity versions below 2017. This fixes a multiple listeners crash. 

### iOS
#### Added
* Added OMID viewability integration. Unity is now [IAB certified with VAST viewability](https://iabtechlab.com/blog/vast-4-1-open-measurement-the-long-awaited-video-verification-solution/). 

### Android
#### Added
* Added OMID viewability integration. Unity is now [IAB certified with VAST viewability](https://iabtechlab.com/blog/vast-4-1-open-measurement-the-long-awaited-video-verification-solution/). 

## [3.1.1] - 2019-05-16
#### Added
* Updated the Android and iOS binaries to 3.1.0.
* Support for multiple listeners.
* `ASWebAuthenticationSession` support.

#### Fixed
* Banner memory leak.
* `GetDeviceId` on Android SDK versions below 23.
* Volume change event not properly captured on iOS.
* `USRVStorage` JSON exception caught and handled.
* Analytics `onLevelUp` taking a string instead of an integer.
* Crash prevented in the `AdUnitActivity.onPause` event.
* Playstation and Xbox no longer throw errors attempting to access `UnityAdsSettings` when building a Project that includes ads on other platforms.
* Test mode resources folder moved to Editor-only scope.

## [3.0.3] - 2019-03-15
#### Added
* Updated the Android and iOS binaries.

#### Fixed
* https://fogbugz.unity3d.com/f/cases/1115398/
* Uncaught exception for purchasing integration on iOS.

## [3.0.2] - 2019-02-26
#### Added
* Updated the Android and iOS binaries.

#### Fixed
* https://fogbugz.unity3d.com/f/cases/1127423/
* https://fogbugz.unity3d.com/f/cases/1127770/

## [3.0.1] - 2019-01-25
#### Added
* Integrated the Ads 3.0.1 SDK.

## [2.3.2] - 2018-11-21
#### Added
* Integrated the Ads 2.3.0 SDK with Unity 2019.X.

#### Fixed
* https://fogbugz.unity3d.com/f/cases/1107128/
* https://fogbugz.unity3d.com/f/cases/1108663/

## [2.3.1] - 2018-11-15
#### Added
* Updated to Ads 2.3.0 SDK.
* Multithreaded Request API.
* `SendEvent` API for Ads and IAP SDK communication.
* New Unity integration.

## [2.2.1] - 2017-04-192
#### Fixed
* Fixed issues for iOS and Android.

## [2.2.0] - 2017-03-22
#### Added
* IAP Promotion support (iOS, Android).

#### Fixed
* Several rare crashes (iOS).

#### Changed
* Improved cache handling (iOS, Android).
* Increased flexibility showing different ad formats (iOS, Android).
