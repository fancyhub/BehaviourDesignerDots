using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.UnityMathIntComparison, EBTNode.Conditional)]
    public struct BTNodeUnityMathIntComparison : IBTNode<IntComparison>
    {
        public BTNodeConditional Base;

        public IntComparison.Operation operation;
        public BTVarInt integer1;
        public BTVarInt integer2;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            IntComparison spec_task = task as IntComparison;
            operation = spec_task.operation;
            integer1.Ctor(spec_task.integer1, context);
            integer2.Ctor(spec_task.integer2, context);
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            switch (operation)
            {
                default:
                    return EBTStatus.Failure;
                case IntComparison.Operation.LessThan:
                    return integer1.GetValue(ref vm) < integer2.GetValue(ref vm) ? EBTStatus.Success : EBTStatus.Failure;
                case IntComparison.Operation.LessThanOrEqualTo:
                    return integer1.GetValue(ref vm) <= integer2.GetValue(ref vm) ? EBTStatus.Success : EBTStatus.Failure;
                case IntComparison.Operation.EqualTo:
                    return integer1.GetValue(ref vm) == integer2.GetValue(ref vm) ? EBTStatus.Success : EBTStatus.Failure;
                case IntComparison.Operation.NotEqualTo:
                    return integer1.GetValue(ref vm) != integer2.GetValue(ref vm) ? EBTStatus.Success : EBTStatus.Failure;
                case IntComparison.Operation.GreaterThanOrEqualTo:
                    return integer1.GetValue(ref vm) >= integer2.GetValue(ref vm) ? EBTStatus.Success : EBTStatus.Failure;
                case IntComparison.Operation.GreaterThan:
                    return integer1.GetValue(ref vm) > integer2.GetValue(ref vm) ? EBTStatus.Success : EBTStatus.Failure;
            }

        }
    }
}
