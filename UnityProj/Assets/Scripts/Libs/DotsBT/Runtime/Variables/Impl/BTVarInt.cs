using System;
using BehaviorDesigner.Runtime;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarInt)]
    public struct BTVarInt : IBTVar<int, SharedInt>
    {
        public BTVarBase<int> Base;
        public void Ctor(SharedInt shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedInt).Value);
        }

        public int GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, int value) => Base.SetValue(ref vm, value);
    }
}
