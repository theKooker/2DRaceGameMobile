using System;

namespace UnityEngine.Advertisements
{
    class HideEventArgs : EventArgs
    {
        public string placementId { get; private set; }

        public HideEventArgs(string placementId)
        {
            this.placementId = placementId;
        }
    }
}
