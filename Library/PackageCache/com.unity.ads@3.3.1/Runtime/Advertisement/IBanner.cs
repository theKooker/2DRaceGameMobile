using System;

namespace UnityEngine.Advertisements
{
    interface IBanner
    {
        bool isLoaded { get; }

        void Load(string placementId, BannerLoadOptions loadOptions);
        void Show(string placementId, BannerOptions showOptions);
        void Hide(bool destroy = false);
        void SetPosition(BannerPosition position);
    }
}
