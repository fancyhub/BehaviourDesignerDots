using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotsBT
{
    public unsafe struct BTArray<T> where T : unmanaged
    {
        public BTPtr Ptr;
        public int Length; //不能修改, 除了Ctor
        public void Ctor(ref BTMemoryAllocator allocator, int len)
        {
            this.Ptr = allocator.AllocArray<T>(len);
            this.Length = len;
        }

        public T Get(ref BTVM vm, int index)
        {
            if (index < 0 || index >= Length)
                return default;

            T* p_data = vm.Get<T>(Ptr, false);
            p_data = p_data + index;
            return *p_data;
        }

        public void Set(ref BTVM vm, int index, T v)
        {
            if (index < 0 || index >= Length)
                return;

            T* p_data = vm.Get<T>(Ptr, true);
            p_data = p_data + index;
            *p_data = v;
        }

        public void Swap(ref BTVM vm, int i, int j)
        {
            if (i < 0 || i >= Length || j < 0 || j >= Length)
                throw new System.ArgumentOutOfRangeException();
            if (i == j)
                return;

            T* p_data = vm.Get<T>(Ptr, true);
            T t = *(p_data + i);
            *(p_data + i) = *(p_data + j);
            *(p_data + j) = t;
        }

        public void ResetAll(ref BTVM vm, T v)
        {
            T* p_data = vm.Get<T>(Ptr, true);
            for (int i = 0; i < Length; i++)
            {
                *(p_data + i) = v;
            }
        }

        //传入Seed,返回Seed
        public uint Shuffle(ref BTVM vm, uint seed)
        {
            int count = Length;
            if (count < 2) //低于两个
                return seed;

            Unity.Mathematics.Random random = new Unity.Mathematics.Random();
            random.InitState(seed);

            T* p_data = vm.Get<T>(Ptr, true);
            for (int i = count - 1; i > 0; i--)
            {
                int j = random.NextInt(i + 1);
                if (j == i)//不需要交换                
                    continue;

                T* p1 = p_data + i;
                T* p2 = p_data + j;
                T t = *p1;
                *p1 = *p2;
                *p2 = t;
            }
            return random.state;
        }
    }

    public unsafe struct BTList<T> where T : unmanaged
    {
        public BTArray<T> Array;
        public int Count;

        public void Ctor(ref BTMemoryAllocator allocator, int capacity)
        {
            Array.Ctor(ref allocator, capacity);
            this.Count = 0;
        }

        public T Get(ref BTVM vm, int index)
        {
            if (index < 0 || index >= Count)
                throw new System.ArgumentOutOfRangeException();
            return Array.Get(ref vm, index);
        }

        public void Set(ref BTVM vm, int index, T v)
        {
            if (index < 0 || index >= Count)
                throw new System.ArgumentOutOfRangeException();

            Array.Set(ref vm, index, v);
        }

        public void Swap(ref BTVM vm, int i, int j)
        {
            if (i < 0 || i >= Count || j < 0 || j >= Count)
                throw new System.ArgumentOutOfRangeException();
            Array.Swap(ref vm, i, j);
        }

        public void Add(ref BTVM vm, T v)
        {
            if (Count >= Array.Length)
                throw new System.ArgumentOutOfRangeException();

            Array.Set(ref vm, Count, v);
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }
    }

    public unsafe struct BTStack<T> where T : unmanaged
    {
        public BTArray<T> Array;
        public int Count;

        public void Ctor(ref BTMemoryAllocator allocator, int capacity)
        {
            Array.Ctor(ref allocator, capacity);
            this.Count = 0;
        }

        public void Push(ref BTVM vm, T v)
        {
            if (Count >= Array.Length)
                throw new System.ArgumentOutOfRangeException();

            Array.Set(ref vm, Count, v);
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }
    }
}
