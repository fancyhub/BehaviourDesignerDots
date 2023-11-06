using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Entities;
using System;
using Sirenix.OdinInspector;
using Unity.Collections;

namespace DotsBT.ED
{

    public static class DotsBtDebuggerCollector
    {
        public static List<EdBTUnit> Collect()
        {
            Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp;
            EntityQueryBuilder entity_builder = new EntityQueryBuilder(allocator)
                                                .WithAll<DotsBT.BTAssetCompData>();
            List<EdBTUnit> ret = new();
            foreach (var world in World.All)
            {
                NativeArray<Entity> e_list = entity_builder.Build(world.EntityManager).ToEntityArray(allocator);
                foreach (var e in e_list)
                {
                    BTAssetCompData asset_comp_data = world.EntityManager.GetComponentData<BTAssetCompData>(e);

                    var unit = EdBTUnit.Create(0, world, e,null);
                    unit.Asset = BTBaker.EdGetWeakRefObj(asset_comp_data.Image);
                    unit.HasRunData = world.EntityManager.HasComponent<BTComponentRunTimeData>(e);
                    if (unit.HasRunData)
                    {
                        unit.HasRunData = world.EntityManager.GetComponentData<BTComponentRunTimeData>(e).Valid;
                    }
                    ret.Add(unit);
                }
            }
            return ret;
        }
    }


    [Serializable]
    public class EdBTUnit
    {
        public int UnitID;

        [Space]
        public int EntityId;
        public UnityEngine.ScriptableObject EntityProxy;
        public GameObject GameObject;
        public World World;
        public Entity Entity;
        public BTAsset Asset;
        [HideInInspector]
        public bool HasRunData;

        [TableList]
        public List<EdBTBlackBoardValue> BlackBoardValues = new List<EdBTBlackBoardValue>();


        public static EdBTUnit Create(int unit_id, World world, Entity e, GameObject gameObject)
        {
            EdBTUnit ret = new EdBTUnit();
            ret.UnitID = unit_id;

            ret.EntityId = e.Index;
            ret.EntityProxy = EntitySelectionProxy.CreateInstance(world, e);
            ret.GameObject = gameObject;
            ret.World = world;
            ret.Entity = e;             
            return ret;
        }

        public void OnSelect()
        {
            if (GameObject == null)
                return;
            //EditorGUIUtility.PingObject(ClientGameObject);

            Selection.activeGameObject = GameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        public string GetDisplay()
        {
            return $"GhostId: {UnitID}";
        }

        public bool IsValid()
        {
            if (World == null || !World.IsCreated)
                return false;

            EntityManager mgr = World.EntityManager;
            return mgr.Exists(Entity);
        }

        public bool TryGetServerWorldComp<T>(out T out_comp_data) where T : unmanaged, IComponentData
        {
            if (!World.EntityManager.HasComponent<T>(Entity))
            {
                out_comp_data = default(T);
                return false;
            }

            out_comp_data = World.EntityManager.GetComponentData<T>(Entity);
            return true;
        }


        [HorizontalGroup("A")]
        [Button("打开行为树")]
        public void OpenEntityBTRuntimeDebugWindow2()
        {
            EdBTGraphView.ShowGraph(World, Entity);
        }

        [HorizontalGroup("A")]
        [Button("刷新变量")]
        public void RefreshValues()
        {
            BlackBoardValues.Clear();

            if (!IsValid())
                return;

            if (!TryGetServerWorldComp(out BTComponentRunTimeData comp_data))
                return;

            List<BTBBBoxedValue> temp = new List<BTBBBoxedValue>();
            comp_data.BlackBoard.EdGetBoxedValues(temp);

            foreach (var p in temp)
            {
                var item = new EdBTBlackBoardValue()
                {
                    Scope = p.Scope,
                    Value = p.Value,
                    Used = true,
                };

                item.Name = _FindShareName(Asset, p.NameId);
                if (item.Name == null)
                {
                    item.Used = false;
                    if (item.Scope == EBTVarScope.Global)
                        item.Name = _FindGlobalVarName(p.NameId);
                    if (item.Name == null)
                        item.Name = p.NameId.ToString();
                }

                BlackBoardValues.Add(item);
            }
        }

        public static string _FindShareName(BTAsset asset, int name_id)
        {
            if (asset == null || asset.Data == null || asset.Data.UsedVarList == null)
                return null;

            foreach (var p in asset.Data.UsedVarList)
            {
                if (p.NameId == name_id)
                    return p.Name;
            }
            return null;
        }

        public static string _FindGlobalVarName(int name_id)
        {
            var all_global_vas = BehaviorDesigner.Runtime.GlobalVariables.Instance.GetAllVariables();
            foreach (var p in all_global_vas)
            {
                if (BTUtil.Name2Id(p.Name) == name_id)
                    return p.Name;
            }
            return null;
        }
    }

    public class DotsBTDebuggerWindow : OdinMenuEditorWindow
    {
        private EdSelection _selection;
        public List<EdBTUnit> _all_units = new List<EdBTUnit>();        

        public EdSelection EdSelection
        {
            get
            {
                if (_selection == null)
                {
                    _selection = new EdSelection();
                    _selection.OnSelectionChanged = _OnObjectSelectionChanged;
                }
                return _selection;
            }
        }

        //%(ctrl on Windows and Linux, cmd on macOS), ^ (ctrl on Windows, Linux, and macOS), # (shift), & (alt).
        [MenuItem("Tools/Behaivor Designer(Dots)/Debugger %m")]
        private static void OpenWindow()
        {
            var window = GetWindow<DotsBTDebuggerWindow>();
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

            tree.Add("刷新", new EdBtWindowObjRefresh() { window = this });

            _all_units = DotsBtDebuggerCollector.Collect();
            foreach (var p in _all_units)
            {
                //tree.AddObjectAtPath(p.GetDisplay(), p);
                tree.Add(p.GetDisplay(), p);
            }
            return tree;
        }

        private void _OnObjectSelectionChanged()
        {
            //1. 根据当前的 Selection 找到 GhostId
            GameObject obj = Selection.activeObject as GameObject;
            if (obj == null)
                return;
            DotsBTDebugEntityIndexContainer comp = obj.GetComponentInParent<DotsBTDebugEntityIndexContainer>();
            if (comp == null || comp.ghostId == 0)
                return;

            //2. 判断当前Window 选中的Item 是否和目标GhostId一样
            EdBTUnit cur_item = this.MenuTree.Selection.SelectedValue as EdBTUnit;
            if (cur_item != null && cur_item.UnitID == comp.ghostId)
                return;

            //3. 找到索引
            int index = -1;
            for (int i = 0; i < _all_units.Count; i++)
            {
                if (_all_units[i].UnitID == comp.ghostId)
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
            if (!(cur is EdBTUnit))
                return;

            EdBTUnit cur_item = cur as EdBTUnit;
            cur_item.OnSelect();
        }         
    }

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

    [Serializable]
    public class EdBtWindowObjRefresh
    {
        [NonSerialized]
        public OdinMenuEditorWindow window;
        [Button]
        public void Refresh()
        {
            window.ForceMenuTreeRebuild();
        }
    }


    [Serializable]
    public class EdBTBlackBoardValue
    {
        public string Name;
        public bool Used;
        public EBTVarScope Scope;
        [ShowInInspector]
        public object Value;
    }
}

