using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditorInternal;

namespace DotsBT
{
    public struct BtDebugComp : IComponentData
    {
        public int FrameIndex;

        public NativeArray<byte> Status;
        public Entity TargetEntity;

        public unsafe void Reset()
        {
            if (TargetEntity == Entity.Null)
                return;
            if (!Status.IsCreated)
                return;

            UnsafeUtility.MemClear(Status.GetUnsafePtr(), Status.Length);
        }

        public unsafe BTDebugStatusArray GetArrary(Entity e)
        {
            if (e != TargetEntity || TargetEntity == Entity.Null)
                return BTDebugStatusArray.Empty;

            if (!Status.IsCreated)
                return BTDebugStatusArray.Empty;

            return new BTDebugStatusArray((byte*)Status.GetUnsafePtr(), Status.Length, FrameIndex);
        }

        public unsafe BTDebugStatusArray CreateArray(Entity e)
        {
            if (e != TargetEntity || TargetEntity == Entity.Null)
                return BTDebugStatusArray.Empty;

            if (!Status.IsCreated)
                return BTDebugStatusArray.Empty;

#if UNITY_EDITOR
            UnsafeUtility.MemClear(Status.GetUnsafePtr(), Status.Length);
#endif
            return new BTDebugStatusArray((byte*)Status.GetUnsafePtr(), Status.Length, FrameIndex);
        }
    }

    public unsafe struct BTDebugStatusArray
    {
        public static BTDebugStatusArray Empty = new BTDebugStatusArray(null, 0, 0);
        public readonly int FrameIndex;
        public readonly int Count;
        private byte* _Status;

        public BTDebugStatusArray(byte* data, int count, int frameIndex)
        {
            this.Count = count;
            this._Status = data;
            this.FrameIndex = frameIndex;
        }

        public bool IsValid()
        {
            if (Count > 0 && _Status != null)
                return true;
            return false;
        }

        public void SetState(int index, EBTStatus state)
        {
            if (_Status == null || index >= Count || index < 0)
                return;
            _Status[index] = (byte)state;
        }

        public EBTStatus GetState(int index)
        {
            if (index < 0 || index >= Count)
                return EBTStatus.Inactive;
            return (EBTStatus)this._Status[index];
        }
    }
}