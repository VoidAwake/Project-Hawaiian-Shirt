using UnityEditor;
using UnityEngine;

namespace JordanTama.UI.Editor.Editor
{
    public class SettingsWindow : EditorWindow
    {
        private UnityEditor.Editor editor;
        
        [MenuItem("JordanTama/UI Framework Settings")]
        private static void ShowWindow()
        {
            SettingsWindow window = GetWindow<SettingsWindow>();
            window.titleContent =
                new GUIContent("UI Framework Settings", EditorGUIUtility.IconContent("Settings").image);
            window.Show();
        }

        private void OnEnable() => editor = UnityEditor.Editor.CreateEditor(Settings.Instance);

        private void OnGUI() => editor.OnInspectorGUI();
    }
}
