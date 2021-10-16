using System;

namespace UnityEngine.Advertisements
{
    class ReadyEventArgs : EventArgs
    {
        public string placementId { get; private set; }

        public ReadyEventArgs(string placementId)
        {
            this.placementId = placementId;
        }
    }
}
