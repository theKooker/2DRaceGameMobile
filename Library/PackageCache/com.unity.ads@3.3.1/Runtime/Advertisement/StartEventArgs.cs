using System;

namespace UnityEngine.Advertisements
{
    class StartEventArgs : EventArgs
    {
        public string placementId { get; private set; }

        public StartEventArgs(string placementId)
        {
            this.placementId = placementId;
        }
    }
}
