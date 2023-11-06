using System;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.UntilSuccess, EBTNode.Decorator)]
    public struct BTNodeUntilSuccess : IBTNode<UntilSuccess>
    {
        public BTNodeDecorator Base;
        public void Ctor(ref BTVM vm, Task task,BTBakeContextNode nodeExParam)
        {
            Base.Ctor(ref vm, task,nodeExParam);
        }

        public void Stop(ref BTVM vm)
        {
            Base.Stop(ref vm);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            EBTStatus child_status = Base.UpdateDecoratedNode(ref vm);
            switch (child_status)
            {
                default: return EBTStatus.Failure;

                case EBTStatus.Running:
                case EBTStatus.Failure:
                    return EBTStatus.Running;
                case EBTStatus.Success:
                    return EBTStatus.Success;
            }
        }
    }
}
