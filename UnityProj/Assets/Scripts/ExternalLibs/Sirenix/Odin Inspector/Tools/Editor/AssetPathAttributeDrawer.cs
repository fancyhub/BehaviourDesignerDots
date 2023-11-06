using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class AssetPathAttributeDrawer : OdinAttributeDrawer<AssetPathAttribute, string>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        var rect = Attribute.ShowPreview ? EditorGUILayout.GetControlRect(GUILayout.Height(Attribute.PreviewHeight)) : EditorGUILayout.GetControlRect();
        if (label != null)
        {
            rect = EditorGUI.PrefixLabel(rect, label);
        }
        var value = ValueEntry.SmartValue;
        var tex = AssetDatabase.LoadAssetAtPath(value, Attribute.AssetType);
        if (Attribute.ShowPreview && tex != null)
        {
            var preview = AssetPreview.GetAssetPreview(tex);
            var previewRect = rect;
            previewRect.min = new Vector2(rect.max.x - Attribute.PreviewHeight, rect.min.y);
            EditorGUI.DrawPreviewTexture(previewRect, preview);
        }
        tex = EditorGUILayout.ObjectField(tex, Attribute.AssetType, false);
        value = tex != null ? AssetDatabase.GetAssetPath(tex) : string.Empty;
        ValueEntry.SmartValue = value;
    }
}
