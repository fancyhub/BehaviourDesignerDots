using System;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.ReturnFailure, EBTNode.Decorator)]
    public struct BTNodeReturnFailure : IBTNode<ReturnFailure>
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
            return EBTStatus.Failure;
        }
    }
}
