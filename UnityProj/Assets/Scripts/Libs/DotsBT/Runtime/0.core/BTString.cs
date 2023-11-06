using System;
using System.Runtime.InteropServices;

namespace DotsBT
{
    public struct BTString
    {
        public BTPtr Ptr;
        public int Length; //不能修改, 除了Ctor
        public unsafe void Ctor(ref BTMemoryAllocator allocator, string v)
        {
            if (v == null || v.Length == 0)
            {
                Ptr = BTPtr.Null;
                Length = 0;
                return;
            }

            byte[] buff = System.Text.Encoding.UTF8.GetBytes(v);
            this.Ptr = allocator.AllocArrayAndSet(buff);
            this.Length = buff.Length;
        }

        public unsafe string GetStr(ref BTVM vm)
        {
            if (Length == 0)
                return string.Empty;
            byte* p_data = vm.Get<byte>(Ptr, false);
            return System.Text.Encoding.UTF8.GetString(p_data, Length);
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
