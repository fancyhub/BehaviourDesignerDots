using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.Selector, EBTNode.Composite)]
    public unsafe struct BTNodeSelector : IBTNode<Selector>
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
            currentChildIndex = -1;
            if (!child.IsNull)
            {
                vm.StopNode(child);
            }
        } 

        public EBTStatus Update(ref BTVM vm)
        {
            int count = Base.GetChildCount();
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
                    default: //不应该出现
                        currentChildIndex = -1;
                        return EBTStatus.Failure;

                    case EBTStatus.Running:
                        return EBTStatus.Running;

                    case EBTStatus.Success:
                        currentChildIndex = -1;
                        return EBTStatus.Success;

                    case EBTStatus.Failure:
                        break;
                }
            }

            currentChildIndex = -1;
            return EBTStatus.Failure;
        }
    }
}
