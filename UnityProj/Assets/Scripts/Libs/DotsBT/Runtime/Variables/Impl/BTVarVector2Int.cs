using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarVector2Int)]
    public struct BTVarVector2Int : IBTVar<Vector2Int, SharedVector2Int>
    {
        public BTVarBase<Vector2Int> Base;
        public void Ctor(SharedVector2Int shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedVector2Int).Value);
        }

        public Vector2Int GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Vector2Int value) => Base.SetValue(ref vm, value);
    }
}
