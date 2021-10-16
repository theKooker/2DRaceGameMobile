# Personalized Placements integration for Android developers
## Overview
This guide covers implementation for Personalized Placements in your Android game.

* If you are a Unity developer using C#, [click here](MonetizationPersonalizedPlacementsUnity.md). 
* If you are an iOS developer using Objective-C, [click here](MonetizationPersonalizedPlacementsIos.md). 

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
To get started, [download](https://github.com/Unity-Technologies/unity-ads-android/releases) the latest version of the Monetization framework for Android. Personalized Placements require the Monetization SDK 3.0 or later. For information on downloading and importing the SDK, read that section of the [basic ads integration guide](MonetizationBasicIntegrationAndroid.md#importing-the-monetization-framework) for Android. 

### Implementing Unity Ads 
To implement rewarded or non-rewarded ads in your Unity Project, you must initialize the Unity Monetization SDK, then implement scripts at your surfacing points that access and display content through Placement IDs. For a comprehensive guide, read the [ads integration guide](MonetizationBasicIntegrationAndroid.md#integration-for-personalized-placements) for Android.

**Note**: The Unity Ads integration for Personalized Placements differs slightly from the basic ads integration, as it requires the [`UnityMonetization`](../api/UnityEngine.Monetization.html) API instead of the `UnityAds` API.  

Unity also recommends reviewing the [rewarded ads best practices](MonetizationResourcesBestPracticesAds.md) guide as part of your implementation strategy.  

#### Using mediation
If you use ads mediation, the integration process for Unity Ads differs as dictated by your mediation partner. You can find integration guides and SDK adapters for the following widely-used mediation partners below:
 
* [MoPub](https://developers.mopub.com/docs/mediation/networks/unityads/) 
* [AdMob](https://developers.google.com/admob/unity/mediation/unity) 

For more details, see documentation on [Unity Ads mediation](MonetizationResourcesMediation.md). If you have questions regarding compatibility with other mediators, please contact your Unity representative. 

### Implementing in-app purchasing 
The Monetization SDK allows you to take advantage of Personalized Placements using a custom purchasing integration, by implementing a __purchasing adapter__. This adapter is required for Unity to hook into the necessary machine learning data. For a complete guide on implementing a purchasing adapter, see documentation on [purchasing integration](MonetizationPurchasingIntegrationAndroid.md) for Android.  

### Implementing Standard Events 
[Standard Events](https://docs.unity3d.com/2019.1/Documentation/Manual/UnityAnalyticsStandardEvents.html) track user experience and player behavior across your game. Implement a subset of game economy events so that Unity’s machine learning engine can optimize your Placement content. Game economy events help the Personalized Placements data model form a complete picture of each player’s preferences and actions in your game, then determine what actions to take accordingly.

The Monetization SDK allows you to use Unity Analytics APIs in Android games that are not made with Unity. To instrument these events in your game and fire the event at the user level to Unity’s endpoint, invoke the ```UnityAnalytics``` namespace: 

```
public class UnityAnalytics { }
``` 

#### Logging acquire and spend events
The following events help train Personalized Placements to know when it’s best to show a Promo.  

Call the `onItemAcquired` event when players acquire a resource in-game. 

```
public static void onItemAcquired (String transactionContext, Float amount, String itemId, Float balance, String itemType, String level, String transactionId, AcquisitionType acquisitionType);
```

Call the `onItemSpent` event when players consume a resource in-game. 

```
public static void onItemSpent (String transactionContext, Float amount, String itemId, Float balance, String itemType, String level, String transactionId, AcquisitionType acquisitionType);
```

| **Parameter** | **Data type** | **Description** | **Examples** |
| ------------- | ------------- | --------------- | ------------ | 
| `transactionContext` | String | (Required) The channel through which the item was spent or acquired. | <ul><li>`"IAP Store"`</li><li>`"Shop"`</li><li>`"Crafting"`</li></ul> |
| `amount` | Float | (Required) The unit quantity of the spent or acquired item. | <ul><li>`1.0F`</li><li>`200.0F`</li><li>`5000.0F`</li></ul> |
| `itemId` | String | (Required) A name or unique identifier for the spent or acquired item. | <ul><li>`"Mana Potion"`</li><li>`"Hay Bales"`</li><li>`"Tokens"`</li></ul> |
| `balance` | Float | (Optional) The player’s new (post-transaction) quantity of the spent or acquired item. | <ul><li>`10.0F`</li><li>`50.0F`</li><li>`120.0F`</li></ul> |
| `itemType` | String | (Optional) The category of the spent or acquired item. Unity recommends using one of the following categories for proper data modeling:<br><br><ul><li>`"Currency"`</li><li>`"Resource"`</li><li>`"Item"`</li><li>`"Other"`</li></ul> | <ul><li>`"Currency"`</li><li>`"Resource"`</li><li>`"Item"`</li></ul> |
| `level` | String | (Optional) The name or identifier of the player’s progression level when the item was spent or acquired. This value is expressed differently depending on the game (for example, as a numerical level, as a title, or as progress on a map). | A game with traditional player levels:<br><br><ul><li>`"13"`</li><li>`"72"`</li></ul><br>A game that gauges progression with titles:<br><br><ul><li>`"Novice"`</li><li>`"Grand Champion"`</li></ul><br>A game without player levels, that gauges progression through advancing to levels or maps:<br><br><ul><li>`"Pirate Bay"`</li><li>`"Elysian Fields"`</li></ul> |
| `transactionId` | String | (Optional) A unique identifier for the specific transaction that occurred. You can use this to group multiple events into a single transaction.<br><br>**Note**: If the `acquisitionType` is premium (meaning the player purchased the commodity with real money), Unity recommends matching the transaction ID logged at the point of purchase with the one sent for `onItemAcquired` or `onItemSpent`, so the data model can easily associate the two as part of a single transaction. For more information, see documentation on [purchasing integration](MonetizationPurchasingIntegrationAndroid.md) for Android. | <ul><li>`"001A"`</li><li>`"123A456B"`</li></ul> |
| `acquisitionType` | `AcquisitionType` | (Required) The `AcquisitionType` can be either:<br><br><ul><li>`AcquisitionType.PREMIUM` indicates the item was purchased with real currency.</li><li>`AcquisitionType.SOFT` indicates the item was purchased with virtual currency or resources.</li></ul> | <ul><li>`AcquisitionType.Premium`</li><li>`AcquisitionType.Soft`</li></ul> |

Consider the following examples: 

| **Use case** | **Event(s)** | **Example** |
| ------------ | ------------ | ----------- | 
| The player acquires resources through the course of gameplay. | Acquire | A player loots a potion off a defeated enemy:<br><br>`Analytics.ItemAcquired ("PvE Loot", 1, "Health Potion", 5, "Item", "Pirate Bay", "00001", AcquisitionType.SOFT);` |
| The player acquires multiple items bundled through the course of gameplay. | Acquire for each item, sharing the same transaction ID. | A player receives a potion and silver pieces upon completing a quest:<br><br>`Analytics.ItemAcquired ("PC Quest", 1, "Health Potion", 6, "Item", "Pirate Bay", "00002", AcquisitionType.SOFT);`<br><br>`Analytics.ItemAcquired ("PC Quest", 50, "Silver", 200, "Currency", "Pirate Bay", "00002", AcquisitionType.SOFT);` |
| The player acquires resources through an in-app purchase. | Acquire (in the purchase handler). | A player purchases a premium currency pack:<br><br>`Analytics.ItemAcquired ("Gold Vendor", 300, "Gold Coin", 300, "Currency", "Pirate Bay", "00003", AcquisitionType.PREMIUM);` |
| The player acquires multiple items bundled through an in-app purchase. | Acquire for each item (in the purchase handler), sharing the same transaction ID. | A player spends real money to purchase an armor bundle:<br><br>`Analytics.ItemAcquired ("Castle Armory", 1, "Warlord Breastplate", 1, "Item", "75", "00004", AcquisitionType.PREMIUM);`<br><br>`Analytics.ItemAcquired ("Castle Armory", 1, "Warlord Helmet", 1, "Item", "75", "00004", AcquisitionType.PREMIUM);`<br><br>`Analytics.ItemAcquired ("Castle Armory", 1, "Warlord Boots", 1, "Item", "75", "00004", AcquisitionType.PREMIUM);` |
| The player consumes resources through the course of gameplay. | Spend | A player uses a potion in battle:<br><br>`Analytics.ItemSpent ("Arena Combat", 1, "Health Potion", 5, "Item", "42", "00005", AcquisitionType.SOFT);` |
| The player consumes a resource to acquire another resource. | Spend and acquire, sharing the same transaction ID | A player spends silver pieces to acquire crafting resources:<br><br>`Analytics.ItemSpent ("Bazaar Trading Post", 50, "Silver", 150, "Currency", "25", "00006", AcquisitionType.SOFT);`<br><br>`Analytics.ItemAcquired ("Bazaar Trading Post", 25, "Arrowhead", 25, "Resource", "25", "00006", AcquisitionType.SOFT);` |
| The player spends premium currency to acquire a virtual good. | Spend and acquire, sharing the same transaction ID. | A player purchases a weapon with premium currency:<br><br>`Analytics.ItemSpent ("Arena Store", 300, "Gold Coin", 0, "Currency", "Grand Champion", "00007", AcquisitionType.PREMIUM);`<br><br>`Analytics.ItemAcquired ("Arena Store", 1, "Vorpal Blade", 1, "Item", "Grand Champion", "00007", AcquisitionType.SOFT);` |
| The player consumes multiple resources to acquire a virtual good. | Spend for each resource spent, and acquired. | A player gives the blacksmith currency and materials to make them a weapon:<br><br>`Analytics.ItemSpent ("Castle Blacksmith", 100, "Silver", 50, "Currency", "Royal Castle", "00008", AcquisitionType.SOFT);`<br><br>`Analytics.ItemSpent ("Royal Blacksmith", 1, "Iron Ore", 0, "Resource", "Royal Castle", "00008", AcquisitionType.SOFT);`<br><br>`Analytics.ItemSpent ("Royal Blacksmith", 1, "Stinging Blade Mold", 0, "Resource", "Royal Castle", "00008", AcquisitionType.SOFT);`<br><br>`Analytics.ItemAcquired ("Royal Blacksmith", 1, "Stinging Blade", 1, "Item", "Royal Castle", "00008", AcquisitionType.SOFT);` |

**Note**: Make sure you implement these events anywhere inventory transactions occur, including in your [callback handler for rewarded ads](MonetizationBasicIntegrationAndroid.md#implementing-rewarded-ads).

#### Logging level failure 
Call the `LevelFail` event when players fail a level or die in-game. The data model can use this to show a Promo for a Product that will help them pass the level on the next attempt. 

```
public static void onLevelFail (Integer levelIndex);
``` 

#### Logging level-ups 
Call the `LevelUp` event when players progress or increase in power.  

```
public static void onLevelUp (Integer newLevelIndex);
``` 

See the following code sample for an example implementation: 

```
import com.unity3d.services.analytics.AcquisitionType; 
import com.unity3d.services.analytics.UnityAnalytics; 

public class GameEconomyEventManager extends Activity { 

    @Override 

    protected void onCreate(Bundle savedInstanceState) { 
        super.onCreate(savedInstanceState); 
    } 

    public void acquireItem(String transactionContext, Float amount, String itemId, Float balance, String itemType, String level, String transactionId, AcquisitionType acquisitionType) { 
        UnityAnalytics.onItemAcquired(transactionContext, amount, itemId, balance, itemType, level, transactionId, acquisitionType); 
        // Add commodity to the player’s inventory. 
    } 

    public void spentItem(String transactionContext, Float amount, String itemId, Float balance, String itemType, String level, String transactionId, AcquisitionType acquisitionType) { 
       UnityAnalytics.onItemSpent(transactionContext, amount, itemId, balance, itemType, level, transactionId, acquisitionType); 
        // Deduct commodity from the player’s inventory. 
    } 

    public void levelFail(Integer levelIndex) { 
        UnityAnalytics.onLevelFail(levelIndex); 
        // Deduct a life. 
        // Respawn the player or display the Game Over menu. 
    } 

    public void levelUp(Integer newLevelIndex) { 
        UnityAnalytics.onLevelUp(newLevelIndex); 
        // Increase the player’s stats. 
        // Unlock new skills. 
    } 
}
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

Next, [follow the Promo configuration wizard](https://docs.unity3d.com/2019.1/Documentation/Manual/IAPPromoPromotions.html) for each Promo you wish to include in your Personalized Placements. The setup consists of the following steps: 
 
1. Name the Promo. 
2. Select Products to include in the Promo. 
3. Select the Personalized Placements through which to promote the Promo. 

#### Testing Promos 
To verify that your Placements are configured to receive Promo content as well as ads, follow the [testing steps](MonetizationBasicIntegrationAndroid.md#testing) detailed in the basic ads integration guide. If the Placement is configured properly, you will see a custom test ad indicating that Promos are enabled. 

## What's next?
View the Personalized Placements [post-integration steps](MonetizationPersonalizedPlacementsScale.md), to monitor performance and scale for maximum revenue.