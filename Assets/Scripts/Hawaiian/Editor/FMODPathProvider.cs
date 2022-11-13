using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Hawaiian.Editor
{
    public class FMODPathProvider : ScriptableObject,ISearchWindowProvider
    {
        public Action<EditorEventRef> OnSelectedItem;

        public FMODPathProvider(Action<EditorEventRef> callback)
        {
            OnSelectedItem = callback;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("FMOD Banks"), 0));

            string[] assetGuids = AssetDatabase.FindAssets("t:editoreventref");
            List<string> paths = new List<string>();

            foreach (string guid in assetGuids)
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));

            List<EditorBankRef> groups = new List<EditorBankRef>();

            for (var j = 0; j < paths.Count; j++)
            {
                EventCache cache = AssetDatabase.LoadAssetAtPath<EventCache>(paths[j]);

                for (var k = 0; k < cache.EditorEvents.Count; k++)
                {
                    EditorEventRef item = cache.EditorEvents[k];

                    if (item.Banks.Count <= 0)
                        continue;


                    for (var i = 0; i < item.Banks.Count; i++)
                    {
                        EditorBankRef bankRef = item.Banks[i];
                        if (!groups.Contains(bankRef))
                        {
                            searchList.Add(new SearchTreeGroupEntry(new GUIContent(bankRef.Name), i + 1));
                            groups.Add(bankRef);
                        }
                    }

                    SearchTreeEntry entry =
                        new SearchTreeEntry(new GUIContent(item.Path,
                            EditorGUIUtility.ObjectContent(item, item.GetType()).image))
                        {
                            level = item.Banks.Count + 1,
                            userData = item
                        };

                    searchList.Add(entry);
                }
            }

            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            OnSelectedItem?.Invoke(SearchTreeEntry.userData as EditorEventRef);
            return true;
        }

    
    }
}