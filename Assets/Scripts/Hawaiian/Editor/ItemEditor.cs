using System;
using Codice.Client.BaseCommands;
using Hawaiian.Inventory;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

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
        SerializedProperty returnToPlayer;
        SerializedProperty multishot;
        SerializedProperty multishotAmount;
        SerializedProperty trapPlacementRadius;
        SerializedProperty trapPlacementIcon;
        SerializedProperty trapInstance;
        SerializedProperty isKey;
        SerializedProperty otherInstance;
        SerializedProperty IsRicochet;
        SerializedProperty MaximumBounces;
        SerializedProperty Rarity;
        SerializedProperty KnockbackDistance;
        SerializedProperty heldItemPrefab;
        SerializedProperty ParryWindow;
        SerializedProperty TimeTillParry;
        SerializedProperty ParryPercentage;
        SerializedProperty ShieldUp;
        SerializedProperty ShieldDown;


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
            isKey = serializedObject.FindProperty("IsKey");
            returnToPlayer = serializedObject.FindProperty("ReturnsToPlayer");
            multishot = serializedObject.FindProperty("IsMultiShot");
            multishotAmount = serializedObject.FindProperty("ProjectileAmount");
            trapPlacementIcon = serializedObject.FindProperty("PlacementIcon");
            trapPlacementRadius = serializedObject.FindProperty("PlacementRadius");
            trapInstance = serializedObject.FindProperty("TrapInstance");
            IsRicochet = serializedObject.FindProperty("IsRicochet");
            MaximumBounces = serializedObject.FindProperty("MaximumBounces");
            Rarity = serializedObject.FindProperty("Rarity");
            KnockbackDistance = serializedObject.FindProperty("KnockbackDistance");
            heldItemPrefab = serializedObject.FindProperty("heldItemPrefab");
            ParryWindow = serializedObject.FindProperty("ParryWindow");
            TimeTillParry = serializedObject.FindProperty("TimeTillParry");
            ParryPercentage = serializedObject.FindProperty("ParryPercentage");
            ShieldUp = serializedObject.FindProperty("ShieldUp");
            ShieldDown = serializedObject.FindProperty("ShieldDown");

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
                    ShowProjectileComponents(_item);
                    break;
                case ItemType.Throwable:
                    ShowThrowableComponents();
                    break;
                case ItemType.Objective:
                    ShowObjectiveComponent();
                    break;
                case ItemType.Trap:
                    ShowTrapComponents();
                    break;
                case ItemType.Other:
                    ShowOtherComponent();
                    break;
                case ItemType.Shield:
                    ShowShieldComponents();
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
            EditorGUILayout.IntSlider(maxStacks, 0, 100,
                new GUIContent("Maximum Stacks",
                    "Stacks refer to how many of this item can be in one inventory slot at a time"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(droppedItemBase, new GUIContent("Dropped Item Reference"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(Rarity, new GUIContent("Item Rarity"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(heldItemPrefab);
        }

        private void ShowTrapComponents()
        {
            GUILayout.Label("Trap Stats", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(itemDamage, new GUIContent("Damage"));
            EditorGUILayout.IntSlider(trapPlacementRadius, 0, 30,
                new GUIContent("Placement Radius", "Refers to how far the placement time "));
            EditorGUILayout.Space();
            GUILayout.Label("Trap Components", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(trapPlacementIcon, new GUIContent("Placement Icon"));
            EditorGUILayout.PropertyField(trapInstance, new GUIContent("Trap Instance "));
        }

        private void ShowShieldComponents()
        {
            GUILayout.Label("Shield Stats", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(ParryWindow, new GUIContent("Parry Window", "How long the shield parry will activate for, the longer the easier it is to parry"));
            EditorGUILayout.IntSlider(ParryPercentage, 0, 100, new GUIContent("Parry Percentage",
                "Refers to the percentage difference needed between a perfect and standard parry for example, " +
                "a parry percentage of 20% means that the parry much be activated within the first 20% of the parry window time to be considered a perfect parry. Reccomended between 5-30%"));
            EditorGUILayout.PropertyField(TimeTillParry, new GUIContent("Time Until Next Parry", "How long the shield is ready again after being used"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(ShieldUp, new GUIContent("Shield Up Sprite"));
            EditorGUILayout.PropertyField(ShieldDown, new GUIContent("Shield Down Sprite"));



        }

        private void ShowMeleeComponents()
        {
            GUILayout.Label("Melee Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(projectileInstance, new GUIContent("Melee Slash Reference"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(itemDamage, new GUIContent("Damage"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(attackRate, new GUIContent("Attack Rate"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(KnockbackDistance, new GUIContent("Knockback Distance"));
        }

        private void ShowProjectileComponents(Item item)
        {
            GUILayout.Label("Projectile Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(projectileInstance, new GUIContent("Projectile Instance Reference"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(itemDamage, new GUIContent("Damage"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(drawSpeed,
                new GUIContent("DrawSpeed", "Refers to how quickly the projectile reaches its maximum distance"));
            EditorGUILayout.PropertyField(drawDistance,
                new GUIContent("DrawDistance", "Refers to how far the projectile can reach at full charge"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sticksOnWall,
                new GUIContent("Sticks on Wall", "Refers to if the projectile will stick on a wall"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(returnToPlayer,
                new GUIContent("Returns to the player?",
                    "If the projectile does not hit a unit or obstacle in its initial throw, the projectile will return to the player"));

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(multishot,
                new GUIContent("Is A Multishot", "Refers to if the item will produce multiple shots at once"));

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(IsRicochet,
                new GUIContent("Is Ricochet", "Refers to if the item will bounce of wall"));

            if (item.IsMultiShot)
            {
                GUILayout.Label("Multishot Stats", EditorStyles.boldLabel);
                EditorGUILayout.IntSlider(multishotAmount, 2, 20,
                    new GUIContent("Number of Projectiles", "The amount of projectiles spawned by the item when used"));
            }
            else
            {
                item.ProjectileAmount = 0;
            }

            if (item.IsRicochet)
            {
                GUILayout.Label("Ricochet Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(MaximumBounces,
                    new GUIContent("Maximum Bounces",
                        "Refers to the max amount of time an item can bounce off the wall"));
            }
        }

        private void ShowThrowableComponents()
        {
            EditorGUILayout.PropertyField(projectileInstance, new GUIContent("Throwable Instance Reference"));
            EditorGUILayout.Space();
            GUILayout.Label("Throwable Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(itemDamage, new GUIContent("Damage"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(drawSpeed,
                new GUIContent("Speed", "Refers to how fast the item moves through the air"));
            EditorGUILayout.PropertyField(drawDistance,
                new GUIContent("Distance", "Refers to how far the throwable can go"));
        }

        private void ShowObjectiveComponent()
        {
            GUILayout.Label("Objective Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(points,
                new GUIContent("Points",
                    "The amount of points/money the player will recieve if they finish the mission with this item in their inventory"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isMainObjective, new GUIContent("Is Main Objective"));
            EditorGUILayout.PropertyField(canBeHeldByEnemies, new GUIContent("Can Be Held By Enemies"));
        }

        private void ShowOtherComponent()
        {
            GUILayout.Label("Other Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(isKey, new GUIContent("Key"));
        }
    }
}

#endif