using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarColor)]
    public struct BTVarColor: IBTVar<Color, SharedColor>
    {
        public BTVarBase<Color> Base;
        public void Ctor(SharedColor shared_var, BTBakeContextNode context)
        {   
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedColor).Value);
        }

        public Color GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Color value) => Base.SetValue(ref vm, value);
    }
}
