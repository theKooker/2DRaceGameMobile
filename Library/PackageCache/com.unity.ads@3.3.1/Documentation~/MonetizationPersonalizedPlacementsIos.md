# Personalized Placements integration for iOS developers
## Overview
This guide covers implementation for Personalized Placements in your iOS game.

* If you are a Unity developer using C#, [click here](MonetizationPersonalizedPlacementsUnity.md). 
* If you are an Android developer using Java, [click here](MonetizationPersonalizedPlacementsAndroid.md). 

### Guide contents
* [Import the Unity Monetization framework](#importing-the-unity-monetization-framework) 
* [Implement Unity Ads](#implementing-unity-ads) 
  * (Optional) Integrate mediation adapters 
* [Implement in-app purchasing](#implementing-in-app-purchasing) (IAP) 
  * (Optional) Implement a custom purchasing adapter 
* [Implement Standard Event tracking](#implementing-standard-events) 
* [Configure Personalized Placements](#configuring-personalized-placements) 
  * (Optional) Add Personalized Placements to your mediation integration  
  * Contact Unity to activate LTV optimization 
* [Configure IAP Promos in the Developer Dashboard](#configuring-iap-promos)

## Implementation
### Importing the Unity Monetization framework 
To get started, [download](https://github.com/Unity-Technologies/unity-ads-ios/releases) the latest version of the Monetization framework for iOS. Personalized Placements require the Monetization SDK 3.0 or later. For information on downloading and importing the SDK, read that section of the [basic ads integration guide](MonetizationBasicIntegrationIos.md#integration-for-personalized-placements) for iOS. 

### Implementing Unity Ads 
To implement rewarded or non-rewarded ads in your Unity Project, you must initialize the Unity Monetization SDK, then implement scripts at your surfacing points that access and display content through Placement IDs. For a comprehensive guide, read the [ads integration guide](MonetizationBasicIntegrationIos.md#integration-for-personalized-placements) for iOS.

**Note**: The Unity Ads integration for Personalized Placements differs slightly from the basic ads integration, as it requires the [`UnityMonetization`](MonetizationResourcesApiIos.md#unitymonetization) API instead of the `UnityAds` API.  

Unity also recommends reviewing the [rewarded ads best practices guide](MonetizationResourcesBestPracticesAds.md) as part of your implementation strategy.  

#### Using mediation
If you use ads mediation, the integration process for Unity Ads differs as dictated by your mediation partner. You can find integration guides and SDK adapters for the following widely-used mediation partners below:
 
* [MoPub](https://developers.mopub.com/docs/mediation/networks/unityads/) 
* [AdMob](https://developers.google.com/admob/unity/mediation/unity) 

For more details, see documentation on [Unity Ads mediation](MonetizationResourcesMediation.md). If you have questions regarding compatibility with other mediators, please contact your Unity representative. 

### Implementing in-app purchasing 
The Monetization SDK allows you to take advantage of Personalized Placements using a custom purchasing integration, by implementing a purchasing adapter. This adapter is required for Unity to hook into the necessary machine learning data. For a complete guide on implementing a purchasing adapter, see documentation on [purchasing integration](MonetizationPurchasingIntegrationIos.md) for iOS.  

### Implementing Standard Events 
[Standard Events](https://docs.unity3d.com/Manual/UnityAnalyticsStandardEvents.html) track user experience and player behavior across your game. Implement a subset of game economy events so that Unity’s machine learning engine can optimize your Placement content. Game economy events help the Personalized Placements data model form a complete picture of each player’s preferences and actions in your game, then determine what actions to take accordingly. 

The Monetization SDK allows you to use Unity Analytics APIs in iOS games that are not made with Unity. To implement these events in your game and fire the event at the user level to Unity’s endpoint, invoke the `UnityAnalytics` interface: 

```
@interface UnityAnalytics: NSObject
``` 

#### Logging acquire and spend events  
The following events help train Personalized Placements to know when it’s best to show a Promo. 

Call the `onItemAcquired` event when players acquire a resource in-game. 

```
+ (void) onItemAcquired: (NSString *) transactionId itemId: (NSString *) itemId transactionContext: (NSString *) transactionContext level: (NSString *) level itemType: (NSString *) itemType amount: (float) amount balance: (float) balance acquisitionType: (UnityAnalyticsAcquisitionType) acquisitionType;      
```

Call the `onItemSpent` event when players consume a resource in-game. 

```
+ (void) onItemSpent: (NSString *) transactionId itemId: (NSString *) itemId transactionContext: (NSString *) transactionContext level: (NSString *) level itemType: (NSString *) itemType amount: (float) amount balance: (float) balance acquisitionType: (UnityAnalyticsAcquisitionType) acquisitionType;     
```

| **Parameter** | **Data type** | **Description** | **Examples** |
| ------------- | ------------- | --------------- | ------------ | 
| `transactionId` | `NSString` | (Optional) A unique identifier for the specific transaction that occurred. You can use this to group multiple events into a single transaction.<br><br>**Note**: If the `acquisitionType` is premium (meaning the player purchased the commodity with real money), Unity recommends matching the transaction ID logged at the point of purchase with the one sent for `onItemAcquired` or `onItemSpent`, so the data model can easily associate the two as part of a single transaction. For more information, see documentation on [purchasing integration](MonetizationPurchasingIntegrationIos.md) for iOS. | <ul><li>`@"00001"`</li><li>`@"12345”`</li></ul> |
| `itemId` | `NSString` | (Required) A name or unique identifier for the spent or acquired item. | <ul><li>`itemId: @"Mana Potion"`</li><li>`itemId: @"Hay Bales"`</li><li>`itemId: @"Tokens"`</li></ul> |
| `transactionContext` | `NSString` | (Required) The channel through which the item was spent or acquired. | <ul><li>`transactionContext: @"IAP Store"`</li><li>`transactionContext: @"Shop"`</li><li>`transactionContext: @"Crafting"`</li></ul> |
| `level` | `NSString` | (Optional) The name or identifier of the player’s progression level when the item was spent or acquired. This value will be expressed differently depending on the game (for example, as a numerical level, as a title, or as progress on a map). | A game with traditional player levels:<br><br><ul><li>`level: @"13"`</li><li>`level: @"72"`</li></ul><br>A game that gauges progression with titles:<br><br><ul><li>`level: @"Novice"`</li><li>`level: @"Grand Champion"`</li></ul><br>A game without player levels, that gauges progression through advancing to levels or maps:<br><br><ul><li>`level: @"Pirate Bay"`</li><li>`level: @"Elysian Fields"`</li></ul> |
| `itemType` | `NSString` | (Optional) The category of the spent or acquired item. Unity recommends using one of the following categories for proper data modeling:<br><br><ul><li>`"Currency"`</li><li>`"Resource"`</li><li>`"Item"`</li><li>`"Other"`</li></ul> | <ul><li>`itemType: @"Currency"`</li><li>`itemType: @"Resource"`</li><li>`itemType: @"Item"`</li></ul> |
| `amount` | `NSFloat` | (Required) The unit quantity of the spent or acquired item. | <ul><li>`amount: 1.0F`</li><li>`amount: 200.0F`</li><li>`amount: 5000.0F`</li></ul> |
| `balance` | `NSFloat` | (Optional) The player’s new (post-transaction) quantity of the spent or acquired item. |  <ul><li>`amount: 10.F`</li><li>`amount: 50.0F`</li><li>`amount: 120.0F`</li></ul> |
| `acquisitionType` | `UnityAnalyticsAcquisitionType` | (Required) The `UnityAnalyticsAcquisitionType` can be either:<br><br><ul><li>`kUnityAnalyticsAcquisitionTypePremium` indicates the item was purchased with real currency.<li><li>`kUnityAnalyticsAcquisitionTypeSoft` indicates the item was acquired with virtual currency or resources.</li></ul> | <ul><li>`acquisitionType: kUnityAnalyticsAcquisitionTypePremium`</li><li>`acquisitionType: kUnityAnalyticsAcquisitionTypeSoft`</li></ul> |

Consider the following examples: 

| **Use case** | **Event(s)** | **Example** |
| ------------ | ------------ | ----------- | 
| The player acquires resources through the course of gameplay.| Acquire | A player loots a potion off a defeated enemy:<br><br>`[UnityAnalytics onItemAcquired: @"00001" itemId: @"Health Potion" transactionContext: @"PvE Loot" level: @"Pirate Bay" itemType: @"Item" amount: 1.0F balance: 5.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];` |
| The player acquires multiple items bundled through the course of gameplay.| Acquire for each item, sharing the same transaction ID. | A player receives a potion and silver pieces upon completing a quest:<br><br>`[UnityAnalytics onItemAcquired: @"00002" itemId: @"Health Potion" transactionContext: @"PC Quest" level: @"Pirate Bay" itemType: @"Item" amount: 1.0F balance: 6.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];`<br><br>`[UnityAnalytics onItemSpent: @"00002" itemId: @"Silver" transactionContext: @"PC Quest" level: @"Pirate Bay" itemType: @"Currency" amount: 50.0F balance: 200.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];` |
| The player acquires resources through an in-app purchase. | Acquire (in the purchase handler). | A player purchases a premium currency pack:<br><br>`[UnityAnalytics onItemAcquired: @"00003" itemId: @"Gold Coin" transactionContext: @"Gold Vendor" level: @"Pirate Bay" itemType: @"Currency" amount: 300.0F balance: 300.0F acquisitionType: kUnityAnalyticsAcquisitionTypePremium];` |
| The player acquires multiple items bundled through an in-app purchase. | Acquire for each item (in the purchase handler), sharing the same transaction ID. | A player spends real money to purchase an armor bundle:<br><br>`[UnityAnalytics onItemAcquired: @"00004" itemId: @"Warlord Breastplate" transactionContext: @"Castle Armory" level: @"75" itemType: @"Item" amount: 1.0F balance: 1.0F acquisitionType: kUnityAnalyticsAcquisitionTypePremium];`<br><br>`A[UnityAnalytics onItemAcquired: @"00004" itemId: @"Warlord Helmet" transactionContext: @"Castle Armory" level: @"75" itemType: @"Item" amount: 1.0F balance: 1.0F acquisitionType: kUnityAnalyticsAcquisitionTypePremium];`<br><br>`[UnityAnalytics onItemAcquired: @"00004" itemId: @"Warlord Boots" transactionContext: @"Castle Armory" level: @"75" itemType: @"Item" amount: 1.0F balance: 1.0F acquisitionType: kUnityAnalyticsAcquisitionTypePremium];` |
| The player consumes resources through the course of gameplay. | Spend | A player uses a potion in battle:<br><br>`[UnityAnalytics onItemSpent: @"00005" itemId: @"Health Potion" transactionContext: @"Arena Combat" level: @"42" itemType: @"Item" amount: 1.0F balance: 5.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];` |
| The player consumes a resource to acquire another resource. | Spend and acquire, sharing the same transaction ID. | A player spends silver pieces to acquire crafting resources:<br><br>`[UnityAnalytics onItemSpent: @"00006" itemId: @"Silver" transactionContext: @"Bazaar Trading Post" level: @"25" itemType: @"Currency" amount: 50.0F balance: 150.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];`<br><br>`[UnityAnalytics onItemAcquired: @"00006" itemId: @"Arrowhead" transactionContext: @"Bazaar Trading Post" level: @"25" itemType: @"Resource" amount: 25.0F balance: 25.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];` |
| The player spends premium currency to acquire a virtual good. | Spend and acquire, sharing the same transaction ID. | A player purchases a weapon with premium currency:<br><br>`[UnityAnalytics onItemSpent: @"00007" itemId: @"Gold Coin" transactionContext: @"Arena Store" level: @"Grand Champion" itemType: @"Currency" amount: 300.0F balance: 0.0F acquisitionType: kUnityAnalyticsAcquisitionTypePremium];`<br><br>`[UnityAnalytics onItemAcquired: @"00007" itemId: @"Vorpal Blade" transactionContext: @"Arena Store" level: @"Grand Champion" itemType: @"Item" amount: 50.0F balance: 200.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];` |
| The player consumes multiple resources to acquire a virtual good. | Spend for each resource spent, and acquired. | A player gives the blacksmith currency and materials to make them a weapon:<br><br>`[UnityAnalytics onItemSpent: @"00008" itemId: @"Silver" transactionContext: @"Royal Blacksmith" level: @"Royal Castle" itemType: @"Currency" amount: 100.0F balance: 50.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];`<br><br>`[UnityAnalytics onItemSpent: @"00008" itemId: @"Iron Ore" transactionContext: @"Royal Blacksmith" level: @"Royal Castle" itemType: @"Resource" amount: 1.0F balance: 0.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];`<br><br>`[UnityAnalytics onItemSpent: @"00008" itemId: @"Stinging Blade Mold" transactionContext: @"Royal Blacksmith" level: @"Royal Castle" itemType: @"Resource" amount: 1.0F balance: 0.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];`<br><br>`[UnityAnalytics onItemAcquired: @"00008" itemId: @"Stinging Blade" transactionContext: @"Royal Blacksmith" level: @"Royal Castle" itemType: @"Item" amount: 1.0F balance: 1.0F acquisitionType: kUnityAnalyticsAcquisitionTypeSoft];` |

**Note**: Make sure you implement these events anywhere inventory transactions occur, including in your [callback handler for rewarded ads](MonetizationBasicIntegrationIos.md#implementing-rewarded-ads).

#### Logging level failure 
Call the `onLevelFail` event when players fail a level or die in-game. The data model can use this to show a Promo for a Product that will help them pass the level on the next attempt. 

```
+ (void) onLevelFail: (int) levelIndex;
``` 

#### Logging levels-up 
Call the `onLevelUp` event when players progress or increase in power.  

```
+ (void) onLevelUp: (int) newLevelIndex;
```

See the following code sample for an example implementation: 

```
#import  

@interface ViewController : UIViewController 

@end 

@implementation ViewController 

// Call when a player acquires an item: 
- (void)acquireItem:(NSString *)transactionId itemId:(NSString *)itemId transactionContext:(NSString *)transactionContext level:(NSString *)level itemType:(NSString *)itemType amount:(float)amount balance:(float)balance acquisitionType:(UnityAnalyticsAcquisitionType)acquisitionType { 

    [UnityAnalytics onItemAcquired:transactionId itemId:itemId transactionContext:transactionContext level:level itemType:itemType amount:amount balance:balance acquisitionType:acquisitionType]; 

    // Add commodity to the player’s inventory. 
} 

// Call when a player consumes or sells an item: 
- (void)spentItem:(NSString *)transactionId itemId:(NSString *)itemId transactionContext:(NSString *)transactionContext level:(NSString *)level itemType:(NSString *)itemType amount:(float)amount balance:(float)balance acquisitionType:(UnityAnalyticsAcquisitionType)acquisitionType { 

    [UnityAnalytics onItemSpent:transactionId itemId:itemId transactionContext:transactionContext level:level itemType:itemType amount:amount balance:balance acquisitionType:acquisitionType]; 

    // Deduct commodity from the player’s inventory. 
} 

// Call when a player dies or fails a challenge: 
- (void)levelFail:(int)levelIndex { 

    [UnityAnalytics onLevelFail:levelIndex]; 

    // Deduct a life.  
    // Respawn the player or display the Game Over menu. 
} 

// Call when a player gains a level or similarly upgrades: 
- (void)levelUp:(int)theNewLevelIndex { 

    [UnityAnalytics onLevelFail:theNewLevelIndex]; 
    // Increase the player’s stats. 
    // Unlock new skills. 
} 
@end 
```

### Configuring Personalized Placements 
[Create Placements](MonetizationPlacements.md) for your monetization surfacing points (as described in the [pre-integration guide](MonetizationPersonalizedPlacementsPrep.md#identify-surfacing-points-that-will-entice-players-to-enhance-their-experience)), or convert existing ones in your game. To create or convert a Personalized Placement, simply [enable both **Ads** and **IAP Promo** content](MonetizationPlacements.md#placement-settings) in the **Content Settings** menu.  

Unity recommends multiple Personalized Placements in your game, to give the player a consistent experience throughout.  

#### Converting an existing video Placement 
If you already have a live Placement that you wish to convert, simply add IAP Promo offers to your rewarded or non-rewarded video Placement. To do so, navigate to the desired Placement’s settings menu, then select **Content Types** and check **IAP Promo**. Make sure you associate your Promo campaigns with your active Placements. 

#### Activating Placements 
Provide your Unity representative with the [Game ID](MonetizationResourcesDashboardGuide.md#game-ids) and Personalized [Placement ID](MonetizationPlacements.md#placement-settings)(s) you wish to activate in your Project. 

**Important**: Be sure to revisit your [mediation integration](MonetizationResourcesMediation.md) and update the Placement ID(s) you’re using. 

### Configuring IAP Promos 
The final step is to configure Promos. Begin by [importing the Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html#ExportCatalog) you created when implementing IAP to the [Operate dashboard](https://operate.dashboard.unity3d.com). Each Product requires an associated [creative](https://docs.unity3d.com/Manual/IAPPromoProducts.html#ConfigureCreatives) in order to be included with Promos. 

Next, [follow the Promo configuration wizard](https://docs.unity3d.com/Manual/IAPPromoPromotions.html) for each Promo you wish to include in your Personalized Placements. The setup consists of the following steps: 
 
1. Name the Promo. 
2. Select Products to include in the Promo. 
3. Select the Personalized Placements through which to promote the Promo. 

#### Testing Promos 
To verify that your Placements are configured to receive Promo content as well as ads, follow the [testing steps](MonetizationBasicIntegrationIos.md#testing) detailed in the basic ads integration guide. If the Placement is configured properly, you will see a custom test ad indicating that Promos are enabled. 

## What's next?
View the Personalized Placements [post-integration steps](MonetizationPersonalizedPlacementsScale.md), to monitor performance and scale for maximum revenue.