using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotsBT
{
    [Serializable]
    public class DotsBTGraphViewConfig : UnityEngine.ScriptableObject
    {
        public const string CPath = "Assets/Designer/DotsBT/DotsBTGraphViewConfig/DotsBTGraphViewConfig.asset";

#if UNITY_EDITOR
        private static DotsBTGraphViewConfig _inst;
        public static DotsBTGraphViewConfig Inst
        {
            get
            {
                if (_inst != null) 
                    return _inst;
                _inst = UnityEditor.AssetDatabase.LoadAssetAtPath<DotsBTGraphViewConfig>(CPath);
                if (_inst == null)
                    Debug.LogError("Load GraphView Setting Error: "  + CPath);
                return _inst;
            }
        }
#endif

        [Min(60)]
        public float NodeWidth = 100;
        [Min(60)]
        public float NodeHeight = 100;
        [Min(0)]
        public float BorderWidth = 5;
        [Min(30)]
        public float BorderHeight = 60;

        public Vector2 NodeSize => new Vector2(NodeWidth, NodeHeight);
        public Vector2 NodeFullSize => new Vector2(NodeWidth + BorderWidth, NodeHeight + BorderHeight);

        public GUIStyle Inactive;
        public GUIStyle Failure;
        public GUIStyle Success;
        public GUIStyle Running;

        public Rect BtnRect;
        public GUIStyle BtnPlus;
        public GUIStyle BtnMinus;


        public GUIStyle this[EBTStatus status]
        {
            get
            {
                switch (status)
                {
                    default: return Inactive;
                    case EBTStatus.Failure: return Failure;
                    case EBTStatus.Success: return Success;
                    case EBTStatus.Inactive: return Inactive;
                    case EBTStatus.Running: return Running;
                }
            }
        }
    }
}
