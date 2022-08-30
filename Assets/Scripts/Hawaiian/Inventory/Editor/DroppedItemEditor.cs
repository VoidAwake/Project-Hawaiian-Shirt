using UnityEditor;

namespace Hawaiian.Inventory.Editor
{
    [CustomEditor(typeof(DroppedItem))]
    public class DroppedItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
 
            base.OnInspectorGUI();
            
            var script = target as DroppedItem;
 
            if (EditorGUI.EndChangeCheck())
            {
                script.OnItemChanged();
            }

            if (script.Item == null)
            {
                EditorGUILayout.HelpBox("An item has not been assigned. This GameObject will be disabled at runtime.", MessageType.Warning);
            }
        }
    }
}