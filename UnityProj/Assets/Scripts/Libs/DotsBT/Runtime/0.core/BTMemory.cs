using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.InteropServices;

namespace DotsBT
{
    //内存的分段
    public enum EBTMemSeg : byte
    {
        None = 0,
        Exe = 1,       //节点
        SharedVar = 2, //共享的Var
        GlobalVar = 3, //global var
    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct BTPtr : IEquatable<BTPtr>
    {
        public static BTPtr Null => new BTPtr() { _address = 0 };

        [FieldOffset(0), UnityEngine.SerializeField]
        private uint _address;
        [FieldOffset(0), System.NonSerialized]
        private byte _seg;
        [FieldOffset(2), System.NonSerialized]
        private ushort _offset;

        public EBTMemSeg Seg => (EBTMemSeg)_seg;
        public ushort Offset => _offset;
        public uint Address => _address;

        public static BTPtr Create(EBTMemSeg seg, ushort offset) { return new BTPtr() { _seg = (byte)seg, _offset = offset }; }

        public bool IsNull { get { return _address == 0; } }
        public override int GetHashCode() { return (int)_address; }
        public bool Equals(BTPtr other) { return other._address == _address; }
        public override bool Equals(object obj) { return obj is BTPtr p && p._address == _address; }
        public static bool operator ==(BTPtr a, BTPtr b) { return a._address == b._address; }
        public static bool operator !=(BTPtr a, BTPtr b) { return a._address != b._address; }
        public static BTPtr operator +(BTPtr a, int offset)
        {
            long dest = a._offset + offset;
            if (dest < 0 || dest > ushort.MaxValue)
            {
                throw new Exception("越界");
            }
            return new BTPtr()
            {
                _seg = a._seg,
                _offset = (ushort)dest,
            };
        }

        public override string ToString() { return $"{Seg}:{Offset}"; }
    }

    public unsafe struct BTSegMemory
    {
        [MarshalAs(UnmanagedType.U1)]public readonly bool Writable;
        private readonly byte _seg;
        public readonly ushort Len;
        private readonly byte* _buff;

        //注意: 一定不要在Buff释放之后,再次访问 BTMemory
        public static BTSegMemory Create(EBTMemSeg seg, bool writable, byte[] buff)
        {
            fixed (byte* p = buff)
            {
                return new BTSegMemory(seg, writable, p, buff.Length);
            }
        }

        //注意: 不要在 NativeArray 释放之后, 再次访问BTMemory
        public static BTSegMemory Create(EBTMemSeg seg, bool writable, NativeArray<byte> array)
        {
            return new BTSegMemory(seg, writable, (byte*)array.GetUnsafePtr(), array.Length);
        }

        public static BTSegMemory Create(EBTMemSeg seg, bool writable, MyBlobArray<byte> array)
        {
            return new BTSegMemory(seg, writable, array.GetUnsafePtr(), array.Length);
        }

        public BTSegMemory(EBTMemSeg seg, bool writable, byte* buff, int len)
        {
            this._seg = (byte)seg;
            this.Writable = writable;
            this._buff = (byte*)buff;
            if (len > ushort.MaxValue || len < 0)
                throw new Exception($"Memory 太大了 {len}, 要在 [0,{ushort.MaxValue}] ");
            this.Len = (ushort)len;
        }

        public EBTMemSeg Seg => (EBTMemSeg)_seg;

        public T* Get<T>(BTPtr ptr, bool write) where T : unmanaged
        {
            if (ptr.IsNull || ptr.Seg != Seg)
                return null;

            if (write && !Writable)
                throw new Exception($"内存段 {Seg} 不允许写入");

            int size = sizeof(T) + ptr.Offset;
            if (size > Len)
                throw new Exception("越界");
            return (T*)(_buff + ptr.Offset);
        }

        public T[] Clone<T>(int element_count) where T : unmanaged
        {
            if (element_count < 0)
                return new T[0];
            int size = sizeof(T) * element_count;
            if (size >= Len)
                return new T[0];

            T[] ret = new T[element_count];

            fixed (T* pRet = ret)
            {
                Buffer.MemoryCopy(_buff, pRet, size, size);
            }
            return ret;
        }

        public BTPtr GetPtr<T>(ref T v) where T : unmanaged
        {
            fixed (T* p = &v)
            {
                long offset = (byte*)p - _buff;

                if (offset < 0)
                    return BTPtr.Null;

                if ((offset + sizeof(T)) > Len)
                    return BTPtr.Null;
                return BTPtr.Create(Seg, (ushort)offset);
            }
        }
    }

    public unsafe struct BTMemory
    {
        public BTSegMemory Exe;
        public BTSegMemory SharedVar;
        public BTSegMemory GlobalVar;

        public T* Get<T>(BTPtr ptr, bool write) where T : unmanaged
        {
            if (ptr.IsNull)
                return null;

            switch (ptr.Seg)
            {
                default: return null;

                case EBTMemSeg.Exe:
                    return Exe.Get<T>(ptr, write);
                case EBTMemSeg.SharedVar:
                    return SharedVar.Get<T>(ptr, write);
                case EBTMemSeg.GlobalVar:
                    return GlobalVar.Get<T>(ptr, write);
            }
        }

        public T GetValue<T>(BTPtr ptr) where T : unmanaged
        {
            T* p = Get<T>(ptr, false);
            if (p == null)
                return default;
            return *p;
        }

        public bool SetValue<T>(BTPtr ptr, T value) where T : unmanaged
        {
            T* p = Get<T>(ptr, true);
            if (p == null)
                return false;
            *p = value;
            return true;
        }
    }

    //没有free
    public unsafe struct BTMemoryAllocator
    {
        private int _pos;
        private BTSegMemory _memory;

        public BTMemoryAllocator(BTSegMemory memory, int pos = 0)
        {
            _memory = memory;
            _pos = pos;
        }

        public BTSegMemory Memory => _memory;

        public BTPtr AllocArray<T>(int element_count) where T : unmanaged
        {
            return Alloc(sizeof(T) * element_count);
        }

        public BTPtr AllocArrayAndSet<T>(T[] data) where T : unmanaged
        {
            int total_size = sizeof(T) * data.Length;
            BTPtr ret = Alloc(total_size);

            byte* p = _memory.Get<byte>(ret, true);
            if (p == null)
                return ret;

            fixed (T* p_src = data)
            {
                Buffer.MemoryCopy(p_src, p, total_size, total_size);
            }
            return ret;
        }

        public BTPtr AllocAndSet<T>(T value) where T : unmanaged
        {
            BTPtr ret = Alloc(sizeof(T));
            if (ret.IsNull)
                return ret;

            T* p = _memory.Get<T>(ret, true);
            if (p == null)
                return BTPtr.Null;

            *p = value;
            return ret;
        }

        public BTPtr Alloc<T>() where T : unmanaged
        {
            return Alloc(sizeof(T));
        }

        public BTPtr Alloc(int byte_size)
        {
            if (byte_size <= 0)
                return BTPtr.Null;

            if (!_memory.Writable)
            {
                UnityEngine.Debug.LogError($"内存段 {_memory.Seg} 不允许写入");
                return BTPtr.Null;
            }

            int remain = _memory.Len - _pos;
            if (remain < byte_size)
                throw new System.OutOfMemoryException();
            var ret = BTPtr.Create(_memory.Seg, (ushort)_pos);
            _pos += byte_size;
            return ret;
        }

        public BTPtr GetPtr<T>(ref T v) where T : unmanaged
        {
            return _memory.GetPtr(ref v);
        }

        public int AllocatedSize { get { return _pos; } }
    }
}
