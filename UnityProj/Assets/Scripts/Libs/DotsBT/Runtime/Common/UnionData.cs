using System;
using System.Collections.Generic;

namespace DotsBT
{
    //1. 不能直接使用, 需要在外面包一层, 父结构需要显示指定大小
    //2. 在父结构里面, 需要放在最后一个
    public struct UnionData
    {
        private byte _data;
        public unsafe bool Set<T>(int cap, in T data) where T : unmanaged
        {
            if (sizeof(T) > cap)
            {
                UnityEngine.Debug.LogError($"类型 sizeof({typeof(T)}) > {cap}");
                return false;
            }

            fixed (byte* p_body = &_data)
            {
                T* pT = (T*)p_body;
                *pT = data;
            }
            return true;
        }

        public unsafe bool TryGet<T>(int cap, out T data) where T : unmanaged
        {
            if (sizeof(T) > cap)
            {
                data = default;
                UnityEngine.Debug.LogError($"类型 sizeof({typeof(T)}) > {cap}");
                return false;
            }

            fixed (byte* p_body = &_data)
            {
                T* pT = (T*)p_body;
                data = *pT;
            }
            return true;
        }
    }

}
