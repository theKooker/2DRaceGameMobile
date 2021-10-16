# GDPR Compliance
On May 25, 2018, the General Data Protection Regulation ([GDPR](https://en.wikipedia.org/wiki/General_Data_Protection_Regulation)) took effect in the European Economic Area (EEA), and all versions of the Unity Ads SDK are compliant.

**Note**: Consent extends beyond GDPR and should be applied in any region where consent is required to collect personal data.

## What this means
Unity provides a built-in solution for handling end-user consent, as well as APIs to pass consent flags if you wish to implement a custom solution. Passing an affirmative consent flag (`consent == "true"`) to the SDK means that Unity and its partners can show personalized content for that user, while `consent == "false"` means that user cannot receive personalized content. 

## Unity's built-in solution
Unity recommends that you update to the latest version of the SDK, but it is not required for GDPR compliance. Legacy versions (below version 2.0) of the SDK now only serve contextual ads to users, strictly based on geographic location and current gameplay. No historical or personal data is used for ad targeting, including user behavior within the app and across other apps, or installs.

Versions 2.0 and higher automatically present users with an opportunity to opt in to targeted advertising, with no implementation needed from the publisher. On a per-app basis, the first time a Unity ad appears, the user sees a banner with the option to opt in to behaviorally targeted advertising. Thereafter, the user can click an information button to manage their privacy choices.

## Implementing a custom solution
If a publisher or mediator manually requests a user opt-in, the Unity opt-in will not appear. Please note that users can still request opt-out or data deletion, and access their data at any time by tapping the Unity Data Privacy icon when or after an ad appears.

### Passing user consent
Use the following code to pass a consent flag to the Unity Ads SDK:

#### Unity (C#)
```
// If the user opts in to targeted advertising:
MetaData gdprMetaData = new MetaData("gdpr");
gdprMetaData.Set("consent", "true");
Advertisement.SetMetaData(gdprMetaData);

// If the user opts out of targeted advertising:
MetaData gdprMetaData = new MetaData("gdpr");
gdprMetaData.Set("consent", "false");
Advertisement.SetMetaData(gdprMetaData);
```

#### iOS (Objective-C)
```
// If the user opts in to targeted advertising:
UADSMetaData *gdprConsentMetaData = [[UADSMetaData alloc] init];
[gdprConsentMetaData set:@"gdpr.consent" value:@YES];
[gdprConsentMetaData commit];

// If the user opts out of targeted advertising:
UADSMetaData *gdprConsentMetaData = [[UADSMetaData alloc] init];
[gdprConsentMetaData set:@"gdpr.consent" value:@NO];
[gdprConsentMetaData commit];
```

#### Android (Java)
```
// If the user opts in to targeted advertising:
MetaData gdprMetaData = new MetaData(this);
gdprMetaData.set("gdpr.consent", true);
gdprMetaData.commit();

// If the user opts out of targeted advertising:
MetaData gdprMetaData = new MetaData(this);
gdprMetaData.set("gdpr.consent", false);
gdprMetaData.commit();
```

If the user takes no action to agree or disagree to targeted advertising (for example, closing the prompt), Unity recommends re-prompting them at a later time.

## Learn more
Please visit our legal site for more information on [Unity's approach to GDPR](https://unity3d.com/legal/gdpr).