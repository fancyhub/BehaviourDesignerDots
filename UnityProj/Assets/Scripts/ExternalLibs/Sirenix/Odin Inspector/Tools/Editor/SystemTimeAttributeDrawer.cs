using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SystemTimeAttributeDrawer : OdinAttributeDrawer<SystemTimeAttribute, int>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        var value = ValueEntry.SmartValue;
        var dd = value / 1440;
        var hh = (value / 60) % 24; 
        var mm = value % 60; 
        var dhm = new Vector3Int(dd,hh,mm); 
        dhm = EditorGUILayout.Vector3IntField($"{label}(DDHHMM)", dhm);
        
        dhm.y = Math.Clamp(dhm.y, 0, 23); 
        dhm.z = Math.Clamp(dhm.z, 0, 59); 
        ValueEntry.SmartValue = dhm.x * 1440 + dhm.y * 60 + dhm.z;
    }
}
    

