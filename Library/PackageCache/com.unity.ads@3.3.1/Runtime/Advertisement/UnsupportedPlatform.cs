using System;

namespace UnityEngine.Advertisements
{
    sealed class UnsupportedPlatform : IPlatform
    {
        private static IBanner s_Banner = new NullBanner();
        public event EventHandler<ReadyEventArgs> OnReady { add {} remove {} }
        public event EventHandler<StartEventArgs> OnStart { add {} remove {} }
        public event EventHandler<FinishEventArgs> OnFinish;
        public event EventHandler<ErrorEventArgs> OnError { add {} remove {} }

        public bool isInitialized
        {
            get
            {
                return false;
            }
        }

        public bool isSupported
        {
            get
            {
                return false;
            }
        }

        public string version
        {
            get
            {
                return null;
            }
        }

        public bool debugMode
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public IBanner Banner
        {
            get
            {
                return s_Banner;
            }
        }

        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad)
        {
        }

        public bool IsReady(string placementId)
        {
            return false;
        }

        public PlacementState GetPlacementState(string placementId)
        {
            return PlacementState.NotAvailable;
        }

        public void Load(string placementId)
        {}

        public void Show(string placementId)
        {
            var handler = OnFinish;
            if (handler != null)
            {
                handler(this, new FinishEventArgs(placementId, ShowResult.Failed));
            }
        }

        public void SetMetaData(MetaData metaData)
        {
        }

        public void AddListener(IUnityAdsListener listener)
        {
        }

        public void RemoveListener(IUnityAdsListener listener)
        {
        }

        private class NullBanner : IBanner
        {
            public event EventHandler<EventArgsWithPlacementId> OnShow { add {} remove {} }
            public event EventHandler<EventArgsWithPlacementId> OnHide { add {} remove {} }
            public event EventHandler<ErrorEventArgs> OnError { add {} remove {} }
            public event EventHandler<EventArgs> OnUnload { add {} remove {} }
            public event EventHandler<EventArgsWithPlacementId> OnLoad { add {} remove {} }
            public event EventHandler<EventArgsWithPlacementId> OnClick { add {} remove {}}

            public bool isLoaded => false;
            public void Load(string placementId, BannerLoadOptions loadOptions) {}
            public void Hide(bool destroy = false) {}
            public void Show(string placementId, BannerOptions showOptions) {}
            public void SetPosition(BannerPosition position) {}
        }
    }
}
