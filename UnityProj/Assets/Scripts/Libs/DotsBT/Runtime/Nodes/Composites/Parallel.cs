using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.Parallel, EBTNode.Composite)]
    public unsafe struct BTNodeParallel : IBTNode<Parallel>
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
            int fail_count = 0;
            int succ_count = 0;

            for (int i = 0; i < count; i++)
            {
                EBTStatus status = childStatus.Get(ref vm, i);

                //检查是否需要运行
                if (status == EBTStatus.Inactive || status == EBTStatus.Running)
                {
                    BTPtr child = Base.GetChildPtrAt(ref vm, i);
                    status = vm.UpdateNode(child);

                    childStatus.Set(ref vm, i, status);
                }

                if (status == EBTStatus.Success)
                    succ_count++;
                else if (status == EBTStatus.Failure)
                    fail_count++;
            }

            if (fail_count > 0)
            {
                Stop(ref vm);
                return EBTStatus.Failure;
            }
            if (succ_count == count)
            {
                childStatus.ResetAll(ref vm, EBTStatus.Inactive);
                return EBTStatus.Success;
            }
            return EBTStatus.Running;
        }

        public void Stop(ref BTVM vm)
        {
            int count = childStatus.Length;
            for (int i = 0; i < count; i++)
            {
                EBTStatus status = childStatus.Get(ref vm, i);
                childStatus.Set(ref vm, i, EBTStatus.Inactive);
                if (status == EBTStatus.Running)
                    vm.StopNode(Base.GetChildPtrAt(ref vm, i));
            }
        }
    }
}
