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

    public class EdUnitSelection
    {
        private EdSelection _obj_selection;
        public Action<int> OnSelectionChanged;

        public void Update()
        {
            if(_obj_selection== null)
            {
                _obj_selection = new EdSelection();
                _obj_selection.OnSelectionChanged = _OnObjSelectChange;
            }
        }

        private void _OnObjSelectChange()
        {
            //1. 根据当前的 Selection 找到 GhostId
            GameObject obj = Selection.activeObject as GameObject;
            if (obj == null)
                return;

            int unit_id = 0;
            //Get Some Component 
            //var comp = obj.GetComponent<XXUnitComp>();
            //if (comp == null)
            //    return;
            //unit_id = comp.UnitId;

            OnSelectionChanged?.Invoke(unit_id);
        }
    }
}
