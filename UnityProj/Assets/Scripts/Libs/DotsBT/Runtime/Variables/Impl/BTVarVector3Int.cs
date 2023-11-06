using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarVector3Int)]
    public struct BTVarVector3Int : IBTVar<Vector3Int, SharedVector3Int>
    {
        public BTVarBase<Vector3Int> Base;
        public void Ctor(SharedVector3Int shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var,context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedVector3Int).Value);
        }

        public Vector3Int GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Vector3Int value) => Base.SetValue(ref vm, value);
    }
}
