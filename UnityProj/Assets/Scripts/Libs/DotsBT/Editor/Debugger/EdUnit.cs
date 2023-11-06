using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Entities;
using System;
using Sirenix.OdinInspector;

namespace DotsBT.Debugger.ED
{

    [Serializable]
    public class EdUnit
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


        public static EdUnit Create(int unit_id, World world, Entity e, GameObject gameObject)
        {
            EdUnit ret = new EdUnit();
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
            DotsBT.GraphView.ED.EdBtDebuggerViewer.ShowGraph(World, Entity);
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
}
