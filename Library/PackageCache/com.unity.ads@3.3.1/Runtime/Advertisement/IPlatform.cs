using System;

namespace UnityEngine.Advertisements
{
    interface IPlatform
    {
        event EventHandler<StartEventArgs> OnStart;
        event EventHandler<FinishEventArgs> OnFinish;

        IBanner Banner { get; }
        bool isInitialized { get; }
        bool isSupported { get; }
        string version { get; }
        bool debugMode { get; set; }

        void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad);
        bool IsReady(string placementId);
        PlacementState GetPlacementState(string placementId);
        void Load(string placementId);
        void Show(string placementId);
        void SetMetaData(MetaData metaData);
        void AddListener(IUnityAdsListener listener);
        void RemoveListener(IUnityAdsListener listener);
    }
}
