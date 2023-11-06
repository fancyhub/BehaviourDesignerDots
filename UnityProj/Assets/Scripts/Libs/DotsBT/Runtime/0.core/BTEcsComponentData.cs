using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace DotsBT
{
    //索引
    public struct BTBlobVarItemIndex
    {
        public int NameId;
        public ushort _VarType;
        public ushort VarlueSize;
        public BTPtr Ptr;

        public EBTVar VarMeta { get => (EBTVar)_VarType; set => _VarType = (ushort)value; }
    }

    public struct BTBlobEntity
    {
        public static BTBlobEntity Null => new BTBlobEntity() { Entity = Entity.Null, DataOffset = 0 };
        public Entity Entity;
        public ushort DataOffset;

        public static BTBlobEntity Create(Entity e, BTPtr ptr)
        {
            return new BTBlobEntity() { Entity = e, DataOffset = ptr.Offset };
        }
    }

    //[StructLayout(LayoutKind.Sequential, Size = ELEMENT_SIZE * CAP + HEADER_SIZE)]
    public unsafe struct BTBlobEntityList
    {
        public const int CAP = 10;
        public const int ELEMENT_SIZE = 8;
        public const int HEADER_SIZE = 4;
        public int Count;
        public BTBlobEntity _e0;

        //下面的必须要写出来,要不然 Untiy 在加载的时候, 不会帮我动态修改
        public BTBlobEntity _e1;
        public BTBlobEntity _e2;
        public BTBlobEntity _e3;
        public BTBlobEntity _e4;
        public BTBlobEntity _e5;
        public BTBlobEntity _e6;
        public BTBlobEntity _e7;
        public BTBlobEntity _e8;
        public BTBlobEntity _e9;

        public bool Add(BTBlobEntity e)
        {
            if (Count >= CAP)
                return false;

            Count++;
            *GetPtr(Count - 1) = e;
            return true;
        }

        public BTBlobEntity this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    return BTBlobEntity.Null;
                return *GetPtr(index);
            }
        }

        public BTBlobEntity* GetPtr(int index)
        {
            if (index < 0 || index >= Count)
                return null;
            fixed (BTBlobEntity* p = &_e0)
            {
                return p + index;
            }
        }

        public static BTBlobEntityList Create(List<BTBlobEntity> list)
        {
            if (list.Count > BTBlobEntityList.CAP)
                throw new Exception("Entity Size 超过了 " + BTBlobEntityList.CAP);

            BTBlobEntityList ret = new BTBlobEntityList();
            foreach (var p in list)
                ret.Add(p);
            return ret;
        }
    }

    public struct BTBlobVars
    {
        public MyBlobArray<BTBlobVarItemIndex> NameList;
        public BTBlobEntityList EntityList;
        public MyBlobArray<byte> Data;

        public readonly bool CheckVars(BTAssetData data, EBTVarScope scope)
        {
            if (data == null)
                return false;
            BTAssetVar[] var_list = data.UsedVarList;
            if (var_list == null)
                return true;

            foreach (var v in var_list)
            {
                if (v.Scope != scope)
                    continue;

                int name_index = -1;
                for (int i = 0; i < NameList.Length; i++)
                {
                    if (NameList[i].NameId == v.NameId)
                    {
                        name_index = i;
                        break;
                    }
                }
                if (name_index < 0)
                {
                    UnityEngine.Debug.LogError($"行为树 {data.Name} 找不到变量 {v.Name}, Pre Link 失败");
                    return false;
                }
                var name_item = NameList[name_index];

                if (name_item.VarMeta != v.VarMeta)
                {
                    UnityEngine.Debug.LogError($"行为树 {data.Name} 变量 {v.Name} 类型不匹配, Pre Link 失败");
                    return false;
                }

                if (name_item.VarlueSize != v.ValueSize)
                {
                    UnityEngine.Debug.LogError($"行为树 {data.Name} 变量 {v.Name} 大小不匹配, Pre Link 失败");
                    return false;
                }
            }
            return true;
        }

        public unsafe NativeArray<byte> CreateRuntime(Allocator allocator)
        {
            var ret = Data.Clone2NativeArray(allocator);
            for (int i = 0; i < EntityList.Count; i++)
            {
                BTBlobEntity entity = EntityList[i];

                Entity* pEntity = (Entity*)((byte*)ret.GetUnsafePtr() + entity.DataOffset);
                *pEntity = entity.Entity;
            }

            return ret;
        }
    }

    public struct BTAssetCompData : IComponentData
    {
        public Entity GlobalVarEntity;

        public BTBlobVars Vars;
        public Unity.Entities.Content.WeakObjectReference<BTAsset> Image;
    }

    public struct BTCleanUpComp : ICleanupComponentData
    {
        //这些都是需要析构的
        public NativeArray<byte> ImageData;
        public NativeArray<byte> SharedVarData;

        public void Dispose()
        {
            ImageData.Dispose();
            SharedVarData.Dispose();
        }
    }

    public struct BTGlobalCompData : IComponentData
    {
        public BTBlobVars Vars;
    }

    public struct BTComponentRunTimeData : IComponentData
    {
        public bool Valid;
        public EBTStatus Status;

        //运行时的数据        
        public BTPtr RootTask;
        public BTMemory Memory;
        public BTBlackBoard BlackBoard;

        public float DeltaTime;
    }
}
