using System;

namespace UnityEngine.Advertisements
{
    class EventArgsWithPlacementId : EventArgs
    {
        public string placementId { get;}

        public EventArgsWithPlacementId(string placementId)
        {
            this.placementId = placementId;
        }
    }
}
