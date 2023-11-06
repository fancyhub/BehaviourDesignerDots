using System;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.UntilFailure, EBTNode.Decorator)]
    public struct BTNodeUntilFailure : IBTNode<UntilFailure>
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
                default:                     
                    return EBTStatus.Failure;

                case EBTStatus.Running:
                case EBTStatus.Success:
                    return EBTStatus.Running;

                case EBTStatus.Failure:
                    return EBTStatus.Failure;
            }
        }
    }
}
