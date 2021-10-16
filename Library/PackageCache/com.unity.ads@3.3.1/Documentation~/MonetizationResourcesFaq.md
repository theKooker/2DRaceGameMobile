# Monetization FAQs
### Integration FAQs
#### [Which version of Unity should I be using?](MonetizationBasicIntegration.md)
#### [Why am I not able to see any ads in game?](https://support.unity3d.com/hc/en-us/articles/217262566-Why-don-t-I-see-ads-in-my-game-)
#### [Why am I only seeing 2 or 3 ads at a time?](MonetizationResourcesStatistics.md#fill-rate)
#### [What are some games that currently use Unity Ads?](MonetizationResourcesBestPracticesAds.md#case-studies-and-references)
#### [Can I use ad Placements to promote my other games?](MonetizationCrossPromotions.md)
#### [Is it possible to block certain types of ads from my game?](MonetizationResourcesDashboardGuide.md#ad-content-filters)
#### [Is it possible to block ads from certain developers?](MonetizationResourcesDashboardGuide.md#ad-content-filters)
#### [Can the daily number of available ads be limited?](MonetizationResourcesStatistics.md#fill-rate)
#### [Can Unity Ads be implemented through mediation?](MonetizationResourcesMediation.md)

### Stats FAQs
#### [What do the fields in the monetization stat reports mean?](MonetizationResourcesStatistics.md#understanding-unity-ads-metrics)
#### [Is it possible to automatically generate stat reports?](MonetizationResourcesStatistics.md#configuring-an-automated-report)
#### [How often are monetization stats updated?](MonetizationResourcesStatistics.md#understanding-unity-ads-metrics)

### Revenue FAQs
#### [Why am I not seeing any revenue, even with 100 impressions?](MonetizationResourcesRevenueAndPayment.md#analyzing-revenue)
#### [Is revenue earned by completed views, or based on installs?](MonetizationResourcesBestPracticesAds.md#understanding-how-ads-generate-revenue)
#### [How much money can I expect to make with my game?](MonetizationResourcesRevenueAndPayment.md#monetization-factors)

### Payment FAQs
#### [How does the payment process work?](MonetizationResourcesRevenueAndPayment.md#payment)
#### [Do I need to pay taxes on payments as an individual?](MonetizationResourcesRevenueAndPayment.md#requesting-payment-as-an-individual)
#### [Do I need to provide a VAT number?](MonetizationResourcesRevenueAndPayment.md#tax-information)
#### [When can I expect to be paid?](MonetizationResourcesRevenueAndPayment.md#minimum-payout-amount-and-fulfillment)
#### [My earnings balance was deducted by the amount invoiced, but I have yet to receive payment. What is the reason for this?](MonetizationResourcesRevenueAndPayment.md#minimum-payout-amount-and-fulfillment)

### IAP Promo FAQs
#### [Can I delete Placements in the dashboard?](https://support.unity3d.com/hc/en-us/articles/360000326546-Can-I-delete-Placements-in-the-dashboard-)
#### [How can I cap the frequency of Placements that get called often (e.g. return to lobby)?](https://support.unity3d.com/hc/en-us/articles/360000326526-How-can-I-cap-the-frequency-of-Placements-that-get-called-often-e-g-return-to-lobby-)
#### [How do I set up a limited-time offer?](https://support.unity3d.com/hc/en-us/articles/360000326466-How-do-I-set-up-a-limited-time-offer-)
#### [How are creatives uploaded for different devices and languages handled?](https://support.unity3d.com/hc/en-us/articles/360000326566-How-are-creatives-uploaded-for-different-devices-and-languages-handled-)
#### [Why am I seeing an error when trying to export my IAP Product Catalog to JSON in Unity 2017.3?](https://support.unity3d.com/hc/en-us/articles/360000326303-Why-am-I-seeing-an-error-when-trying-to-export-my-IAP-Product-Catalog-to-JSON-in-Unity-2017-3-)
#### [Why doesn’t the creative I uploaded exactly match what I see on my device?](https://support.unity3d.com/hc/en-us/articles/360000326586-Why-doesn-t-the-creative-I-uploaded-exactly-match-what-I-see-on-my-device-)
#### [Why isn't my Placement showing any Promotions?](https://support.unity3d.com/hc/en-us/articles/360000326446-Why-isn-t-my-Placement-showing-any-Promotions-)

### Personalized Placements FAQs
#### Do Personalized Placements work with mediation?
Yes. To use with mediation, you must integrate your Personalized Placements with your mediation partner’s Unity adapter as you would a normal [Placement](MonetizationPlacements.md). The actual integration will differ by partner. For more information, see documentation on [mediation](MonetizationResourcesMediation.md). 
#### What ad formats do Personalized Placements support?
Personalized Placements support interstitial or rewarded video, playable, and display ads (for more information, see documentation on [Monetization content types](MonetizationContentTypes.md)). 
 
* [Integration guide](MonetizationBasicIntegrationUnity.md) for Unity developers (C#)
* [Integration guide](MonetizationBasicIntegrationIos.md) for iOS developers (Objective-C)
* [Integration guide](MonetizationBasicIntegrationAndroid.md) for Android developers (Java)  

#### How is the control group for my observation phase (phase one) implementation formed?
In the beta, there is an [observation phase](MonetizationPersonalizedPlacementsScale.md#what-to-expect-in-the-observation-phase) to prove revenue lift by randomly selecting 50% of the player base to experience optimized content, while the other 50% act as a control group. If you have questions about the random selection process, or how users are redistributed for the [scaling phase](MonetizationPersonalizedPlacementsScale.md#what-to-expect-in-the-scaling-phase) (phase two) expanded exposure, please contact your Unity representative. 
#### When will I see revenue lift from Personalized Placements?
You should see immediate lift, however the machine learning engine usually takes about two weeks to optimize, depending on how many daily active users play your game. This can vary, depending on where you choose to implement your Placements (for example, at the beginning of the game versus the end of the game), which is why Unity recommends multiple Personalized Placements spread throughout the game to deliver a consistent experience. As with any machine learning model, expect performance to improve over time.  
#### How does Unity determine the optimal content to display?
Personalized Placements optimize toward lifetime value (LTV) for retention and sustainable revenue. Unity calculates the LTV of a player by projecting their total potential value over an infinite amount of time. This model considers many factors beyond bid value. For more information, see documentation on [how Personalized Placements work](MonetizationPersonalizedPlacements.md).

### Authorized Sellers for Apps FAQs
#### What is Authorized Sellers for Apps?
In November 2018, the [IAB](https://www.iab.com/) Technology Laboratory released the Authorized Sellers for Apps (`app-ads.txt`) specifications, an extension of the original [Authorized Digital Sellers](https://iabtechlab.com/ads-txt-about/) (`ads.txt`) standard for protecting web ad inventory. `app-ads.txt` applies this functionality to ads in mobile apps, as well as over-the-top (OTT) video apps, allowing you to increase your pool of authorized digital advertising inventory while reducing fraud. For demand-side platforms (DSPs), it allows buyers to confidently purchase through approved seller accounts.

#### Is this new? How does it differ from ads.txt on web or mobile web?
`app-ads.txt` provides the same functionality as `ads.txt`, but with a pointer from apps. The `ads.txt` file, which lists exchanges and other content owners that are authorized to sell space on a given site, normally resides in the root folder of a website. The `app-ads.txt` file for a given app also often resides on an associated website, with the app referencing that file location.

#### What problem does app-ads.txt attempt to solve?
`app-ads.txt` is intended to protect advertisers from buying invalid or suspicious traffic, and to protect publishers from losing revenue to malicious sources intercepting their demand.

#### What is Unity’s position on app-ads.txt?
Unity strongly supports `app-ads.txt`. While it doesn’t solve all instances of wrongdoing in mobile exchanges, it does create a healthier ecosystem with greater accountability and transparency across the supply chain.

#### What is the publisher benefit of adopting app-ads.txt?
Over time, Unity expects mobile DSPs to enable advertisers to buy authorized inventory differently from unauthorized inventory. As this occurs, advertisers might also expect to pay a premium for this inventory, creating a strong incentive for publishers to adopt `app-ads.txt`.

#### What is the advertiser benefit of adopting app-ads.txt?
As with `ads.txt` on web, advertisers will have greater assurance that they are buying legitimate impressions from authorized re-sellers.

#### As a publisher, how do I adopt app-ads.txt for Unity monetization?
Follow the step-by-step process documented in the [IAB app-ads.txt Publisher Advisory](https://iabtechlab.com/wp-content/uploads/2018/11/IABTechLab_-App-ads.txt_publisher_advisory.pdf). The key steps are detailed below for your convenience:

1. If you haven’t done so already, make sure the `"developer website"` field is current in the store pages hosting your apps. Advertising systems use such websites to retrieve the `app-ads.txt` file.
2. Upload a file named `app-ads.txt` to your website (please see the above specs for full details on file location), containing the list of authorized sellers of your app’s ad inventory per the official guidance. The content of the `app-ads.txt` file follows the same rules as `ads.txt` for web, with the only exception of subdomain directive (see the [spec](https://iabtechlab.com/ads-txt/) for details).
3. Enter the following fields in your hosted `app-ads.txt` file:

```
unity3d.com, [Developer ID], DIRECT, 96cabb5fbdde37a7
telaria.com, rwd19-[Developer ID], RESELLER, 1a4e959a1b50034a
tremorhub.com, rwd19-[Developer ID], RESELLER, 1a4e959a1b50034a
loopme.com, 9621, RESELLER, 6c8d5f95897a5a3b
```

**Note**: The Developer ID can be found in the [Operate Dashboard](https://operate.dashboard.unity3d.com/), by selecting **Invoicing** for the left navigation menu. For example, if your Developer ID is `12345`, the fields would be entered as follows:

```
unity3d.com, 12345, DIRECT, 96cabb5fbdde37a7
telaria.com, rwd19-12345, RESELLER, 1a4e959a1b50034a
tremorhub.com, rwd19-12345, RESELLER, 1a4e959a1b50034a
loopme.com, 9621, RESELLER, 6c8d5f95897a5a3b
```

**Important**: This list may change over time, but updates will always be reflected in this document.

#### Are there additional resources from the IAB that I can refer to?
Yes. For further reading, see the [Implementation and FAQ](https://iabtechlab.com/ads-txt/) resources from the IAB.