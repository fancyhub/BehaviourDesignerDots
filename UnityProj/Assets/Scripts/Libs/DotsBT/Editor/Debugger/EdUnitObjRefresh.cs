using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Entities;
using System;
using Sirenix.OdinInspector;
using Unity.Collections;

namespace DotsBT.Debugger.ED
{
    [Serializable]
    public class EdUnitObjRefresh
    {
        [NonSerialized]
        public OdinMenuEditorWindow window;
        [Button]
        public void Refresh()
        {
            window.ForceMenuTreeRebuild();
        }
    }
}
