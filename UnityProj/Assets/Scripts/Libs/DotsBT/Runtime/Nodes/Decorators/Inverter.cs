using System;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.Inverter, EBTNode.Decorator)]
    public struct BTNodeInverter : IBTNode<Inverter>
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
            var status = Base.UpdateDecoratedNode(ref vm);
            if (status == EBTStatus.Running)
                return EBTStatus.Running;
            else if (status == EBTStatus.Failure)
                return EBTStatus.Success;
            return EBTStatus.Failure;
        }
    }
}
