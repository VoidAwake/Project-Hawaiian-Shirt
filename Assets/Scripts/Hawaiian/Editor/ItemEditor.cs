using System;
using Codice.Client.BaseCommands;
using Hawaiian.Inventory;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Hawaiian.Editor
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : UnityEditor.Editor
    {
        
        SerializedProperty itemSprite;
        SerializedProperty itemName;
        SerializedProperty maxStacks;
        SerializedProperty itemDamage;
        SerializedProperty drawSpeed;
        SerializedProperty drawDistance;
        SerializedProperty sticksOnWall;
        SerializedProperty attackRate;
        SerializedProperty droppedItemSprite;
        SerializedProperty isMainObjective;
        SerializedProperty canBeHeldByEnemies;
        SerializedProperty points;
        SerializedProperty droppedItemBase;
        SerializedProperty projectileInstance;


        private void OnEnable()
        {
            itemName = serializedObject.FindProperty("ItemName");
            itemSprite = serializedObject.FindProperty("ItemSprite");
            maxStacks = serializedObject.FindProperty("MaxStack");
            attackRate = serializedObject.FindProperty("AttackRate");
            itemDamage = serializedObject.FindProperty("ItemDamage");
            drawSpeed = serializedObject.FindProperty("DrawSpeed");
            drawDistance = serializedObject.FindProperty("DrawDistance");
            sticksOnWall = serializedObject.FindProperty("SticksOnWall");
            droppedItemSprite = serializedObject.FindProperty("DroppedItemSprite");
            isMainObjective = serializedObject.FindProperty("IsMainObjective");
            canBeHeldByEnemies = serializedObject.FindProperty("CanBeHeldByEnemies");
            points = serializedObject.FindProperty("Points");
            droppedItemBase = serializedObject.FindProperty("DroppedItemBase");
            projectileInstance = serializedObject.FindProperty("ProjectileInstance");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            
            Item _item = (Item) target;
            
            EditorGUILayout.Space();

            ShowMainComponents();

            switch (_item.Type)
            {
                case ItemType.Melee:
                    ShowMeleeComponents();
                    break;
                case ItemType.Projectile:
                    ShowProjectileComponents();
                    break;
                case ItemType.Throwable:
                    ShowThrowableComponents();
                    break;
                case ItemType.Objective:
                    ShowObjectiveComponent();
                    break;
                case ItemType.Other:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowMainComponents()
        {
            GUILayout.Label("Main Components", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(itemName, new GUIContent("Item Name"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(itemSprite, new GUIContent("Item Sprite "));
            EditorGUILayout.PropertyField(droppedItemSprite, new GUIContent("Dropped Item Sprite "));
            EditorGUILayout.Space();
            EditorGUILayout.IntSlider(maxStacks, 0, 100, new GUIContent("Maximum Stacks", "Stacks refer to how many of this item can be in one inventory slot at a time"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(droppedItemBase, new GUIContent("Dropped Item Reference"));
            EditorGUILayout.Space();
        }

        private void ShowMeleeComponents()
        {
            GUILayout.Label("Melee Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(projectileInstance, new GUIContent("Melee Slash Reference"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(itemDamage, new GUIContent("Damage"));
            EditorGUILayout.PropertyField(attackRate, new GUIContent("Attack Rate"));
        }
        
        private void ShowProjectileComponents()
        {
            GUILayout.Label("Projectile Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(projectileInstance, new GUIContent("Projectile Instance Reference"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(itemDamage, new GUIContent("Damage"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(drawSpeed, new GUIContent("DrawSpeed","Refers to how quickly the projectile reaches its maximum distance"));
            EditorGUILayout.PropertyField(drawDistance, new GUIContent("DrawDistance","Refers to how far the projectile can reach at full charge"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sticksOnWall, new GUIContent("Sticks on Wall","Refers to if the projectile will stick on a wall"));
        }
        
        private void ShowThrowableComponents()
        {
            EditorGUILayout.PropertyField(projectileInstance, new GUIContent("Throwable Instance Reference"));
            EditorGUILayout.Space();
            GUILayout.Label("Throwable Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(itemDamage, new GUIContent("Damage"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(drawSpeed, new GUIContent("Speed","Refers to how fast the item moves through the air"));
            EditorGUILayout.PropertyField(drawDistance, new GUIContent("Distance","Refers to how far the throwable can go"));
        }
        
        private void ShowObjectiveComponent()
        {
            GUILayout.Label("Objective Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(points, new GUIContent("Points", "The amount of points/money the player will recieve if they finish the mission with this item in their inventory"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isMainObjective, new GUIContent("Is Main Objective"));
            EditorGUILayout.PropertyField(canBeHeldByEnemies, new GUIContent("Can Be Held By Enemies"));
        }
    }
}

#endif
