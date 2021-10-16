#if UNITY_EDITOR
using System;
#elif UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;

#elif UNITY_IOS
using System;
using System.Runtime.InteropServices;
using AOT;
#endif

namespace UnityEngine.Advertisements
{
    #if UNITY_EDITOR
    [AddComponentMenu("")]
    sealed internal class Banner : MonoBehaviour, IBanner
    {

        private bool m_showing;
        private string m_placementId;
        private bool loaded { get; set; }
        private BannerPosition currentBannerPosition;
        public Texture2D aTexture;

        private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
        private BannerOptions m_ShowOptions;

        public void Awake()
        {
            m_showing = false;
            aTexture = backgroundTexture(320, 50, Color.grey);
        }

        public bool isLoaded => loaded;

        /// <summary>
        /// Loads the banner ad with a specified <a href="../manual/MonetizationPlacements.html">Placement</a>, and no callbacks.
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        public void Load(string placementId, BannerLoadOptions loadOptions)
        {
            loaded = true;
            currentBannerPosition = bannerPosition;
            loadOptions?.loadCallback();
        }

        /// <summary>
        /// Allows you to hide a banner ad, instead of destroying it altogether.
        /// </summary>
        public void Hide(bool destroy = false)
        {
            m_showing = false;
            if (destroy)
            {
                loaded = false;
                return;
            }
            m_ShowOptions?.hideCallback();
        }

        /// <summary>
        /// Displays the banner ad with a specified <a href="../manual/MonetizationPlacements.html">Placement</a>, and no callbacks.
        /// </summary>
        /// <param name="placementId">The unique identifier for a specific Placement, found on the <a href="https://operate.dashboard.unity3d.com/">developer dashboard</a>.</param>
        public void Show(string placementId, BannerOptions showOptions)
        {
            m_ShowOptions = showOptions;
            if (!loaded)
            {
                Load(placementId, null);
            }
            m_placementId = placementId;
            m_showing = true;
            showOptions?.showCallback();
        }

        /// <summary>
        /// <para>Sets the position of the banner ad, using the <a href="../api/UnityEngine.Advertisements.BannerPosition.html"><c>BannerPosition</c></a> enum.</para>
        /// <para>Banner position defaults to <c>BannerPosition.BOTTOM_CENTER</c>.</para>
        /// </summary>
        /// <param name="position">An enum representing the on-screen anchor position of the banner ad.</param>
        public void SetPosition(BannerPosition position)
        {
            bannerPosition = position;
        }

        void OnGUI()
        {
            if (!this.m_showing)
            {
                return;
            }
            GUIStyle myStyle = new GUIStyle(GUI.skin.box);
            myStyle.alignment = TextAnchor.MiddleCenter;
            myStyle.fontSize = 20;
            
            if (GUI.Button(getBannerRect(currentBannerPosition), aTexture))
            {
                m_ShowOptions?.clickCallback();
            }
    
            if(aTexture){
                GUI.DrawTexture(getBannerRect(currentBannerPosition), aTexture, ScaleMode.ScaleToFit);
                GUI.Box(getBannerRect(currentBannerPosition), "This would be your banner", myStyle);
            }
        }

        void OnApplicationQuit()
        {
            m_showing = false;
        }

        private Texture2D backgroundTexture(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private Rect getBannerRect(BannerPosition position)
        {
            switch(position)
            {
                case BannerPosition.TOP_CENTER:
                    return (new Rect(Screen.width / 2 - 160, 0, 320, 50));
                case BannerPosition.TOP_LEFT:
                    return (new Rect(0, 0, 320, 50));
                case BannerPosition.TOP_RIGHT:
                    return (new Rect(Screen.width - 320, 0, 320, 50));
                case BannerPosition.CENTER:
                    return (new Rect(Screen.width / 2 -160, Screen.height / 2 - 25, 320, 50));
                case BannerPosition.BOTTOM_CENTER:
                    return (new Rect(Screen.width / 2 -160, Screen.height - 50, 320, 50));
                case BannerPosition.BOTTOM_LEFT:
                    return (new Rect(0, Screen.height - 50, 320, 50));
                case BannerPosition.BOTTOM_RIGHT:    
                    return (new Rect(Screen.width - 320, Screen.height - 50, 320, 50));
                default:
                    return (new Rect(Screen.width / 2 - 160, Screen.height - 50, 320, 50));
            }
        }
    }
    #elif UNITY_ANDROID
    static class Color
    {
        public const int Transparent = 0;
    }

    sealed internal class Banner : AndroidJavaProxy, IBanner
    {
        private AndroidJavaClass m_BannersClass;
        private AndroidJavaObject m_CurrentActivity;
        private CallbackExecutor m_CallbackExecutor;

        private BannerBundle m_BannerBundle;
        private bool m_showAfterLoad;
        private BannerOptions m_ShowOptions;
        private BannerLoadOptions m_LoadOptions;

        public Banner(AndroidJavaObject currentActivity, CallbackExecutor callbackExecutor) : base("com.unity3d.services.banners.IUnityBannerListener")
        {
            m_BannersClass = new AndroidJavaClass("com.unity3d.services.banners.UnityBanners");
            m_CurrentActivity = currentActivity;
            m_CallbackExecutor = callbackExecutor;
            m_BannerBundle = null;
            m_BannersClass.CallStatic("setBannerListener", this);
        }

        public bool isLoaded => m_BannerBundle != null;

        public void Load(string placementId, BannerLoadOptions loadOptions)
        {
            m_LoadOptions = loadOptions;
            if (m_BannerBundle != null && m_BannerBundle.bannerPlacementId.Equals(placementId))
            {
                if (loadOptions?.loadCallback != null)
                {
                    EventHandler<EventArgsWithPlacementId> handler = (object sender, EventArgsWithPlacementId args) =>
                    {
                        loadOptions.loadCallback();
                    };
                    m_CallbackExecutor.Post((executor) =>
                    {
                        handler(this, new EventArgsWithPlacementId(placementId));
                    });
                    
                }
            }
            else
            {
                if (m_BannerBundle != null)
                {
                    Hide(true); 
                    m_BannerBundle = null;
                }
                if (placementId != null)
                {
                    m_BannersClass.CallStatic("loadBanner", m_CurrentActivity, placementId);
                }
                else
                {
                    m_BannersClass.CallStatic("loadBanner", m_CurrentActivity);
                }
            }
        }

        public void Hide(bool destroy = false)
        {
            if (m_BannerBundle != null)
            {
                if (destroy)
                {
                    m_BannerBundle = null;
                    m_BannersClass.CallStatic("destroy");
                }
                else
                {
                    m_CurrentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                        var parent = m_BannerBundle.bannerView.Call<AndroidJavaObject>("getParent");
                        parent?.Call("removeView", m_BannerBundle.bannerView);
                    }));
                    if (m_ShowOptions?.hideCallback != null)
                    {
                        EventHandler<EventArgsWithPlacementId> handler = (object sender, EventArgsWithPlacementId args) =>
                        {
                            m_ShowOptions.hideCallback();
                        };
                        m_CallbackExecutor.Post((executor) =>
                        {
                            handler(this, new EventArgsWithPlacementId(m_BannerBundle == null? string.Empty : m_BannerBundle.bannerPlacementId));
                        });
                    }
                }
            }
        }

        public void Show(string placementId, BannerOptions showOptions)
        {
            m_ShowOptions = showOptions;
            if (m_BannerBundle != null && (string.IsNullOrEmpty(placementId) || m_BannerBundle.bannerPlacementId.Equals(placementId)))
            {
                m_CurrentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    var parent = m_BannerBundle.bannerView.Call<AndroidJavaObject>("getParent");
                    if (parent == null)
                    {
                        var layoutParams = m_BannerBundle.bannerView.Call<AndroidJavaObject>("getLayoutParams");
                        m_CurrentActivity.Call("addContentView", m_BannerBundle.bannerView, layoutParams);
                    }
                }));
                if (m_ShowOptions?.showCallback != null)
                {
                    EventHandler<EventArgsWithPlacementId> handler = (object sender, EventArgsWithPlacementId args) =>
                    {
                        m_ShowOptions.showCallback();
                    };
                    m_CallbackExecutor.Post((executor) =>
                    {
                        handler(this, new EventArgsWithPlacementId(placementId));
                    });
                }
            }
            else
            {
                if (m_BannerBundle != null)
                {
                    Hide(true);
                    m_BannerBundle = null;
                }
                m_showAfterLoad = true;
                Load(placementId, null);
            }
        }

        public void SetPosition(BannerPosition position)
        {
                var index = (int)position;
                var enumClass = new AndroidJavaClass("com.unity3d.services.banners.view.BannerPosition");
                var values = enumClass.CallStatic<AndroidJavaObject>("values");
                var bannerPosition = new AndroidJavaClass("java.lang.reflect.Array").CallStatic<AndroidJavaObject>("get", values, index);

                m_BannersClass.CallStatic("setBannerPosition", bannerPosition);
        }

        void onUnityBannerShow(string placementId)
        {
        }

        void onUnityBannerHide(string placementId)
        {
        }

        void onUnityBannerLoaded(String placementId, AndroidJavaObject view)
        {
            m_BannerBundle = new BannerBundle(placementId, view);
            view.Call("setBackgroundColor", Color.Transparent);
            if (m_showAfterLoad)
            {
                m_showAfterLoad = false;
                var layoutParams = view.Call<AndroidJavaObject>("getLayoutParams");
                m_CurrentActivity.Call("addContentView", view, layoutParams);
                if (m_ShowOptions?.showCallback != null)
                {
                    EventHandler<EventArgsWithPlacementId> showHandler = (object sender, EventArgsWithPlacementId args) =>
                    {
                        m_ShowOptions.showCallback();
                    };
                    m_CallbackExecutor.Post((executor) =>
                    {
                        showHandler(this, new EventArgsWithPlacementId(placementId));
                    });
                }
            }

            if (m_LoadOptions?.loadCallback != null)
            {
                EventHandler<EventArgsWithPlacementId> loadHandler = (object sender, EventArgsWithPlacementId args) =>
                {
                    m_LoadOptions.loadCallback();
                };
                m_CallbackExecutor.Post((executor) =>
                {
                    loadHandler(this, new EventArgsWithPlacementId(placementId));
                });
            }
        }

        void onUnityBannerUnloaded(String placementId)
        {
        }

        void onUnityBannerClick(string placementId)
        {
            if (m_ShowOptions?.clickCallback != null)
            {
                EventHandler<EventArgsWithPlacementId> clickHandler = (object sender, EventArgsWithPlacementId args) =>
                {
                    m_ShowOptions.clickCallback();
                };
                m_CallbackExecutor.Post((executor) =>
                {
                    clickHandler(this, new EventArgsWithPlacementId(placementId));
                });
            }
        }

        void onUnityBannerError(string message)
        {
            if (m_LoadOptions?.errorCallback != null)
            {
                EventHandler<ErrorEventArgs> errorHandler = (object sender, ErrorEventArgs args) =>
                {
                    m_LoadOptions.errorCallback(args.message);
                };
                m_CallbackExecutor.Post((executor) =>
                {
                    errorHandler(this, new ErrorEventArgs(message));
                });
            }
        }

        class BannerBundle
        {
            public AndroidJavaObject bannerView { get; }
            public String bannerPlacementId { get; }

            public BannerBundle(String bannerPlacementId, AndroidJavaObject bannerView)
            {
                this.bannerPlacementId = bannerPlacementId;
                this.bannerView = bannerView;
            }
        }
    }
    #elif UNITY_IOS
    sealed internal class Banner : IBanner
    {
        static Banner s_Instance;
        static CallbackExecutor s_CallbackExecutor;
        private static BannerLoadOptions s_LoadOptions;
        private static BannerOptions s_ShowOptions;
        private static string s_PlacementId;

        delegate void unityAdsBannerShow(string placementId);
        delegate void unityAdsBannerHide(string placementId);
        delegate void unityAdsBannerClick(string placementId);
        delegate void unityAdsBannerUnload(string placementId);
        delegate void unityAdsBannerLoad(string placementId);
        delegate void unityAdsBannerError(string message);

        [DllImport("__Internal")]
        static extern void UnityAdsBannerShow(string placementId, bool showAfterLoad);
        [DllImport("__Internal")]
        static extern void UnityAdsBannerHide(bool shouldDestroy);
        [DllImport("__Internal")]
        static extern bool UnityAdsBannerIsLoaded();
        [DllImport("__Internal")]
        static extern void UnityAdsBannerSetPosition(int position);

        [DllImport("__Internal")]
        static extern void UnityAdsSetBannerShowCallback(unityAdsBannerShow callback);
        [DllImport("__Internal")]
        static extern void UnityAdsSetBannerHideCallback(unityAdsBannerHide callback);
        [DllImport("__Internal")]
        static extern void UnityAdsSetBannerClickCallback(unityAdsBannerClick callback);
        [DllImport("__Internal")]
        static extern void UnityAdsSetBannerErrorCallback(unityAdsBannerError callback);
        [DllImport("__Internal")]
        static extern void UnityAdsSetBannerUnloadCallback(unityAdsBannerUnload callback);
        [DllImport("__Internal")]
        static extern void UnityAdsSetBannerLoadCallback(unityAdsBannerLoad callback);
        [DllImport("__Internal")]
        static extern void UnityBannerInitialize();

        public Banner(CallbackExecutor callbackExecutor)
        {
            s_CallbackExecutor = callbackExecutor;
            s_Instance = this;

            UnityAdsSetBannerShowCallback(UnityAdsBannerDidShow);
            UnityAdsSetBannerHideCallback(UnityAdsBannerDidHide);
            UnityAdsSetBannerClickCallback(UnityAdsBannerClick);
            UnityAdsSetBannerErrorCallback(UnityAdsBannerDidError);
            UnityAdsSetBannerUnloadCallback(UnityAdsBannerDidUnload);
            UnityAdsSetBannerLoadCallback(UnityAdsBannerDidLoad);
            UnityBannerInitialize();
        }

        public bool isLoaded => UnityAdsBannerIsLoaded();

        public void Load(string placementId, BannerLoadOptions loadOptions)
        {
            s_LoadOptions = loadOptions;
            if (!string.IsNullOrEmpty(s_PlacementId) && !s_PlacementId.Equals(placementId))
            {
                Hide(true); 
            }
            UnityAdsBannerShow(placementId, false);
        }

        public void Show(string placementId, BannerOptions showOptions)
        {
            s_ShowOptions = showOptions;
            s_LoadOptions = null;
            if (!string.IsNullOrEmpty(s_PlacementId) && !s_PlacementId.Equals(placementId))
            {
                Hide(true); 
            }
            UnityAdsBannerShow(placementId, true);
        }

        public void Hide(bool destroy = false)
        {
            UnityAdsBannerHide(destroy);
            if (!destroy)
            {
                UnityAdsBannerDidHide(string.Empty);
            }
        }

        public void SetPosition(BannerPosition position){
            UnityAdsBannerSetPosition((int)position);
        }

        [MonoPInvokeCallback(typeof(unityAdsBannerShow))]
        static void UnityAdsBannerDidShow(string placementId)
        {
            if (s_ShowOptions?.showCallback != null)
            {
                EventHandler<EventArgsWithPlacementId> handler = (object sender, EventArgsWithPlacementId args) =>
                {
                    s_ShowOptions.showCallback();
                };
                s_CallbackExecutor.Post((executor) =>
                {
                    handler(s_Instance, new EventArgsWithPlacementId(placementId));
                });
            }
        }

        [MonoPInvokeCallback(typeof(unityAdsBannerHide))]
        static void UnityAdsBannerDidHide(string placementId)
        {
            if (s_ShowOptions?.hideCallback != null)
            {
                EventHandler<EventArgsWithPlacementId> handler = (object sender, EventArgsWithPlacementId args) =>
                {
                    s_ShowOptions.hideCallback();
                };
                s_CallbackExecutor.Post((executor) =>
                {
                    handler(s_Instance, new EventArgsWithPlacementId(string.Empty));
                });
            }
        }

        [MonoPInvokeCallback(typeof(unityAdsBannerClick))]
        static void UnityAdsBannerClick(string placementId)
        {
            if (s_ShowOptions?.clickCallback != null)
            {
                EventHandler<EventArgsWithPlacementId> clickHandler = (object sender, EventArgsWithPlacementId args) =>
                {
                    s_ShowOptions.clickCallback();
                };
                s_CallbackExecutor.Post((executor) =>
                {
                    clickHandler(s_Instance, new EventArgsWithPlacementId(placementId));
                });
            }
        }

        [MonoPInvokeCallback(typeof(unityAdsBannerError))]
        static void UnityAdsBannerDidError(string message)
        {
            if (s_LoadOptions?.errorCallback != null)
            {
                EventHandler<ErrorEventArgs> errorHandler = (object sender, ErrorEventArgs args) =>
                {
                    s_LoadOptions.errorCallback(args.message);
                };
                s_CallbackExecutor.Post((executor) =>
                {
                    errorHandler(s_Instance, new ErrorEventArgs(message));
                });
            }
        }

        [MonoPInvokeCallback(typeof(unityAdsBannerUnload))]
        static void UnityAdsBannerDidUnload(string placementId)
        {
        }

        [MonoPInvokeCallback(typeof(unityAdsBannerUnload))]
        static void UnityAdsBannerDidLoad(string placementId)
        {
            s_PlacementId = placementId;
            if (s_LoadOptions?.loadCallback != null)
            {
                EventHandler<EventArgsWithPlacementId> loadHandler = (object sender, EventArgsWithPlacementId args) =>
                {
                    s_LoadOptions.loadCallback();
                };
                s_CallbackExecutor.Post((executor) =>
                {
                    loadHandler(s_Instance, new EventArgsWithPlacementId(placementId));
                });
            }
        }
    }
    #endif
}
