using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarVector3)]
    public struct BTVarVector3 : IBTVar<Vector3, SharedVector3>
    {
        public BTVarBase<Vector3> Base;
        public void Ctor(SharedVector3 shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var,context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedVector3).Value);
        }

        public Vector3 GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Vector3 value) => Base.SetValue(ref vm, value);
    }
}
