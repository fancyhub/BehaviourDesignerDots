using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace DotsBT
{
    public struct DotsBTRuntimeArrayComp : IComponentData
    {
        public int FrameIndex;
        public NativeArray<EBTStatus> Status;

        public Entity TargetEntity;
        public int TargetNodeCount; //临时存储的, 编辑器设置,也是编辑器读取的, 可以不放在这里

        public unsafe void Reset()
        {
            if (TargetEntity == Entity.Null)
                return;
            UnsafeUtility.MemClear(Status.GetUnsafePtr(), Status.Length * UnsafeUtility.SizeOf<EBTStatus>());
        }

        public unsafe BTDebugStatusArray CreateArray(Entity e)
        {
            if (e != TargetEntity || TargetEntity == Entity.Null)
                return new BTDebugStatusArray();
#if UNITY_EDITOR
            UnsafeUtility.MemClear(Status.GetUnsafePtr(), Status.Length * UnsafeUtility.SizeOf<EBTStatus>());
#endif
            return new BTDebugStatusArray()
            {
                Length = Status.Length,
                Status = (EBTStatus*)Status.GetUnsafePtr(),
            };
        }
    }

    public unsafe struct BTDebugStatusArray
    {
        public int Length;
        public EBTStatus* Status;

        public void SetState(int index, EBTStatus state)
        {
            if (Status == null || index >= Length || index < 0)
                return;
            Status[index] = state;
        }
    }
}