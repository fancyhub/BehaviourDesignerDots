using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarVector2)]
    public struct BTVarVector2 : IBTVar<Vector2, SharedVector2>
    {
        public BTVarBase<Vector2> Base;
        public void Ctor(SharedVector2 shared_var, BTBakeContextNode context)
        {            
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedVector2).Value);
        }

        public Vector2 GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Vector2 value) => Base.SetValue(ref vm, value);
    }
}
