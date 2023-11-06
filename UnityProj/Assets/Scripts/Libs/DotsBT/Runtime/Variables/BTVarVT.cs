using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Unity.Entities;

namespace DotsBT
{
    public unsafe static partial class BTVarVT
    {
        public static bool GetInfo(SharedVariable var, out VarInfo info)
        {
            if (var == null)
            {
                info = default;
                return false;
            }

            return _BDVarInfoDict.TryGetValue(var.GetType(), out info);
        }

        public static EBTVar GetMeta(SharedVariable var)
        {
            if (GetInfo(var, out var info))
                return info.Meta;
            return EBTVar.None;
        }


        public static object GetBoxedValue(ref BTMemory memory, BTPtr ptr, EBTVar meta)
        {
            if (ptr.IsNull)
                return null;

            switch (meta)
            {
                case EBTVar.VarBool: return memory.GetValue<bool>(ptr);
                case EBTVar.VarInt: return memory.GetValue<int>(ptr);
                case EBTVar.VarFloat: return memory.GetValue<float>(ptr);
                case EBTVar.VarVector3: return memory.GetValue<UnityEngine.Vector3>(ptr);
                case EBTVar.VarGameObject: return memory.GetValue<Entity>(ptr);
                case EBTVar.VarBtExVarEnum: return memory.GetValue<Entity>(ptr);
                default: return null;
            }
        }
    }
}
