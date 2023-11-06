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
    public class DotsBtDebuggerWindow : OdinMenuEditorWindow
    {
        private EdUnitSelection _selection;
        public List<EdUnit> _all_units = new List<EdUnit>();        

        public EdUnitSelection EdSelection
        {
            get
            {
                if (_selection == null)
                {
                    _selection = new EdUnitSelection();
                    _selection.OnSelectionChanged = _OnUnitSelectionChanged;
                }
                return _selection;
            }
        }

        //%(ctrl on Windows and Linux, cmd on macOS), ^ (ctrl on Windows, Linux, and macOS), # (shift), & (alt).
        [MenuItem("Tools/Behaivor Designer(Dots)/Debugger %m")]
        private static void OpenWindow()
        {
            var window = GetWindow<DotsBtDebuggerWindow>();
            window.minSize = new Vector2(100, 100);
            window.maxSize = new Vector2(10000, 10000);
            window.Show();
            window.ForceMenuTreeRebuild();
        }

        protected override void OnGUI()
        {
            if (!Application.isPlaying)
            {
                if (_all_units.Count != 0)
                {
                    this.ForceMenuTreeRebuild();
                }
            }
            else
                EdSelection.Update();

            base.OnGUI();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(false);
            var config = tree.Config;
            tree.Selection.SelectionChanged += _OnSelectionChanged;

            tree.Add("刷新", new EdUnitObjRefresh() { window = this });

            _all_units = EdUnitCollector.Collect();
            foreach (var p in _all_units)
            {
                //tree.AddObjectAtPath(p.GetDisplay(), p);
                tree.Add(p.GetDisplay(), p);
            }
            return tree;
        }

        private void _OnUnitSelectionChanged(int unit_id)
        {
            //2. 判断当前Window 选中的Item 是否和目标GhostId一样
            EdUnit cur_item = this.MenuTree.Selection.SelectedValue as EdUnit;
            if (cur_item != null && cur_item.UnitID == unit_id)
                return;

            //3. 找到索引
            int index = -1;
            for (int i = 0; i < _all_units.Count; i++)
            {
                if (_all_units[i].UnitID == unit_id)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
                return;

            //4. 设置item 选中
            this.MenuTree.MenuItems[index + 1].Select();
        }

        private void _OnSelectionChanged(SelectionChangedType obj)
        {
            object cur = MenuTree.Selection.SelectedValue;
            if (!(cur is EdUnit))
                return;

            EdUnit cur_item = cur as EdUnit;
            cur_item.OnSelect();
        }         
    }  
}

