using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarQuaternion)]
    public struct BTVarQuaternion : IBTVar<Quaternion, SharedQuaternion>
    {
        public BTVarBase<Quaternion> Base;
        public void Ctor(SharedQuaternion shared_var, BTBakeContextNode context)
        {            
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedQuaternion).Value);
        }

        public Quaternion GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, Quaternion value) => Base.SetValue(ref vm, value);
    }
}
