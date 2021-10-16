#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
#elif UNITY_ANDROID
using System;
using System.Collections.Generic;
#elif UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
#endif

namespace UnityEngine.Advertisements
{
#if UNITY_EDITOR
    sealed internal class Platform : IPlatform
    {
        static IBanner s_Banner;
        internal static string s_BaseUrl = "http://editor-support.unityads.unity3d.com/games";
        static string s_Version = "3.3.0";

        bool m_DebugMode;
        Configuration m_Configuration;
        Placeholder m_Placeholder;
        static CallbackExecutor m_CallbackExecutor;

        public event EventHandler<StartEventArgs> OnStart;
        public event EventHandler<FinishEventArgs> OnFinish;

        private static HashSet<IUnityAdsListener> _listeners;

        internal Dictionary<string, bool> m_PlacementMap = new Dictionary<string, bool>();
        private Queue<string> m_QueuedLoads = new Queue<string>();
        private bool m_LoadEnabled = false;
        private string m_GameId;
        private bool m_IsInitialized = false;
        
        static Platform()
        {
            _listeners = new HashSet<IUnityAdsListener>();
            var callbackExecutorGameObject = new GameObject("UnityAdsCallbackExecutorObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };
            m_CallbackExecutor = callbackExecutorGameObject.AddComponent<CallbackExecutor>();
            Object.DontDestroyOnLoad(callbackExecutorGameObject);

            var fakeBanner = new GameObject("UnityAdsEditorFakeBanner")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };

            s_Banner = fakeBanner.AddComponent<Banner>();
        }

        public bool isInitialized
        {
            get
            {
                return m_IsInitialized;
            }
        }

        public bool isSupported
        {
            get
            {
                return Application.isEditor;
            }
        }

        public string version
        {
            get
            {
                return s_Version;
            }
        }

        public bool debugMode
        {
            get
            {
                return m_DebugMode;
            }
            set
            {
                m_DebugMode = value;
            }
        }

        public IBanner Banner
        {
            get
            {
                return s_Banner;
            }
        }
        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad)
        {
            m_LoadEnabled = enablePerPlacementLoad;
            m_GameId = gameId;
            if (debugMode)
            {
                Debug.Log("UnityAdsEditor: Initialize(" + gameId + ", " + testMode + ", " + enablePerPlacementLoad + ");");
            }

            var placeHolderGameObject = new GameObject("UnityAdsEditorPlaceHolderObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };
            m_Placeholder = placeHolderGameObject.AddComponent<Placeholder>();
            m_Placeholder.OnFinish += (object sender, FinishEventArgs e) =>
            {
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(sender, new FinishEventArgs(e.placementId, e.showResult));
                }

                m_CallbackExecutor.Post(execute =>
                {
                    foreach (var listener in GetClonedHashSet(_listeners))
                    {
                        listener.OnUnityAdsDidFinish(e.placementId, e.showResult);
                    }
                });
            };

            string configurationUrl = string.Join("/", new string[] {
                s_BaseUrl,
                gameId,
                string.Join("&", new string[] {
                    "configuration?platform=editor",
                    "unityVersion=" + Uri.EscapeDataString(Application.unityVersion),
                    "sdkVersionName=" + Uri.EscapeDataString(version)
                })
            });
            WebRequest request = WebRequest.Create(configurationUrl);
            request.BeginGetResponse(result =>
            {
                WebResponse response = request.EndGetResponse(result);
                var reader = new StreamReader(response.GetResponseStream());
                string responseBody = reader.ReadToEnd();
                try
                {
                    m_Configuration = new Configuration(responseBody);
                    if (!m_Configuration.enabled)
                    {
                        Debug.LogWarning("gameId " + gameId + " is not enabled");
                    }

                    m_CallbackExecutor.Post(execute =>
                    {
                        foreach (var placement in m_Configuration.placements)
                        {
                            m_PlacementMap.Add(placement.Key, false);
                            foreach (var listener in GetClonedHashSet(_listeners))
                            {
                                if (!enablePerPlacementLoad) 
                                {
                                    listener.OnUnityAdsReady(placement.Key);
                                }
                            }
                        }
                        m_IsInitialized = true;
                        foreach (var placement in m_QueuedLoads)
                        {
                            Load(placement);
                        }
                    });
                }
                catch (Exception exception)
                {
                    Debug.LogError("Failed to parse configuration for gameId: " + gameId);
                    Debug.Log(responseBody);
                    Debug.LogError(exception.Message);
                    m_CallbackExecutor.Post(execute =>
                    {
                        foreach (var listener in GetClonedHashSet(_listeners))
                        {
                            listener.OnUnityAdsDidError("Failed to parse configuration for gameId: " + gameId);
                        }
                    });   
                }
                reader.Close();
                response.Close();
            }, null);
        }

        public bool IsReady(string placementId)
        {
            if (placementId == null)
            {
                return isInitialized;
            }
            
            if (!m_LoadEnabled)
            {
                return isInitialized && m_Configuration.placements.ContainsKey(placementId);
            }

            if (m_PlacementMap.ContainsKey(placementId))
            {
                return m_PlacementMap[placementId];
            }

            return false;
        }

        public PlacementState GetPlacementState(string placementId)
        {
            if (IsReady(placementId))
            {
                return PlacementState.Ready;
            }
            return PlacementState.NotAvailable;
        }
    
        public void Load(string placementId)
        {
            // If placementId is null, use explicit defaultPlacement to match native behaviour
            if (isInitialized && placementId == null)
            {
                placementId = m_Configuration.defaultPlacement;
            }

            if (m_LoadEnabled)
            {
                if (m_PlacementMap.ContainsKey(placementId))
                {
                    m_Placeholder.Load(placementId);
                    m_PlacementMap[placementId] = true;
                    foreach (var listener in GetClonedHashSet(_listeners))
                    {
                        listener.OnUnityAdsReady(placementId);
                    }
                } 
                else
                {
                    if (isInitialized)
                    {
                        Debug.Log("Placement " + placementId + " does not exist for gameId: " + m_GameId);  
                    }
                    else
                    {
                        m_QueuedLoads.Enqueue(placementId);
                    }
                    
                }
            }
        }
    
        public void Show(string placementId)
        {
            // If placementId is null, use explicit defaultPlacement to match native behaviour
            if (isInitialized && placementId == null)
            {
                placementId = m_Configuration.defaultPlacement;
            }
            if (IsReady(placementId))
            {
                var handler = OnStart;
                if (handler != null)
                {
                    handler(this, new StartEventArgs(placementId));
                }
                m_CallbackExecutor.Post(execute =>
                {
                    foreach (var listener in GetClonedHashSet(_listeners))
                    {
                        listener.OnUnityAdsDidStart(placementId);
                    }
                });
                m_Placeholder.Show(placementId, m_Configuration.placements[placementId]);
                m_PlacementMap[placementId] = false;
            }
            else
            {
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(this, new FinishEventArgs(placementId, ShowResult.Failed));
                }
                m_CallbackExecutor.Post(execute =>
                {
                    foreach (var listener in GetClonedHashSet(_listeners))
                    {
                        listener.OnUnityAdsDidFinish(placementId, ShowResult.Failed);
                    }
                });
            }
        }

        public void SetMetaData(MetaData metaData)
        {
        }
        
        public void AddListener(IUnityAdsListener listener)
        {
            if (listener != null) {
                _listeners.Add(listener);
            }
        }

        public void RemoveListener(IUnityAdsListener listener)
        {
            if (listener != null) {
                _listeners.Remove(listener);
            }
        }

        private static HashSet<IUnityAdsListener> GetClonedHashSet(HashSet<IUnityAdsListener> hashSet)
        {
            return new HashSet<IUnityAdsListener>(hashSet);
        }
    }

#elif UNITY_ANDROID
    sealed internal class Platform : AndroidJavaProxy, IPlatform, IPurchasingEventSender
    {
        readonly IBanner m_Banner;
        readonly AndroidJavaObject m_CurrentActivity;
        readonly AndroidJavaClass m_UnityAds;
        readonly Purchase m_UnityAdsPurchase;
        readonly CallbackExecutor m_CallbackExecutor;

#if (!UNITY_2017_1_OR_NEWER)
        public int hashCode()
        {
            return GetHashCode();
        }
#endif

        void onUnityAdsReady(string placementId) {
            m_CallbackExecutor.Post((executor) => {
                foreach (var listener in GetClonedHashSet(_listeners)) {
                    listener.OnUnityAdsReady(placementId);
                }
            });
        }

        void onUnityAdsStart(string placementId)
        {
            m_CallbackExecutor.Post((executor) =>
            {
                var handler = OnStart;
                if (handler != null)
                {
                    handler(this, new StartEventArgs(placementId));
                }
                
                foreach (var listener in GetClonedHashSet(_listeners))
                {
                    listener.OnUnityAdsDidStart(placementId);
                }
            });
        }

        void onUnityAdsFinish(string placementId, AndroidJavaObject rawShowResult)
        {
            var showResult = (ShowResult)rawShowResult.Call<int>("ordinal");
            m_CallbackExecutor.Post((executor) =>
            {
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(this, new FinishEventArgs(placementId, showResult));
                }
                
                foreach (var listener in GetClonedHashSet(_listeners))
                {
                    listener.OnUnityAdsDidFinish(placementId, showResult);
                }
            });
        }

        void onUnityAdsError(AndroidJavaObject rawError, string message) {
            m_CallbackExecutor.Post((executor) => {
                foreach (var listener in GetClonedHashSet(_listeners)) {
                    listener.OnUnityAdsDidError(message);
                }
            });
        }

        public event EventHandler<StartEventArgs> OnStart;
        public event EventHandler<FinishEventArgs> OnFinish;
        
        private static HashSet<IUnityAdsListener> _listeners;

        public Platform() : base("com.unity3d.ads.IUnityAdsListener")
        {
            _listeners = new HashSet<IUnityAdsListener>();
            var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            m_CurrentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            m_UnityAds = new AndroidJavaClass("com.unity3d.ads.UnityAds");
            m_UnityAdsPurchase = new Purchase();

            var callbackExecutorGameObject = new GameObject("UnityAdsCallbackExecutorObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };
            m_CallbackExecutor = callbackExecutorGameObject.AddComponent<CallbackExecutor>();
            Object.DontDestroyOnLoad(callbackExecutorGameObject);

            m_Banner = new Banner(m_CurrentActivity, m_CallbackExecutor);
        }

        public bool isInitialized
        {
            get
            {
                return m_UnityAds.CallStatic<bool>("isInitialized");
            }
        }

        public bool isSupported
        {
            get
            {
                return m_UnityAds.CallStatic<bool>("isSupported");
            }
        }

        public string version
        {
            get
            {
                return m_UnityAds.CallStatic<string>("getVersion");
            }
        }

        public bool debugMode
        {
            get
            {
                return m_UnityAds.CallStatic<bool>("getDebugMode");
            }
            set
            {
                m_UnityAds.CallStatic("setDebugMode", value);
            }
        }

        public IBanner Banner
        {
            get
            {
                return m_Banner;
            }
        }

        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad)
        {
            m_UnityAds.CallStatic("initialize", m_CurrentActivity, gameId, this, testMode, enablePerPlacementLoad);
            m_UnityAdsPurchase.Initialize(this);
        }

        public bool IsReady(string placementId)
        {
            if (placementId == null)
            {
                return m_UnityAds.CallStatic<bool>("isReady");
            }
            return m_UnityAds.CallStatic<bool>("isReady", placementId);
        }

        public PlacementState GetPlacementState(string placementId)
        {
            AndroidJavaObject rawPlacementState;
            if (placementId == null)
            {
                rawPlacementState = m_UnityAds.CallStatic<AndroidJavaObject>("getPlacementState");
            }
            else
            {
                rawPlacementState = m_UnityAds.CallStatic<AndroidJavaObject>("getPlacementState", placementId);
            }
            return (PlacementState)rawPlacementState.Call<int>("ordinal");
        }

        public void Load(string placementId)
        {
                m_UnityAds.CallStatic("load", placementId);
        }

        public void Show(string placementId)
        {
            if (placementId == null)
            {
                m_UnityAds.CallStatic("show", m_CurrentActivity);
            }
            else
            {
                m_UnityAds.CallStatic("show", m_CurrentActivity, placementId);
            }
        }

        public void SetMetaData(MetaData metaData)
        {
            var metaDataObject = new AndroidJavaObject("com.unity3d.ads.metadata.MetaData", m_CurrentActivity);
            metaDataObject.Call("setCategory", metaData.category);
            foreach (KeyValuePair<string, object> entry in metaData.Values())
            {
                metaDataObject.Call<bool>("set", entry.Key, entry.Value);
            }
            metaDataObject.Call("commit");
        }

        public void AddListener(IUnityAdsListener listener)
        {
            if (listener != null) {
                _listeners.Add(listener);
            }
        }

        public void RemoveListener(IUnityAdsListener listener)
        {
            if (listener != null) {
                _listeners.Remove(listener);
            }
        }

        void IPurchasingEventSender.SendPurchasingEvent(string payload)
        {
            m_UnityAdsPurchase.SendEvent(payload);
        }
        
        private static HashSet<IUnityAdsListener> GetClonedHashSet(HashSet<IUnityAdsListener> hashSet)
        {
            return new HashSet<IUnityAdsListener>(hashSet);
        }
    }
#elif UNITY_IOS
    internal sealed class Platform : IPlatform
    {
        static Platform s_Instance;
        static CallbackExecutor s_CallbackExecutor;

        delegate void unityAdsReady(string placementId);
        delegate void unityAdsDidError(long rawError, string message);
        delegate void unityAdsDidStart(string placementId);
        delegate void unityAdsDidFinish(string placementId, long rawShowResult);

        [DllImport("__Internal")]
        static extern void UnityAdsInitialize(string gameId, bool testMode, bool enablePerPlacementLoad);

        [DllImport("__Internal")]
        static extern void UnityAdsLoad(string placementId);

        [DllImport("__Internal")]
        static extern void UnityAdsShow(string placementId);

        [DllImport("__Internal")]
        static extern bool UnityAdsGetDebugMode();

        [DllImport("__Internal")]
        static extern void UnityAdsSetDebugMode(bool debugMode);

        [DllImport("__Internal")]
        static extern bool UnityAdsIsSupported();

        [DllImport("__Internal")]
        static extern bool UnityAdsIsReady(string placementId);

        [DllImport("__Internal")]
        static extern long UnityAdsGetPlacementState(string placementId);

        [DllImport("__Internal")]
        static extern string UnityAdsGetVersion();

        [DllImport("__Internal")]
        static extern bool UnityAdsIsInitialized();

        [DllImport("__Internal")]
        static extern void UnityAdsSetMetaData(string category, string data);

        [DllImport("__Internal")]
        static extern void UnityAdsSetReadyCallback(unityAdsReady callback);

        [DllImport("__Internal")]
        static extern void UnityAdsSetDidErrorCallback(unityAdsDidError callback);

        [DllImport("__Internal")]
        static extern void UnityAdsSetDidStartCallback(unityAdsDidStart callback);

        [DllImport("__Internal")]
        static extern void UnityAdsSetDidFinishCallback(unityAdsDidFinish callback);

        [MonoPInvokeCallback(typeof(unityAdsReady))]
        static void UnityAdsReady(string placementId)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                foreach (var listener in GetClonedHashSet(_listeners))
                {
                    listener.OnUnityAdsReady(placementId);
                }
            });
        }

        [MonoPInvokeCallback(typeof(unityAdsDidError))]
        static void UnityAdsDidError(long rawError, string message)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                foreach (var listener in GetClonedHashSet(_listeners))
                {
                    listener.OnUnityAdsDidError(message);
                }
            });
        }

        [MonoPInvokeCallback(typeof(unityAdsDidStart))]
        static void UnityAdsDidStart(string placementId)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                var handler = s_Instance.OnStart;
                if (handler != null)
                {
                    handler(s_Instance, new StartEventArgs(placementId));
                }

                foreach (var listener in GetClonedHashSet(_listeners))
                {
                    listener.OnUnityAdsDidStart(placementId);
                }
            });
        }

        [MonoPInvokeCallback(typeof(unityAdsDidFinish))]
        static void UnityAdsDidFinish(string placementId, long rawShowResult)
        {
            s_CallbackExecutor.Post((executor) =>
            {
                var handler = s_Instance.OnFinish;
                if (handler != null)
                {
                    handler(s_Instance, new FinishEventArgs(placementId, (ShowResult)rawShowResult));
                }

                foreach (var listener in GetClonedHashSet(_listeners))
                {
                    listener.OnUnityAdsDidFinish(placementId, (ShowResult)rawShowResult);
                }
            });
        }

        public event EventHandler<StartEventArgs> OnStart;
        public event EventHandler<FinishEventArgs> OnFinish;

        private IBanner m_Banner;
        
        private static HashSet<IUnityAdsListener> _listeners;

        public Platform()
        {
            s_Instance = this;
            _listeners = new HashSet<IUnityAdsListener>();

            var callbackExecutorGameObject = new GameObject("UnityAdsCallbackExecutorObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };
            s_CallbackExecutor = callbackExecutorGameObject.AddComponent<CallbackExecutor>();
            Object.DontDestroyOnLoad(callbackExecutorGameObject);

            UnityAdsSetReadyCallback(UnityAdsReady);
            UnityAdsSetDidErrorCallback(UnityAdsDidError);
            UnityAdsSetDidStartCallback(UnityAdsDidStart);
            UnityAdsSetDidFinishCallback(UnityAdsDidFinish);
            m_Banner = new Banner(s_CallbackExecutor);
        }

        public bool isInitialized
        {
            get
            {
                return UnityAdsIsInitialized();
            }
        }

        public bool isSupported
        {
            get
            {
                return UnityAdsIsSupported();
            }
        }

        public string version
        {
            get
            {
                return UnityAdsGetVersion();
            }
        }

        public bool debugMode
        {
            get
            {
                return UnityAdsGetDebugMode();
            }
            set
            {
                UnityAdsSetDebugMode(value);
            }
        }

        public IBanner Banner
        {
            get
            {
                return m_Banner;
            }
        }

        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad)
        {
            new PurchasingPlatform().Initialize();
            UnityAdsInitialize(gameId, testMode, enablePerPlacementLoad);
        }

        public bool IsReady(string placementId)
        {
            return UnityAdsIsReady(placementId);
        }

        public PlacementState GetPlacementState(string placementId)
        {
            return (PlacementState)UnityAdsGetPlacementState(placementId);
        }

        public void Load(string placementId)
        {
            UnityAdsLoad(placementId);
        }

        public void Show(string placementId)
        {
            UnityAdsShow(placementId);
        }

        public void SetMetaData(MetaData metaData)
        {
            UnityAdsSetMetaData(metaData.category, metaData.ToJSON());
        }

        public void AddListener(IUnityAdsListener listener)
        {
            if (listener != null) {
                _listeners.Add(listener);
            }
        }

        public void RemoveListener(IUnityAdsListener listener)
        {
            if (listener != null) {
                _listeners.Remove(listener);
            }
        }
        
        private static HashSet<IUnityAdsListener> GetClonedHashSet(HashSet<IUnityAdsListener> hashSet)
        {
            return new HashSet<IUnityAdsListener>(hashSet);
        }
    }
#endif
}
