using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarRect)]
    public struct BTVarRect: IBTVar<Rect, SharedRect>
    {
        public BTVarBase<Rect> Base;
        public void Ctor(SharedRect shared_var, BTBakeContextNode context)
        {            
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedRect).Value);
        }

        public Rect GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Rect value) => Base.SetValue(ref vm, value);
    }
}
