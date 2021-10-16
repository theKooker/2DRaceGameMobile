namespace UnityEngine.Advertisements
{
    internal interface IPurchasingEventSender
    {
        void SendPurchasingEvent(string payload);
    }
}
