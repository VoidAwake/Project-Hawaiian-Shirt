using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JordanTama.UI
{
    public class Settings : ScriptableObject
    {
        [SerializeField] private GameObject uiManagerPrefab;

        public static GameObject UIManagerPrefab => Instance.uiManagerPrefab;
        
        #region Singleton
        
        private const string InstanceDirectory = "UI Framework/Cache/";
        private const string InstanceFileName = "Settings";
        private const string PresetPath =
            "Packages/com.jordantama.ui-framework/Scriptable Objects/SettingsPreset.asset";

        private static Settings instance;

        public static Settings Instance
        {
            get
            {
                if (instance)
                    return instance;

                if (instance = Resources.Load<Settings>(InstanceDirectory + InstanceFileName))
                    return instance;
                
#if UNITY_EDITOR
                Settings preset = AssetDatabase.LoadAssetAtPath<Settings>(PresetPath);
                instance = preset ? Instantiate(preset) : CreateInstance<Settings>();
                instance.name = InstanceFileName;
                
                Directory.CreateDirectory(Application.dataPath + "/Resources/" + InstanceDirectory);
                AssetDatabase.CreateAsset(instance,
                    "Assets/Resources/" + InstanceDirectory + InstanceFileName + ".asset");
#endif

                return instance;
            }
        }
        
        #endregion
    }
}
