using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class LockerAttributeDrawer : OdinAttributeDrawer<LockerAttribute, uint>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        var rect = EditorGUILayout.GetControlRect();
        if (label != null)
        {
            rect = EditorGUI.PrefixLabel(rect, label);
        }
        var value = ValueEntry.SmartValue;
        Attribute.isLock = EditorGUI.Toggle(rect.AlignLeft(rect.width * 0.1f), "", Attribute.isLock);
        GUI.enabled = Attribute.isLock;
        value = (uint)SirenixEditorFields.IntField(rect.AlignRight(rect.width * 0.9f), (int)value);
        GUI.enabled = true;
        ValueEntry.SmartValue = value;
    }
}
