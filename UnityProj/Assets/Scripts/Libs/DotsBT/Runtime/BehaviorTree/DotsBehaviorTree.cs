using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Unity.Collections;
using Unity.Entities;

namespace DotsBT
{
    public class DotsBehaviorTree : Behavior
    {
        public bool UseOrig = false;        
        public DotsGlobalVar GlobalVar;

        public void Awake()
        {
            if (!UseOrig)
            {
                this.enabled = false;
            }
        }

#if UNITY_EDITOR
        public void EdLoadGlobalVarPrefab()
        {
            GlobalVar = DotsGlobalVar.Load();
        }

        public void Reset()
        {
            GlobalVar = DotsGlobalVar.Load();
        }
#endif
    }


    public class DotsBehaviorTreeBaker : Baker<DotsBehaviorTree>
    {
        public static bool Bake(Behavior bt_behavior, DotsGlobalVar global_var, IBaker baker, out BTAssetCompData comp)
        {
            comp = new BTAssetCompData();
            DotsExternalBehavior ext_src = bt_behavior.ExternalBehavior as DotsExternalBehavior;
            if (ext_src == null)
            {
                UnityEngine.Debug.LogError($"Bake {bt_behavior.name} 有问题, 对应的行为树资源为空");
                return false;
            }
            comp.GlobalVarEntity = baker.GetEntity(global_var, TransformUsageFlags.None);


            //Bake Var
            try
            {
                comp.Vars = BTBaker.BakeSharedVars(baker, bt_behavior);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Bake 节点 {bt_behavior.name} 上的行为树的时候出错", bt_behavior);
                UnityEngine.Debug.LogException(e);
                return false;
            }

            //Bake Nodes Image            
            comp.Image = ext_src.RefDotsBTAsset;

            comp.Vars.NameList.AddToBaker(baker);
            comp.Vars.Data.AddToBaker(baker);
            baker.DependsOn(global_var);
            return true;

        }

        public override void Bake(DotsBehaviorTree authoring)
        {
            authoring.EdLoadGlobalVarPrefab();

            if (authoring.GlobalVar == null)
            {
                UnityEngine.Debug.LogError($"Bake {authoring.name} 有问题, 没有找到 Global Var Prefab");
                return;
            }

            if (Bake(authoring, authoring.GlobalVar, this, out var comp))
            {
                //添加组件
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, comp);
            }
        }
    }
}
