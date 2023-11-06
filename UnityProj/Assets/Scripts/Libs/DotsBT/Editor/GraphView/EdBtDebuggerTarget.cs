using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;

namespace DotsBT.GraphView.ED
{
    public struct EdBtDebuggerTarget
    {
        public Entity Entity;
        public World World;

        //True: 发生变化
        //false: 没有变化
        public bool Set(World world, Entity entity)
        {
            if (this.World == world && this.Entity == entity)
                return false;
            this.World = world;
            this.Entity = entity;
            return true;
        }

        public BTAsset GetAsset()
        {
            if (World == null || !World.IsCreated || Entity == Entity.Null)
                return null;
            BTAssetCompData bt_asset_comp = World.EntityManager.GetComponentData<BTAssetCompData>(Entity);
            return BTBaker.EdGetWeakRefObj(bt_asset_comp.Image);
        }

        public bool GetDebugRTArray(out BTDebugStatusArray result)
        {
            result = default;
            if (World == null || !World.IsCreated)
                return false;

            
            var query = World.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<BtDebugComp>());
            var single_entity = query.GetSingletonEntity();
            if (single_entity == Entity.Null)
                return false;

            var comp = World.EntityManager.GetComponentData<BtDebugComp>(single_entity);
            result =  comp.GetArrary(Entity);
            return result.IsValid();
        }

        public void EnableDebug(bool enable)
        {
            if (World == null || !World.IsCreated)
                return;

            var query = World.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<BtDebugComp>());
            var single_entity = query.GetSingletonEntity();
            var comp = World.EntityManager.GetComponentData<BtDebugComp>(single_entity);

            Entity new_tar = Entity;
            if (!enable)
                new_tar = Entity.Null;
            if (comp.TargetEntity == new_tar)
                return;

            comp.TargetEntity = new_tar;
            if (new_tar != Entity.Null)
                comp.Reset();
            World.EntityManager.SetComponentData(single_entity, comp);
        }
    }

}
