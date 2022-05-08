using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawaiian.PositionalEvents
{
    [CreateAssetMenu(menuName = "Hawaiian/ControlSpriteMappings")]
    public class ControlSpriteMappings : ScriptableObject
    {
        [Serializable]
        private struct ControlSpriteMapping
        {
            public string controlPath;
            public Sprite sprite;
        }

        [SerializeField] private List<ControlSpriteMapping> controlSpriteMappingsList;

        public Dictionary<string, Sprite> controlSpriteMappings =>
            controlSpriteMappingsList.ToDictionary(a => a.controlPath, a => a.sprite);
    }
}