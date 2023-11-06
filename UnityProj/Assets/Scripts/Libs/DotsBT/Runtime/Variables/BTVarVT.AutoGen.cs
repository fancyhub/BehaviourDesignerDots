//Auto Gen
using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
namespace DotsBT
{
    public unsafe static partial class BTVarVT
    {

        public struct VarInfo
        {
            public EBTVar Meta;

            public Type Type;
            public int Size;

            public Type ValueType;
            public int ValueSize;

            public Type BDType;
            public Type BDValueType;

            public static VarInfo Create<TBTType, TBTValueType, TBDType, TBDValueType>(EBTVar meta)
                where TBTType : unmanaged, IBTVar
                where TBTValueType : unmanaged
                where TBDType : BehaviorDesigner.Runtime.SharedVariable<TBDValueType>
            {
                return new VarInfo()
                {
                    Meta = meta,

                    Type = typeof(TBTType),
                    Size = sizeof(TBTType),

                    ValueType = typeof(TBTValueType),
                    ValueSize = sizeof(TBTValueType),

                    BDType = typeof(TBDType),
                    BDValueType = typeof(TBDValueType)
                };
            }
        }

		private static readonly Dictionary<Type, VarInfo> _BDVarInfoDict = new(){
			{typeof(BehaviorDesigner.Runtime.SharedVector3),VarInfo.Create<DotsBT.BTVarVector3,UnityEngine.Vector3,BehaviorDesigner.Runtime.SharedVector3,UnityEngine.Vector3>(EBTVar.VarVector3)},
			{typeof(BehaviorDesigner.Runtime.SharedBool),VarInfo.Create<DotsBT.BTVarBool,System.Boolean,BehaviorDesigner.Runtime.SharedBool,System.Boolean>(EBTVar.VarBool)},
			{typeof(BehaviorDesigner.Runtime.SharedVector2Int),VarInfo.Create<DotsBT.BTVarVector2Int,UnityEngine.Vector2Int,BehaviorDesigner.Runtime.SharedVector2Int,UnityEngine.Vector2Int>(EBTVar.VarVector2Int)},
			{typeof(BehaviorDesigner.Runtime.SharedRect),VarInfo.Create<DotsBT.BTVarRect,UnityEngine.Rect,BehaviorDesigner.Runtime.SharedRect,UnityEngine.Rect>(EBTVar.VarRect)},
			{typeof(BehaviorDesigner.Runtime.SharedVector3Int),VarInfo.Create<DotsBT.BTVarVector3Int,UnityEngine.Vector3Int,BehaviorDesigner.Runtime.SharedVector3Int,UnityEngine.Vector3Int>(EBTVar.VarVector3Int)},
			{typeof(DotsBT.SharedTimerData),VarInfo.Create<DotsBT.BTVarTimerData,DotsBT.TimerData,DotsBT.SharedTimerData,DotsBT.TimerData>(EBTVar.VarTimerData)},
			{typeof(BehaviorDesigner.Runtime.SharedQuaternion),VarInfo.Create<DotsBT.BTVarQuaternion,UnityEngine.Quaternion,BehaviorDesigner.Runtime.SharedQuaternion,UnityEngine.Quaternion>(EBTVar.VarQuaternion)},
			{typeof(BehaviorDesigner.Runtime.SharedVector2),VarInfo.Create<DotsBT.BTVarVector2,UnityEngine.Vector2,BehaviorDesigner.Runtime.SharedVector2,UnityEngine.Vector2>(EBTVar.VarVector2)},
			{typeof(BehaviorDesigner.Runtime.SharedLayerMask),VarInfo.Create<DotsBT.BTVarLayerMask,UnityEngine.LayerMask,BehaviorDesigner.Runtime.SharedLayerMask,UnityEngine.LayerMask>(EBTVar.VarLayerMask)},
			{typeof(BehaviorDesigner.Runtime.SharedVector4),VarInfo.Create<DotsBT.BTVarVector4,UnityEngine.Vector4,BehaviorDesigner.Runtime.SharedVector4,UnityEngine.Vector4>(EBTVar.VarVector4)},
			{typeof(BehaviorDesigner.Runtime.SharedInt),VarInfo.Create<DotsBT.BTVarInt,System.Int32,BehaviorDesigner.Runtime.SharedInt,System.Int32>(EBTVar.VarInt)},
			{typeof(BehaviorDesigner.Runtime.SharedGameObject),VarInfo.Create<DotsBT.BTVarGameObject,Unity.Entities.Entity,BehaviorDesigner.Runtime.SharedGameObject,UnityEngine.GameObject>(EBTVar.VarGameObject)},
			{typeof(BehaviorDesigner.Runtime.SharedFloat),VarInfo.Create<DotsBT.BTVarFloat,System.Single,BehaviorDesigner.Runtime.SharedFloat,System.Single>(EBTVar.VarFloat)},
			{typeof(BehaviorDesigner.Runtime.SharedColor),VarInfo.Create<DotsBT.BTVarColor,UnityEngine.Color,BehaviorDesigner.Runtime.SharedColor,UnityEngine.Color>(EBTVar.VarColor)},
		};

		private static readonly Dictionary<EBTVar, VarInfo> _BTVarInfoDict = new(){
			{EBTVar.VarVector3,VarInfo.Create<DotsBT.BTVarVector3,UnityEngine.Vector3,BehaviorDesigner.Runtime.SharedVector3,UnityEngine.Vector3>(EBTVar.VarVector3)},
			{EBTVar.VarBool,VarInfo.Create<DotsBT.BTVarBool,System.Boolean,BehaviorDesigner.Runtime.SharedBool,System.Boolean>(EBTVar.VarBool)},
			{EBTVar.VarVector2Int,VarInfo.Create<DotsBT.BTVarVector2Int,UnityEngine.Vector2Int,BehaviorDesigner.Runtime.SharedVector2Int,UnityEngine.Vector2Int>(EBTVar.VarVector2Int)},
			{EBTVar.VarRect,VarInfo.Create<DotsBT.BTVarRect,UnityEngine.Rect,BehaviorDesigner.Runtime.SharedRect,UnityEngine.Rect>(EBTVar.VarRect)},
			{EBTVar.VarVector3Int,VarInfo.Create<DotsBT.BTVarVector3Int,UnityEngine.Vector3Int,BehaviorDesigner.Runtime.SharedVector3Int,UnityEngine.Vector3Int>(EBTVar.VarVector3Int)},
			{EBTVar.VarTimerData,VarInfo.Create<DotsBT.BTVarTimerData,DotsBT.TimerData,DotsBT.SharedTimerData,DotsBT.TimerData>(EBTVar.VarTimerData)},
			{EBTVar.VarQuaternion,VarInfo.Create<DotsBT.BTVarQuaternion,UnityEngine.Quaternion,BehaviorDesigner.Runtime.SharedQuaternion,UnityEngine.Quaternion>(EBTVar.VarQuaternion)},
			{EBTVar.VarVector2,VarInfo.Create<DotsBT.BTVarVector2,UnityEngine.Vector2,BehaviorDesigner.Runtime.SharedVector2,UnityEngine.Vector2>(EBTVar.VarVector2)},
			{EBTVar.VarLayerMask,VarInfo.Create<DotsBT.BTVarLayerMask,UnityEngine.LayerMask,BehaviorDesigner.Runtime.SharedLayerMask,UnityEngine.LayerMask>(EBTVar.VarLayerMask)},
			{EBTVar.VarVector4,VarInfo.Create<DotsBT.BTVarVector4,UnityEngine.Vector4,BehaviorDesigner.Runtime.SharedVector4,UnityEngine.Vector4>(EBTVar.VarVector4)},
			{EBTVar.VarInt,VarInfo.Create<DotsBT.BTVarInt,System.Int32,BehaviorDesigner.Runtime.SharedInt,System.Int32>(EBTVar.VarInt)},
			{EBTVar.VarGameObject,VarInfo.Create<DotsBT.BTVarGameObject,Unity.Entities.Entity,BehaviorDesigner.Runtime.SharedGameObject,UnityEngine.GameObject>(EBTVar.VarGameObject)},
			{EBTVar.VarFloat,VarInfo.Create<DotsBT.BTVarFloat,System.Single,BehaviorDesigner.Runtime.SharedFloat,System.Single>(EBTVar.VarFloat)},
			{EBTVar.VarColor,VarInfo.Create<DotsBT.BTVarColor,UnityEngine.Color,BehaviorDesigner.Runtime.SharedColor,UnityEngine.Color>(EBTVar.VarColor)},
		};

        public static BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            EBTVar meta = GetMeta(var);
            switch (meta)
            {
				case EBTVar.VarVector3: return new DotsBT.BTVarVector3().BakeVar(context, var);
				case EBTVar.VarBool: return new DotsBT.BTVarBool().BakeVar(context, var);
				case EBTVar.VarVector2Int: return new DotsBT.BTVarVector2Int().BakeVar(context, var);
				case EBTVar.VarRect: return new DotsBT.BTVarRect().BakeVar(context, var);
				case EBTVar.VarVector3Int: return new DotsBT.BTVarVector3Int().BakeVar(context, var);
				case EBTVar.VarTimerData: return new DotsBT.BTVarTimerData().BakeVar(context, var);
				case EBTVar.VarQuaternion: return new DotsBT.BTVarQuaternion().BakeVar(context, var);
				case EBTVar.VarVector2: return new DotsBT.BTVarVector2().BakeVar(context, var);
				case EBTVar.VarLayerMask: return new DotsBT.BTVarLayerMask().BakeVar(context, var);
				case EBTVar.VarVector4: return new DotsBT.BTVarVector4().BakeVar(context, var);
				case EBTVar.VarInt: return new DotsBT.BTVarInt().BakeVar(context, var);
				case EBTVar.VarGameObject: return new DotsBT.BTVarGameObject().BakeVar(context, var);
				case EBTVar.VarFloat: return new DotsBT.BTVarFloat().BakeVar(context, var);
				case EBTVar.VarColor: return new DotsBT.BTVarColor().BakeVar(context, var);
                default:
                    throw new Exception($"未知类型 {meta}");
            }
        }
	}
}
