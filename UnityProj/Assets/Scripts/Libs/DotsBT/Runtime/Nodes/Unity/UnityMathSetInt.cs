using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.UnityMathSetInt, EBTNode.Action)]
    public struct BTNodeUnityMathSetInt : IBTNode<SetInt>
    {
        public BTNodeAction Base;
        public BTVarInt intValue;
        public BTVarInt storeResult;

        public void Ctor(ref BTVM vm, Task task,BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task,context);
            SetInt spec_task = task as SetInt;
            intValue.Ctor(spec_task.intValue, context);
            storeResult.Ctor(spec_task.storeResult, context);
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            int v = intValue.GetValue(ref vm);
            storeResult.SetValue(ref vm, v);
            return EBTStatus.Success;
        }
    }
}
