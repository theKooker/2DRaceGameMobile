# Personalized Placements integration for Unity developers
## Overview
This guide covers implementation for Personalized Placements in your made-with-Unity game.

* If you are an iOS developer using Objective-C, [click here](MonetizationPersonalizedPlacementsIos.md). 
* If you are an Android developer using Java, [click here](MonetizationPersonalizedPlacementsAndroid.md). 

### Guide contents
* [Integrate the Unity Monetization SDK](#installing-unity-ads) 
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
### Installing Unity Ads
To ensure the latest version of Unity Ads, download it through the Asset store, or through the Unity Package Manager in the Editor.

**Important**: You must choose either the Asset or the package. Installing both may lead to build errors.

#### Using the Asset package
[Download](https://assetstore.unity.com/packages/add-ons/services/unity-ads-66123) the latest version of Unity Ads from the Asset store. For information on downloading and installing Asset packages, see [Asset packages documentation](https://docs.unity3d.com/Manual/AssetPackages.html). 

#### Using Package Manager
Install the latest version of Unity Ads through the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html), by following these steps:

1. In the Unity Editor, select **Window** > **Package Manager** to open the Package Manager.
2. Select the **Advertisements** package from the list, then select the most recent verified version.
3. Click the **Install** or **Update** button. 

### Implementing Unity Ads 
To implement rewarded or non-rewarded ads in your Unity Project, you must initialize the Unity Monetization SDK, then implement scripts at your surfacing points that access and display content through Placement IDs. For a comprehensive guide, read the [integration guide for Unity](MonetizationBasicIntegrationUnity.md#integration-for-personalized-placements).

**Note**: The Unity Ads integration for Personalized Placements differs slightly from the basic ads integration, as it requires the [`Monetization`](../api/UnityEngine.Monetization.html) API instead of the `Advertisements` API. 

Unity also recommends reviewing the [rewarded ads best practices](MonetizationResourcesBestPracticesAds.md) guide as part of your implementation strategy.  

#### Using mediation 
If you use ads mediation, the integration process for Unity Ads differs as dictated by your mediation partner. You can find integration guides and SDK adapters for the following widely-used mediation partners below:
 
* [MoPub](https://developers.mopub.com/docs/mediation/networks/unityads/) 
* [AdMob](https://developers.google.com/admob/unity/mediation/unity) 

For more details, see documentation on [Unity Ads mediation](MonetizationResourcesMediation.md). If you have questions regarding compatibility with other mediators, please contact your Unity representative.

### Implementing in-app purchasing  (IAP)
[Unity IAP](https://docs.unity3d.com/Manual/UnityIAP.html) provides the most streamlined integration with Personalized Placements. However, you may also take advantage of the feature if you use a custom purchasing integration, by implementing a purchasing adapter (see below). 

#### Using Unity IAP 
Always use the most current [Unity IAP Asset package](https://assetstore.unity.com/packages/add-ons/services/billing/unity-iap-68207). The IAP Promo feature requires version 1.20 or higher. For detailed setup information, see documentation on configuring [Unity IAP](https://docs.unity3d.com/Manual/UnityIAP.html). If you’re interested in the simplest implementation, Unity recommends taking advantage of the [Codeless IAP](https://docs.unity3d.com/Manual/UnityIAPCodelessIAP.html) feature.  

**Important**: If you implement Unity Ads through a third party mediator, you will need an adaptor that allows the Unity Ads SDK to receive purchase and conversion events while in mediation. See documentation on [using IAP Promo in mediation](MonetizationResourcesMediation.md#using-iap-promo-in-mediation) for more information.

Note that your game must initialize the Unity IAP SDK before initializing the Unity Monetization SDK for IAP Promo to work.

#### Using a purchasing adapter 
If you use a custom IAP solution, see documentation on [custom purchase integration](MonetizationPurchasingIntegrationUnity.md) for guidelines on implementing a __purchasing adapter__. This adapter is required for Unity to hook into the necessary machine learning data. 

### Implementing Standard Events 
[Standard Events](https://docs.unity3d.com/Manual/UnityAnalyticsStandardEvents.html) track user experience and player behavior across your game. Implement a subset of game economy events so that Unity’s machine learning engine can optimize your Placement content. Game economy events help the Personalized Placements data model form a complete picture of each player’s preferences and actions in your game, then determine what actions to take accordingly.

#### Logging acquire and spend events  
The following events help train Personalized Placements to know when it’s best to show a Promo, and which Promos to show.  
Call the `ItemAcquired` event when players acquire a resource in-game. 

```
public static AnalyticsResult ItemAcquired (AcquisitionType currencyType, string transactionContext, float amount, string itemId, float balance, string itemType = null, string level = null, string transactionId = null, IDictionary<string, object> eventData = null)
```      

Call the `ItemSpent` event when players consume a resource in-game. 

```
public static AnalyticsResult ItemSpent (AcquisitionType currencyType, string transactionContext, float amount, string itemId, float balance, string itemType = null, string level = null, string transactionId = null, IDictionary<string, object> eventData = null)
```     
 
| **Parameter** | **Data type** | **Description** | **Examples** |
| ------------- | ------------- | --------------- | ------------ | 
| `currencyType` | `AcquisitionType` | (Required) The `AcquisitionType` can be either:<br><br><ul><li>`AcquisitionType.Premium` indicates the item was purchased with real currency.</li><li>`AcquisitionType.Soft` indicates the item was acquired with virtual currency or resources.</li></ul> | <ul><li>`AcquisitionType.Premium`</li><li>`AcquisitionType.Soft` |
| `transactionContext`| string | (Required) The context through which the item was spent or acquired. | <ul><li>`"IAP Store"`</li><li>`"Shop"`</li><li>`"Crafting"`</li></ul> |
| `amount` | float | (Required) The unit quantity of the spent or acquired item. | <ul><li>`1`</li><li>`200`</li><li>`5000`</li></ul> |
| `itemId` | string | (Required) A name or unique identifier for the spent or acquired item. | <ul><li>`"Mana Potion"`</li><li>`"Hay Bales"`</li><li>`"Tokens"`</li></ul> |
| `balance` | float | (Optional) The player’s new (post-transaction) quantity of the spent or acquired item. | <ul><li>`10`</li><li>`50`</li><li>`120`</li></ul> |
| `itemType` | string | (Optional) The category of the spent or acquired item. Unity recommends using one of the following categories for proper data modeling:<br><br><ul><li>`"Currency"`</li><li>`"Resource"`</li><li>`"Item"`</li><li>`"Other"` | <ul><li>`"Currency"`</li><li>`"Resource"`</li><li>`"Item"` |
| `level`| string | (Optional) The name or identifier of the player’s progression level when the item was spent or acquired. This value is expressed differently depending on the game (for example, as a numerical level, as a title, or as progress on a map). | A game with traditional player levels:<br><br><ul><li>`"13"`</li><li>`"72"`</li></ul><br> A game that gauges progression with titles:<br><br><ul><li>`"Novice"`</li><li>`"Grand Champion"`</li></ul><br>A game without player levels, that gauges progression through advancing to levels or maps:<br><br><ul><li>`"Pirate Bay"`</li><li>`"Elysian Fields"`</li></ul> |
| `transactionId` | string | (Optional) A unique identifier for the specific transaction that occurred. You can use this to group multiple events into a single transaction.<br><br>**Note**: If the acquisition type is premium (meaning the player purchased the commodity with real money), Unity recommends matching the transaction ID logged at the point of purchase with the one sent for ItemAcquired or ItemSpent, so the data model can easily associate the two as part of a single transaction. If you use Unity IAP, the SDK automatically logs the transaction ID, currency code, and amount for each purchase. | <ui><li>`"00001"`</li><li>`"12345"`</li></ui> |
| `eventData` | `IDictionary` | (Optional) A dictionary of custom parameters. |  |

Consider the following use cases: 

| **Use case** | **Event(s)** | **Example** |
| ------------ | ------------ | ----------- | 
| The player acquires resources through the course of gameplay. | Acquire | A player loots a potion off a defeated enemy:<br><br>`Analytics.ItemAcquired (AcquisitionType.Soft, "PvE Loot", 1, "Health Potion", 5, "Item", "Pirate Bay", "00001");` |
| The player acquires multiple items bundled through the course of gameplay. | Acquire for each item, sharing the same transaction ID. | A player receives a potion and silver pieces upon completing a quest:<br><br>`Analytics.ItemAcquired (AcquisitionType.Soft, "PC Quest", 1, "Health Potion", 6, "Item", "Pirate Bay", "00002");`<br><br>`Analytics.ItemAcquired (AcquisitionType.Soft, "PC Quest", 50, "Silver", 200, "Currency", "Pirate Bay", "00002");` |
| The player acquires resources through an in-app purchase. | Acquire (in the purchase handler). | A player purchases a premium currency pack:<br><br>`Analytics.ItemAcquired (AcquisitionType.Premium, "Gold Vendor", 300, "Gold Coin", 300, "Currency", "Pirate Bay", "00003");` |
| The player acquires multiple items bundled through an in-app purchase. | Acquire for each item (in the purchase handler), sharing the same transaction ID. | A player spends real money to purchase an armor bundle:<br><br>`Analytics.ItemAcquired (AcquisitionType.Premium, "Castle Armory", 1, "Warlord Breastplate", 1, "Item", "75", "00004");`<br><br>`Analytics.ItemAcquired (AcquisitionType.Premium, "Castle Armory", 1, "Warlord Helmet", 1, "Item", "75", "00004");`<br><br>`Analytics.ItemAcquired (AcquisitionType.Premium, "Castle Armory", 1, "Warlord Boots", 1, "Item", "75", "00004");` |
| The player consumes resources through the course of gameplay. | Spend | A player uses a potion in battle:<br><br>`Analytics.ItemSpent (AcquisitionType.Soft, "Arena Combat", 1, "Health Potion", 5, "Item", "42", "00005");` |
| The player consumes a resource to acquire another resource. | Spend and acquire, sharing the same transaction ID. | A player spends silver pieces to acquire crafting resources:<br><br>`Analytics.ItemSpent (AcquisitionType.Soft, "Bazaar Trading Post", 50, "Silver", 150, "Currency", "25", "00006");`<br><br>`Analytics.ItemAcquired (AcquisitionType.Soft, "Bazaar Trading Post", 25, "Arrowhead", 25, "Resource", "25", "00006",);` |
| The player spends premium currency to acquire a virtual good. | Spend and acquire, sharing the same transaction ID. | A player purchases a weapon with premium currency:<br><br>`Analytics.ItemSpent (AcquisitionType.Premium, "Arena Store", 300, "Gold Coin", 0, "Currency", "Grand Champion", "00007");`<br><br>`Analytics.ItemAcquired (AcquisitionType.Soft, "Arena Store", 1, "Vorpal Blade", 1, "Item", "Grand Champion", "00007",);` |
| The player consumes multiple resources to acquire a virtual good. | Spend for each resource spent, and acquired. | A player gives the blacksmith currency and materials to make them a weapon:<br><br>`Analytics.ItemSpent (AcquisitionType.Soft, "Royal Blacksmith", 100, "Silver", 50, "Currency", "Royal Castle", "00008");`<br><br>`Analytics.ItemSpent (AcquisitionType.Soft, "Royal Blacksmith", 1, "Iron Ore", 0, "Resource", "Royal Castle", "00008");`<br><br>`Analytics.ItemSpent (AcquisitionType.Soft, "Royal Blacksmith", 1, "Stinging Blade Mold", 0, "Resource", "Royal Castle", "00008");`<br><br>`Analytics.ItemAcquired (AcquisitionType.Soft, "Royal Blacksmith", 1, "Stinging Blade", 1, "Item", "Royal Castle", "00008",);` |

**Note**: Make sure you implement these events anywhere inventory transactions occur, including in your [callback handler for rewarded ads](MonetizationBasicIntegrationUnity#implementing-rewarded-ads). 

#### Logging level failure 
Call the `LevelFail` event when players fail a level or die in-game. The data model can use this to show a Promo for a Product that will help them pass the level on the next attempt. 

```
public static AnalyticsResult LevelFail (string name, IDictionary<string, object> eventData = null)
``` 

| **Parameter** | **Data type** | **Description** | **Examples** |
| ------------- | ------------- | --------------- | ------------ | 
| `name` | string | (Required) Either the name of the failed level, or its index. | <ul><li>`"Pirate Bay"`</li><li>`"4"`</li></ul> |
| `eventData` | `IDictionary` | (Optional) A dictionary of custom parameters. |  |

#### Logging level-ups 
Call the `LevelUp` event when players progress or increase in power.  

```
public static AnalyticsResult LevelUp (string name, IDictionary<string, object> eventData = null)
``` 

| **Parameter** | **Data type** | **Description** | **Examples** |
| ------------- | ------------- | --------------- | ------------ | 
| `name` | string | (Required) The new rank or level name. | <ul><li>`"99"`</li><li>`"Grand Champion"`</li></ul> |
| `eventData` | `IDictionary` | (Optional) A dictionary of custom parameters. |  |

See the following code sample for an example implementation: 

```
using UnityEngine.Analytics; 

 public class GameEconomyEventManager : Monobehaviour { 

   // Call when a player acquires an item: 
   public void AcquireItem(AcquisitionType currencyType, string transactionContext, float amount, string itemId) { 
     Analytics.ItemAcquired(currencyType, transactionContext, amount, itemId);
     // Add commodity to the player’s inventory. 
   } 

   // Call when a player consumes or sells an item: 
   public void ConsumeItem(AcquisitionType currencyType, string transactionContext, float amount, string itemId) { 
     Analytics.ItemSpent(currencyType, transactionContext, amount, itemId);
     // Deduct commodity from the player’s inventory. 
   } 

   // Call when the player dies or fails a challenge: 
   public void FailMission(string missionName) { 
     Analytics.LevelFail(missionName);
     // Deduct a life. 
     // Respawn the player or display the Game Over menu. 
   } 

   // Call when the player gains a level or similarly upgrades: 
   public void IncreaseLevel(string levelValue) { 
     Analytics.LevelUp(levelValue);
     // Increase the player’s stats. 
     // Unlock new skills. 
   } 
 } 
```

### Configuring Personalized Placements 
[Create Placements](MonetizationPlacements.md) for your monetization surfacing points (as described in the [pre-integration guide](MonetizationPersonalizedPlacementsPrep.md)), or convert existing ones in your game. To create or convert a Personalized Placement, simply [enable both Ads and IAP Promo content](MonetizationPlacements.md#placement-settings) in the **Content Settings** menu.  

Unity recommends multiple Personalized Placements in your game, to give the player a consistent experience throughout.  

#### Converting an existing video Placement 
If you already have a live Placement that you wish to convert, simply add IAP Promo offers to your rewarded or non-rewarded video Placement. To do so, navigate to the desired Placement’s settings menu, then select **Content Types** and check **IAP Promo**. Make sure you associate your Promo campaigns with your active Placements. 

#### Activating Placements 
Provide your Unity representative with the [Game ID](MonetizationResourcesDashboardGuide.md#platform-settings) and Personalized [Placement ID](MonetizationPlacements.md#placement-settings)(s) you wish to activate in your Project. 

**Important**: Be sure to revisit your [mediation integration](MonetizationResourcesMediation.md) and update the Placement ID(s) you’re using. 

### Configuring IAP Promos 
The final step is to configure Promos. Begin by [importing the Product Catalog](https://docs.unity3d.com/Manual/IAPPromoProducts.html#ExportCatalog) you created when implementing IAP to the [Operate dashboard](https://operate.dashboard.unity3d.com). Each Product requires an associated [creative](https://docs.unity3d.com/Manual/IAPPromoProducts.html#ConfigureCreatives) in order to be included with Promos. 

Next, [follow the Promo configuration wizard](https://docs.unity3d.com/Manual/IAPPromoPromotions.html) for each Promo you wish to include in your Personalized Placements. The setup consists of the following steps: 
 
1. Name the Promo. 
2. Select Products to include in the Promo. 
3. Select the Personalized Placements through which to promote the Promo. 

#### Testing Promos
To verify that your Placements are configured to receive Promo content as well as ads, follow the [testing steps](MonetizationBasicIntegrationUnity.md#testing) detailed in the ads integration guide. If the Placement is configured properly, you will see a custom test ad indicating that Promos are enabled. 

## What's next?
View the Personalized Placements [post-integration steps](MonetizationPersonalizedPlacementsScale.md), to monitor performance and scale for maximum revenue.