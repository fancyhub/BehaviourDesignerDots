using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class WorldTimeAttributeDrawer : OdinAttributeDrawer<WorldTimeAttribute, int>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        var value = ValueEntry.SmartValue;
        if (Attribute.NeedSec)
        {
            var hh = value / 3600;
            var mm = (value / 60) % 60;
            var ss = value % 60;
            var hms = new Vector3Int(hh, mm, ss);
            hms = EditorGUILayout.Vector3IntField($"{label}(HHMMSS)", hms);
            hms.x = Math.Clamp(hms.x, 0, 23);
            hms.y = Math.Clamp(hms.y, 0, 59);
            hms.z = Math.Clamp(hms.z, 0, 59);
            ValueEntry.SmartValue = hms.x * 3600 + hms.y * 60 + hms.z;
        }
        else
        {
            var hh = value / 60;
            var mm = value % 60;
            var hm = new Vector2Int(hh,mm);
            hm = EditorGUILayout.Vector2IntField($"{label}(HHMM)", hm);
            hm.x = Math.Clamp(hm.x, 0, 23);
            hm.y = Math.Clamp(hm.y, 0, 59);
            ValueEntry.SmartValue = hm.x * 60 + hm.y;

        }
    }
}
    

