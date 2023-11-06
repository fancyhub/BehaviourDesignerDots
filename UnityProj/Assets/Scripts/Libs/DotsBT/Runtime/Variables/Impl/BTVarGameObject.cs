using System;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Unity.Entities;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarGameObject)]
    public struct BTVarGameObject : IBTVar<Entity, GameObject, SharedGameObject>
    {
        public BTVarBase<Entity> Base;

        public void Ctor(SharedGameObject shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var, context);
            if (Base.Scope == EBTVarScope.Local && shared_var.Value != null)
            {
                throw new Exception("GameObject 类型不支持 Local");
            }
        }

        public Entity GetValue(ref BTVM vm)
        {
            BTPtr ptr = Base.Link(ref vm);
            if (ptr.IsNull)
                return Base.Value;

            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Entity value) => Base.SetValue(ref vm, value);

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            GameObject obj = (var as SharedGameObject).Value;
            Entity e = context.Baker.GetEntity(obj, TransformUsageFlags.Dynamic);
            BTPtr ret = context.Allocator.AllocAndSet(e);

            context.EntityList.Add(BTBlobEntity.Create(e, ret));
            return ret;
        }
    }
}
