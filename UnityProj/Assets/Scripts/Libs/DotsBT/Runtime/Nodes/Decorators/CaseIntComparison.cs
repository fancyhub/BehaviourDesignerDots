using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;

namespace DotsBT
{
    [TaskDescription("Decorator, 每次更新的时候, 先判断条件是否满足,如果满足就调用子节点,要不然就Stop子节点")]
    public class CaseIntComparison : BehaviorDesigner.Runtime.Tasks.Decorator
    {
        [Tooltip("The operation to perform")]
        public IntComparison.Operation operation;
        [Tooltip("The first integer")]
        public SharedInt integer1;
        [Tooltip("The second integer")]
        public SharedInt integer2;
    }


    [BTNodeMeta(EBTNode.CaseIntComparison, EBTNode.Decorator)]
    public struct BTNodeCaseIntComparison : IBTNode<CaseIntComparison>
    {
        public BTNodeDecorator Base;

        public IntComparison.Operation operation;
        public BTVarInt integer1;
        public BTVarInt integer2;
        public bool _running;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            CaseIntComparison spec_task = task as CaseIntComparison;
            operation = spec_task.operation;
            integer1.Ctor(spec_task.integer1, context);
            integer2.Ctor(spec_task.integer2, context);
            _running = false;
        }

        public void Stop(ref BTVM vm)
        {
            if (_running)
            {
                _running = false;
                Base.Stop(ref vm);
            }
        }

        public EBTStatus Update(ref BTVM vm)
        {
            if (!_IsConditionTrue(ref vm))
            {
                Stop(ref vm);
                return EBTStatus.Failure;
            }

            var child_status = Base.UpdateDecoratedNode(ref vm);
            _running = child_status == EBTStatus.Running ? true : false;
            return child_status;
        }

        private bool _IsConditionTrue(ref BTVM vm)
        {
            switch (operation)
            {
                default:
                    return false;
                case IntComparison.Operation.LessThan:
                    return integer1.GetValue(ref vm) < integer2.GetValue(ref vm);
                case IntComparison.Operation.LessThanOrEqualTo:
                    return integer1.GetValue(ref vm) <= integer2.GetValue(ref vm);
                case IntComparison.Operation.EqualTo:
                    return integer1.GetValue(ref vm) == integer2.GetValue(ref vm);
                case IntComparison.Operation.NotEqualTo:
                    return integer1.GetValue(ref vm) != integer2.GetValue(ref vm);
                case IntComparison.Operation.GreaterThanOrEqualTo:
                    return integer1.GetValue(ref vm) >= integer2.GetValue(ref vm);
                case IntComparison.Operation.GreaterThan:
                    return integer1.GetValue(ref vm) > integer2.GetValue(ref vm);
            }
        }
    }
}
