using BehaviorDesigner.Runtime;

namespace DotsBT
{
    [BTVarMeta(EBTVar.VarFloat)]
    public struct BTVarFloat : IBTVar<float, SharedFloat>
    {
        public BTVarBase<float> Base;

        public void Ctor(SharedFloat shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedFloat).Value);
        }

        public float GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public float GetValueSquare(ref BTVM vm)
        {
            float v = Base.GetValue(ref vm);
            return v * v;
        }

        public bool SetValue(ref BTVM vm, float value) => Base.SetValue(ref vm, value);
    }
}