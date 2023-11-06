using System;
using System.Diagnostics;
using BehaviorDesigner.Runtime.Tasks;
using Unity.Entities;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.RandomSequence, EBTNode.Composite)]
    public unsafe struct BTNodeRandomSequence : IBTNode<RandomSequence>
    {
        public BTNodeComposite Base;
        public int seed;
        public bool useSeed;

        public BTArray<ushort> childIndexList;
        public short currentChildIndex;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            RandomSequence spec_task = task as RandomSequence;
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
                childIndexList.Shuffle(ref vm, useSeed ? (uint)seed : (uint)System.DateTime.Now.Ticks);
                currentChildIndex = 0;
            }

            for (; currentChildIndex < count; currentChildIndex++)
            {
                BTPtr ptr = Base.GetChildPtrAt(ref vm, childIndexList.Get(ref vm, currentChildIndex));
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
