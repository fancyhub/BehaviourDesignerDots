using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviorDesigner.Editor;

namespace DotsBT.ED
{
    [CustomEditor(typeof(DotsExternalBehavior))]
    public class ExternalBehaviorTreeInspector : ExternalBehaviorInspector
    {
        // intentionally left blank

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            using (var t = new SerializedObject(target))
            {
                var prop = t.FindProperty("RefDotsBTAsset");
                EditorGUILayout.PropertyField(prop);
                t.ApplyModifiedProperties();
            }

            if (GUILayout.Button("Bake"))
            {
                DotsExternalBehavior ext = target as DotsExternalBehavior;
                Bake(ext);
            }
        }

        public void Bake(DotsExternalBehavior orig)
        {
            string orig_path = AssetDatabase.GetAssetPath(orig);

            string dots_path = orig_path.Replace(".asset", "_Dots.asset");
            var dots_asset = DotsBT.BTBaker.EdGetWeakRefObj(orig.RefDotsBTAsset);

            if (dots_asset != null)
            {
                var t = dots_asset.Orig.EdLoad();
                if (t != null && t != orig)
                {
                    UnityEditor.EditorUtility.DisplayDialog("Bake 出错", "Dots的资源里面记录的Orig和自己不相同", "OK");
                    return;
                }

                dots_path = AssetDatabase.GetAssetPath(dots_asset);
            }
            else
            {
                dots_asset = ScriptableObject.CreateInstance<BTAsset>();
                AssetDatabase.CreateAsset(dots_asset, dots_path);

                //TODO: 这个可能有问题? 需要验证, 如果有问题, 需要大家重启Unity了
                dots_asset = AssetDatabase.LoadAssetAtPath<BTAsset>(dots_path);
                orig.RefDotsBTAsset = DotsBT.BTBaker.CreateWeakObjRef(dots_asset);

                EditorUtility.SetDirty(orig);
            }

            AssetDatabase.MakeEditable(dots_path);

            dots_asset.Data = BTBaker.BakeAsset(orig);
            dots_asset.Orig.EdSet(orig);
            EditorUtility.SetDirty(dots_asset);

            UnityEditor.EditorUtility.DisplayDialog("Bake", "Bake Succ", "OK");
        }     
    }
}
