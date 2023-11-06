using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.ParallelComplete, EBTNode.Composite)]
    public unsafe struct BTNodeParallelComplete : IBTNode<ParallelComplete>
    {
        public BTNodeComposite Base;
        public BTArray<EBTStatus> childStatus;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            childStatus.Ctor(ref context.Allocator, Base.GetChildCount());
            childStatus.ResetAll(ref vm, EBTStatus.Inactive);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            int count = childStatus.Length;
            if (count == 0)
                return EBTStatus.Success;


            for (int i = 0; i < count; i++)
            {
                BTPtr child = Base.GetChildPtrAt(ref vm, i);
                var status = vm.UpdateNode(child);
                childStatus.Set(ref vm, i, status);

                if (status == EBTStatus.Failure || status == EBTStatus.Success)
                {
                    Stop(ref vm);
                    return status;
                }
            }

            return EBTStatus.Running;
        }

        public void Stop(ref BTVM vm)
        {
            int count = childStatus.Length;
            for (int i = 0; i < count; i++)
            {
                EBTStatus status = childStatus.Get(ref vm, i);
                if (status == EBTStatus.Running)
                    vm.StopNode(Base.GetChildPtrAt(ref vm, i));
            }
            childStatus.ResetAll(ref vm, EBTStatus.Inactive);
        }
    }
}
