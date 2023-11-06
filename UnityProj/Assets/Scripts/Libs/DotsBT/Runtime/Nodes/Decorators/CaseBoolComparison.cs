using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;

namespace DotsBT
{
    [TaskDescription("Decorator, 每次更新的时候, 先判断条件是否满足,如果满足就调用子节点,要不然就Stop子节点")]
    public class CaseBoolComparison : BehaviorDesigner.Runtime.Tasks.Decorator
    {        
        public SharedBool bool1;
        public SharedBool bool2;
    }


    [BTNodeMeta(EBTNode.CaseBoolComparison, EBTNode.Decorator)]
    public struct BTNodeCaseBoolComparison : IBTNode<CaseBoolComparison>
    {
        public BTNodeDecorator Base;
        public BTVarBool bool1;
        public BTVarBool bool2;
        public bool _running;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            CaseBoolComparison spec_task = task as CaseBoolComparison;
            bool1.Ctor(spec_task.bool1, context);
            bool2.Ctor(spec_task.bool2, context);
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
            if (bool1.GetValue(ref vm) != bool2.GetValue(ref vm))
            {
                Stop(ref vm);
                return EBTStatus.Failure;
            }

            var child_status = Base.UpdateDecoratedNode(ref vm);
            _running = child_status == EBTStatus.Running ? true : false;
            return child_status;
        }         
    }
}
