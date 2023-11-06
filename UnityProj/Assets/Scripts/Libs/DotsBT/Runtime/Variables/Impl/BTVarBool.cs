using System;
using BehaviorDesigner.Runtime;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarBool)]
    public struct BTVarBool : IBTVar<bool, SharedBool>
    {
        public BTVarBase<bool> Base;

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedBool).Value);
        }

        public void Ctor(SharedBool shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public bool GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, bool value) => Base.SetValue(ref vm, value);
    }
}
