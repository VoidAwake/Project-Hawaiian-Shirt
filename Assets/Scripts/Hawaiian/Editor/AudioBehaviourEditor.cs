using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using AudioBehaviour = Hawaiian.Audio.AudioBehaviour; // alias due to UnityEngine.AudioBehaviour

namespace Hawaiian.Editor
{
    [CustomEditor(typeof(AudioBehaviour), true)]
    public class AudioBehaviourEditor : UnityEditor.Editor
    {
        private SerializedProperty _overrideVolume;
        private SerializedProperty _volumeAmount;
        private SerializedProperty _isGlobal;
        private SerializedProperty _playOnAwake;


        [SerializeField] private AudioBehaviour audioBehaviour;

        

        private void OnEnable()
        {
            _overrideVolume = serializedObject.FindProperty("overrideVolume");
            _volumeAmount = serializedObject.FindProperty("volumeOverrideAmount");
            _isGlobal = serializedObject.FindProperty("isGlobal");
            _playOnAwake = serializedObject.FindProperty("playOnAwake");

        }

        public override void OnInspectorGUI()
        {
            
            audioBehaviour = (AudioBehaviour) target;
            serializedObject.Update();
          //  EditorGUILayout.PropertyField(audioList, true);

          EditorGUILayout.BeginHorizontal();
          EditorGUILayout.LabelField("Selected Sound", GUILayout.ExpandWidth(false), GUILayout.Width(250));

          if (GUILayout.Button($"{audioBehaviour.CurrentTrack.ID}", EditorStyles.popup))
          {
              SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                  new FMODPathProvider(
                      x =>
                      {
                          audioBehaviour.CurrentTrack.ID = x.name.Split("/").Last();
                          audioBehaviour.CurrentTrack.Path = x.Path;
                          audioBehaviour.SelectedItem = x;
                      })
                  {
                      name = null,
                      hideFlags = HideFlags.None
                  });
          }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio ID");
            EditorGUILayout.TextField(audioBehaviour.CurrentTrack.ID, EditorStyles.textField);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio Path");
            EditorGUILayout.TextField(audioBehaviour.CurrentTrack.Path, EditorStyles.textField);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_playOnAwake);
            EditorGUILayout.PropertyField(_isGlobal);
            EditorGUILayout.PropertyField(_overrideVolume);
            bool overrideVolumeValue = _overrideVolume.boolValue;
            if (overrideVolumeValue)
                ShowOverrideVolumeSlider();
            
            
            serializedObject.ApplyModifiedProperties();

           // base.OnInspectorGUI();
        }

        private void ShowOverrideVolumeSlider()=> EditorGUILayout.Slider(_volumeAmount, 0f, 1f);
        
    }
}