namespace UnityEngine.Advertisements
{
    class UnityEngineApplication : IUnityEngineApplication
    {
        public bool isEditor { get { return Application.isEditor; } }
        public RuntimePlatform platform { get { return Application.platform; } }
        public string unityVersion { get { return Application.unityVersion; } }
    }
}
