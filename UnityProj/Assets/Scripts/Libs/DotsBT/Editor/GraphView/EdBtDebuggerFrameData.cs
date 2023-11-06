using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;

namespace DotsBT.GraphView.ED
{
    public struct EdBtDebuggerFrameData
    {
        public int FrameIndex;
        public EBTStatus[] Status;

        public bool IsStatusSame(EdBtDebuggerFrameData other)
        {
            if (Status.Length != other.Status.Length)
                return false;

            for (int i = 0; i < other.Status.Length; i++)
            {
                if (other.Status[i] != Status[i])
                    return false;
            }
            return true;
        }
    }

    public class MyFixedQueue<T>
    {
        private T[] _array;
        private int _index = 0; //末尾位置, 用来插入的
        private int _count = 0;
        public MyFixedQueue(int cap)
        {
            _array = new T[cap];
        }

        public int Count { get => _count; }

        public void Enqueue(T data)
        {
            if (_array.Length == 0)
                return;

            _array[_index % _array.Length] = data;
            _index++;
            _count = Math.Min(_count + 1, _array.Length);
        }

        public T Dequeue()
        {
            int index = _CalcArrayIndex(0);
            if (index < 0)
                return default;
            var ret = _array[index];
            _count--;
            return ret;
        }

        public void Clear()
        {
            _count = 0;
        }

        public T this[int index]
        {
            get
            {
                int real_index = _CalcArrayIndex(index);
                if (real_index < 0)
                    return default;
                return _array[real_index];
            }
        }

        private int _CalcArrayIndex(int index)
        {
            if (index < 0 || index >= _count)
                return -1;
            return (_index - _count + index) % _array.Length;
        }
    }


}
