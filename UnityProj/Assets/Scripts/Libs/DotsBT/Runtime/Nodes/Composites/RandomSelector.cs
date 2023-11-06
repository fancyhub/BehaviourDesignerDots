using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.RandomSelector, EBTNode.Composite)]
    public unsafe struct BTNodeRandomSelector : IBTNode<RandomSelector>
    {
        public BTNodeComposite Base;
        public int seed;
        public bool useSeed;

        public BTArray<ushort> childIndexList;
        public short currentChildIndex;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            RandomSelector spec_task = task as RandomSelector;
            seed = spec_task.seed;
            useSeed = spec_task.useSeed;

            childIndexList.Ctor(ref context.Allocator, Base.GetChildCount());
            for (ushort i = 0; i < childIndexList.Length; i++)
                childIndexList.Set(ref vm, i, i);
            currentChildIndex = -1;
        }

        public void Stop(ref BTVM vm)
        {
            if (currentChildIndex == -1)
                return;

            BTPtr child = Base.GetChildPtrAt(ref vm, childIndexList.Get(ref vm, currentChildIndex));
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
                childIndexList.Shuffle(ref vm, useSeed ? (uint)seed : (uint)System.DateTime.Now.Ticks);
            }

            for (; currentChildIndex < count; currentChildIndex++)
            {
                BTPtr ptr = Base.GetChildPtrAt(ref vm, childIndexList.Get(ref vm, currentChildIndex));
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
