using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DotsBT.ED
{
    [CustomEditor(typeof(DotsBehaviorTree))]
    public class DotsBehaviorTreeInspector : BehaviorDesigner.Editor.BehaviorInspector
    {
        // intentionally left blank

        public override void OnInspectorGUI()
        {
            DotsBehaviorTree tar = (DotsBehaviorTree)target;
            if (Application.isPlaying)
                EditorGUILayout.Toggle("UseOrig", tar.UseOrig);
            else
                tar.UseOrig = EditorGUILayout.Toggle("UseOrig", tar.UseOrig);

            EditorGUILayout.ObjectField("DotsGlobalVar",tar.GlobalVar, typeof(DotsGlobalVar),false);
            base.OnInspectorGUI();
        }
    }
}
