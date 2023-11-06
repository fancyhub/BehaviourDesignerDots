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
    public sealed class EdSelection
    {
        public Action OnSelectionChanged;
        private UnityEngine.Object _cur;
        public EdSelection()
        {
            _cur = Selection.activeObject;
        }

        public void Update()
        {
            if (_cur == Selection.activeObject)
                return;

            _cur = Selection.activeObject;
            OnSelectionChanged?.Invoke();
        }
    }
}
