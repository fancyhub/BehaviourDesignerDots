using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarVector4)]
    public struct BTVarVector4 : IBTVar<Vector4, SharedVector4>
    {
        public BTVarBase<Vector4> Base;
        public void Ctor(SharedVector4 shared_var, BTBakeContextNode context)
        {            
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedVector4).Value);
        }

        public Vector4 GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Vector4 value) => Base.SetValue(ref vm, value);
    }
}
