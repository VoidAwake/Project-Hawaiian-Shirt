using System;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Hawaiian.UI.General
{
    public class DynamicGridCellSize : DialogueComponent<Dialogue>
    {
        [SerializeField] private GridLayoutGroup grid;
        
        protected override void OnComponentAwake()
        {
            base.OnComponentAwake();

            var rect = grid.GetComponent<RectTransform>().rect;
            
            switch (grid.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    grid.cellSize = new Vector2(rect.width / grid.constraintCount - grid.spacing.x,
                        rect.height / Mathf.Ceil((float) grid.transform.childCount / grid.constraintCount) - grid.spacing.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{grid.constraint} has not been implemented.");
            }
        }

        protected override void Subscribe() { }

        protected override void Unsubscribe() { }
    }
}