namespace UnityEngine.Advertisements
{
    internal interface IUnityEngineApplication
    {
        bool isEditor { get; }
        RuntimePlatform platform { get; }
        string unityVersion { get; }
    }
}
