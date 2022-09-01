using UnityEditor;
using UnityEngine.UI;

namespace Hawaiian.Editor
{
    [CustomEditor(typeof(Slider),true)]
    public class ExtendedSliderEditor : UnityEditor.Editor
    {
        UnityEditor.Editor defaultEditor;
        private Slider slider;

        
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
