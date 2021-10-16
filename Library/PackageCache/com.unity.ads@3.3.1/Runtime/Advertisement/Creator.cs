using System;

namespace UnityEngine.Advertisements
{
    static class Creator
    {
        internal static IPlatform CreatePlatform()
        {
            try
            {                
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
                return new Platform();
#else
                return new UnsupportedPlatform();
#endif
            }
            catch (Exception exception)
            {
                try
                {
                    Debug.LogError("Initializing Unity Ads.");
                    Debug.LogError(exception.Message);
                }
                catch (MissingMethodException)
                {
                }
                return new UnsupportedPlatform();
            }
        }
    }
}
