using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DotsBT;

namespace DotsBT.ED
{
    [CustomPropertyDrawer(typeof(GResGUID<>), true)]
    public class GResGUIDDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type_ref = fieldInfo.FieldType;
            if (typeof(System.Collections.IList).IsAssignableFrom(type_ref))
            {
                if (type_ref.IsArray)
                    type_ref = type_ref.GetElementType();
                else
                    type_ref = type_ref.GetGenericArguments()[0];
            }

            var targetObjectType = type_ref.GetGenericArguments()[0];

            var propertyX = property.FindPropertyRelative("AssetGUID.Value.x");
            var propertyY = property.FindPropertyRelative("AssetGUID.Value.y");
            var propertyZ = property.FindPropertyRelative("AssetGUID.Value.z");
            var propertyW = property.FindPropertyRelative("AssetGUID.Value.w");

            var guid = new Unity.Entities.Hash128((uint)propertyX.longValue, (uint)propertyY.longValue, (uint)propertyZ.longValue, (uint)propertyW.longValue);
            var currObject = BTBaker.EdLoadRes(guid, targetObjectType);
            var selObj = EditorGUI.ObjectField(position, label, currObject, targetObjectType, false);
            if (selObj == currObject)
                return;

            var uwr = Unity.Entities.Serialization.UntypedWeakReferenceId.CreateFromObjectInstance(selObj);
            propertyX.longValue = uwr.GlobalId.AssetGUID.Value.x;
            propertyY.longValue = uwr.GlobalId.AssetGUID.Value.y;
            propertyZ.longValue = uwr.GlobalId.AssetGUID.Value.z;
            propertyW.longValue = uwr.GlobalId.AssetGUID.Value.w;            
        }
    }

    [CustomPropertyDrawer(typeof(Unity.Entities.Content.WeakObjectReference<>), true)]
    public class WeakObjectReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type_ref = fieldInfo.FieldType;
            if (typeof(System.Collections.IList).IsAssignableFrom(type_ref))
            {
                if (type_ref.IsArray)
                    type_ref = type_ref.GetElementType();
                else
                    type_ref = type_ref.GetGenericArguments()[0];
            }

            var targetObjectType = type_ref.GetGenericArguments()[0];

            var propertyX = property.FindPropertyRelative("Id.GlobalId.AssetGUID.Value.x");
            var propertyY = property.FindPropertyRelative("Id.GlobalId.AssetGUID.Value.y");
            var propertyZ = property.FindPropertyRelative("Id.GlobalId.AssetGUID.Value.z");
            var propertyW = property.FindPropertyRelative("Id.GlobalId.AssetGUID.Value.w");

            var guid = new Unity.Entities.Hash128((uint)propertyX.longValue, (uint)propertyY.longValue, (uint)propertyZ.longValue, (uint)propertyW.longValue);
            var currObject = BTBaker.EdLoadRes(guid, targetObjectType);
            var selObj = EditorGUI.ObjectField(position, label, currObject, targetObjectType, false);
            if (selObj == currObject)
                return;

            var uwr = Unity.Entities.Serialization.UntypedWeakReferenceId.CreateFromObjectInstance(selObj);
            propertyX.longValue = uwr.GlobalId.AssetGUID.Value.x;
            propertyY.longValue = uwr.GlobalId.AssetGUID.Value.y;
            propertyZ.longValue = uwr.GlobalId.AssetGUID.Value.z;
            propertyW.longValue = uwr.GlobalId.AssetGUID.Value.w;
        }
    }
}