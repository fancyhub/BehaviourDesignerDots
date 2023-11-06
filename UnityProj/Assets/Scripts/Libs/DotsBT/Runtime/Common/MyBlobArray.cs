using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace DotsBT
{
    public unsafe struct MyBlobArray
    {
        public static MyBlobArray<T> Create<T>(T[] data) where T : unmanaged
        {
            return Create<T>(data, data.Length);
        }

        public static MyBlobArray<T> Create<T>(T[] data, int element_len) where T : unmanaged
        {
            if (data == null || data.Length == 0 || element_len <= 0 || element_len > data.Length)
            {
                return new MyBlobArray<T>()
                {
                    Asset = BlobAssetReference<T>.Null,
                    Length = 0
                };
            }

            int byte_len = element_len * sizeof(T);
            byte* p = stackalloc byte[byte_len];
            T* p_t = (T*)p;
            for (int i = 0; i < element_len; i++)
            {
                *p_t = data[i];
                p_t++;
            }

            return new MyBlobArray<T>()
            {
                Asset = BlobAssetReference<T>.Create(p, byte_len),
                Length = element_len
            };
        }

        public static MyBlobArray<T> Create<T>(List<T> list) where T : unmanaged
        {
            if (list.Count == 0)
            {
                return new MyBlobArray<T>()
                {
                    Asset = BlobAssetReference<T>.Null,
                    Length = 0
                };
            }

            int byte_len = list.Count * sizeof(T);
            byte* p = stackalloc byte[byte_len];
            T* p_t = (T*)p;
            for (int i = 0; i < list.Count; i++)
            {
                *p_t = list[i];
                p_t++;
            }

            return new MyBlobArray<T>()
            {
                Asset = BlobAssetReference<T>.Create(p, byte_len),
                Length = list.Count
            };
        }
    }

    public unsafe struct MyBlobArray<T> : IDisposable where T : unmanaged
    {
        public BlobAssetReference<T> Asset;
        public int Length;

        public int ByteSize => sizeof(T) * Length;

        public byte* GetUnsafePtr()
        {
            return (byte*)Asset.GetUnsafePtr();
        }

        public void AddToBaker(IBaker baker)
        {
            if (Length > 0)
                baker.AddBlobAsset(ref Asset, out var _);
        }

        public T* ElementAt(int index)
        {
            if (index < 0 || index >= Length)
                return default;
            T* p = (T*)Asset.GetUnsafePtr();
            return p + index;
        }

        public T this[int index]
        {
            get
            {
                T* p = ElementAt(index);
                if (p == null)
                    return default;
                return *p;
            }
        }

        public NativeArray<T> Clone2NativeArray(Allocator allocator)
        {
            NativeArray<T> ret = new NativeArray<T>(Length, allocator);
            if (Length > 0)
            {
                int size = Length * UnsafeUtility.SizeOf<T>();
                void* p_src = Asset.GetUnsafePtr();
                void* p_dst = ret.GetUnsafePtr();
                Buffer.MemoryCopy(p_src, p_dst, size, size);
            }
            return ret;
        }

        public void Dispose()
        {
            Asset.Dispose();
        }
    }
}
