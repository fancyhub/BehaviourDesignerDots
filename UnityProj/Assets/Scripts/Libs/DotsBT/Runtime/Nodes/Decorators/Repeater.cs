using System;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.Repeater, EBTNode.Decorator)]
    public unsafe struct BTNodeRepeater : IBTNode<Repeater>
    {
        public BTNodeDecorator Base;
        public BTVarInt count;
        public BTVarBool repeatForever;
        public BTVarBool endOnFailure;

        public int executionCount;
        public EBTStatus executionStatus;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            Repeater spec_task = task as Repeater;
            executionCount = 0;
            executionStatus = EBTStatus.Inactive;

            count.Ctor(spec_task.count, context);
            repeatForever.Ctor(spec_task.repeatForever, context);
            endOnFailure.Ctor(spec_task.endOnFailure, context);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            //判断数量是否到了
            if (_IsCountEnd(ref vm))
            {
                executionCount = 0;
                executionStatus = EBTStatus.Inactive;
                return EBTStatus.Success;
            }

            EBTStatus st = Base.UpdateDecoratedNode(ref vm);
            switch (st)
            {
                case EBTStatus.Success:
                    if (_UpdateCount(ref vm))
                    {
                        executionCount = 0;
                        executionStatus = EBTStatus.Inactive;
                        return EBTStatus.Success;
                    }
                    return EBTStatus.Running;

                case EBTStatus.Running:
                    executionStatus = EBTStatus.Running;
                    return EBTStatus.Running;

                case EBTStatus.Failure:
                    if (endOnFailure.GetValue(ref vm))
                    {
                        executionCount = 0;
                        return EBTStatus.Failure;
                    }
                    else if (_UpdateCount(ref vm))
                    {
                        executionCount = 0;
                        executionStatus = EBTStatus.Inactive;
                        return EBTStatus.Success;
                    }
                    else
                        return EBTStatus.Running;

                default: //不应该
                    executionCount = 0;
                    executionStatus = EBTStatus.Inactive;
                    return EBTStatus.Failure;
            }
        }

        public void Stop(ref BTVM vm)
        {
            if (executionStatus == EBTStatus.Running)
            {
                executionStatus = EBTStatus.Inactive;
                executionCount = 0;
                Base.Stop(ref vm);
            }
        }

        //True: 结束了
        private bool _UpdateCount(ref BTVM vm)
        {
            //永远不结束
            if (repeatForever.GetValue(ref vm))
                return false;
            executionCount++;
            if (executionCount < count.GetValue(ref vm))
                return false;
            return true;
        }

        private bool _IsCountEnd(ref BTVM vm)
        {
            //永远不结束
            if (repeatForever.GetValue(ref vm))
                return false;

            if (executionCount < count.GetValue(ref vm))
                return false;
            return true;
        }
    }

}
