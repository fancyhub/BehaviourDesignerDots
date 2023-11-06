using System;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.Sequence, EBTNode.Composite)]
    public unsafe struct BTNodeSequence : IBTNode<Sequence>
    {
        public BTNodeComposite Base;
        public short currentChildIndex;

        public void Ctor(ref BTVM vm, Task task,BTBakeContextNode nodeExParam)
        {
            Base.Ctor(ref vm, task,nodeExParam);
            currentChildIndex = -1;
        }

        public void Stop(ref BTVM vm)
        {
            if (currentChildIndex == -1)
                return;

            BTPtr child = Base.GetChildPtrAt(ref vm, currentChildIndex);
            vm.StopNode(child);
            currentChildIndex = -1;
        } 

        public EBTStatus Update(ref BTVM vm)
        {
            int count = Base.GetChildCount();

            //Start
            if (currentChildIndex == -1)
            {
                if (count == 0)
                    return EBTStatus.Success;
                currentChildIndex = 0;
            }

            for (; currentChildIndex < count; currentChildIndex++)
            {
                BTPtr ptr = Base.GetChildPtrAt(ref vm, currentChildIndex);
                EBTStatus child_status = vm.UpdateNode(ptr);
                switch (child_status)
                {
                    case EBTStatus.Running:
                        return EBTStatus.Running;

                    case EBTStatus.Success:
                        break;

                    case EBTStatus.Failure:
                        currentChildIndex = -1;
                        return EBTStatus.Failure;

                    default: //不应该出现                        
                        currentChildIndex = -1;
                        return EBTStatus.Failure;
                }
            }
            currentChildIndex = -1;
            return EBTStatus.Success;
        }
    }
}
