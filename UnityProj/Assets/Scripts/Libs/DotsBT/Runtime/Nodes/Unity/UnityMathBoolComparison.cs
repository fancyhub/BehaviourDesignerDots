using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.UnityMathBoolComparison, EBTNode.Conditional)]
    public struct BTNodeUnityMathBoolComparison : IBTNode<BoolComparison>
    {
        public BTNodeConditional Base;
        public BTVarBool bool1;
        public BTVarBool bool2;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            BoolComparison spec_task = task as BoolComparison;
            bool1.Ctor(spec_task.bool1, context);
            bool2.Ctor(spec_task.bool2, context);
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            if (bool1.GetValue(ref vm) == bool2.GetValue(ref vm))
                return EBTStatus.Success;
            return EBTStatus.Failure;
        }
    }
}
