using System.Collections;
using System.Collections.Generic;
using Hawaiian.Utilities;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameEvent _event = (GameEvent) target;
        
        if (GUILayout.Button("Raise"))
            _event.Raise();
        
        base.OnInspectorGUI();
    }
}
