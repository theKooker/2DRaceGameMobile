using System;
using System.IO;
using System.Reflection;

namespace UnityEngine.Advertisements
{
    [AddComponentMenu("")]
    sealed class Placeholder : MonoBehaviour
    {
        Texture2D m_LandscapeTexture;
        Texture2D m_PortraitTexture;

        bool m_Showing;

        string m_PlacementId;
        bool m_AllowSkip;

        public event EventHandler<FinishEventArgs> OnFinish;

        public void Awake()
        {
            m_LandscapeTexture = Resources.Load("landscape") as Texture2D;
            m_PortraitTexture = Resources.Load("portrait") as Texture2D;
        }

        public void Load(string placementId)
        {
        }

        public void Show(string placementId, bool allowSkip)
        {
            m_PlacementId = placementId;
            m_AllowSkip = allowSkip;
            m_Showing = true;
        }

        public void OnGUI()
        {
            if (!m_Showing)
            {
                return;
            }
            GUI.ModalWindow(0, new Rect(0, 0, Screen.width, Screen.height), ModalWindowFunction, "");
        }

        void OnApplicationQuit()
        {
            m_Showing = false;
        }

        void ModalWindowFunction(int id)
        {
            if (m_LandscapeTexture != null && m_PortraitTexture != null)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Screen.width > Screen.height ? m_LandscapeTexture : m_PortraitTexture, ScaleMode.ScaleAndCrop);
            }
            else
            {
                GUIStyle myStyle = new GUIStyle(GUI.skin.label);
                myStyle.alignment = TextAnchor.MiddleCenter;
                myStyle.fontSize = 32;
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "This screen would be your Ad Unit", myStyle);
            }

            if (m_AllowSkip && GUI.Button(new Rect(20, 20, 150, 50), "Skip"))
            {
                m_Showing = false;
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(this, new FinishEventArgs(m_PlacementId, ShowResult.Skipped));
                }
            }

            if (GUI.Button(new Rect(Screen.width - 170, 20, 150, 50), "Close"))
            {
                m_Showing = false;
                var handler = OnFinish;
                if (handler != null)
                {
                    handler(this, new FinishEventArgs(m_PlacementId, ShowResult.Finished));
                }
            }
        }
    }
}
