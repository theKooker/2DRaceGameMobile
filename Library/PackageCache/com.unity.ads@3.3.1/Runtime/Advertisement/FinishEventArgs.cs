using System;

namespace UnityEngine.Advertisements
{
    class FinishEventArgs : EventArgs
    {
        public string placementId { get; private set; }
        public ShowResult showResult { get; private set; }

        public FinishEventArgs(string placementId, ShowResult showResult)
        {
            this.placementId = placementId;
            this.showResult = showResult;
        }
    }
}
