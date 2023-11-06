using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;

namespace DotsBT
{
    public enum EBTVar : ushort
    {
        None,
        VarBool,
        VarInt,
        VarFloat,
        VarVector2,
        VarVector3,
        VarVector4,
        VarQuaternion,
        VarColor,
        VarVector2Int,
        VarVector3Int,
        VarRect,
        VarLayerMask,
        VarRangeFloat,
        VarGameObject,
        VarBtExVarEnum,
        VarInutButton,
        VarTimerData,
    }

    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class BTVarMetaAttribute : Attribute
    {
        public EBTVar Meta;

        public BTVarMetaAttribute(EBTVar var_meta)
        {
            Meta = var_meta;
        }
    }
}
