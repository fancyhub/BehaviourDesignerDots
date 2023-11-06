using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace DotsBT
{
    public struct BTBlackBoard : IBTBlackBoard
    {
        public Entity _owner;
        public MyBlobArray<BTBlobVarItemIndex> _shared_var;
        public MyBlobArray<BTBlobVarItemIndex> _global_var;
        public BTMemory _memory;

        public BTBlackBoard(Entity owner, MyBlobArray<BTBlobVarItemIndex> shared_var, MyBlobArray<BTBlobVarItemIndex> global_var, BTMemory memory)
        {
            _owner = owner;
            _shared_var = shared_var;
            _global_var = global_var;
            _memory = memory;
        }

        public BTPtr LinkVar(int name_id, EBTVarScope scope)
        {
            if (!_Find(name_id, scope, out var index))
            {
                UnityEngine.Debug.LogError("Shared Var Link error : " + name_id);
                return BTPtr.Null;
            }
            return index.Ptr;
        }

        public Entity GetOwnerEntity()
        {
            return _owner;
        }

        public T GetValue<T>(int name_id, EBTVarScope scope) where T : unmanaged
        {
            if (!_Find(name_id, scope, out var index))
            {
                UnityEngine.Debug.LogError("找不到变量 " + name_id);
                return default;
            }
            return _memory.GetValue<T>(index.Ptr);
        }

        public bool SetSharedValue<T>(int name_id, T value) where T : unmanaged
        {
            if (!_Find(name_id, EBTVarScope.Share, out var index))
            {
                UnityEngine.Debug.LogError("找不到变量 " + name_id);
                return false;
            }
            return _memory.SetValue<T>(index.Ptr, value);
        }

        public void EdGetBoxedValues(List<BTBBBoxedValue> all_values)
        {
            all_values.Clear();
            for (int i = 0; i < _shared_var.Length; i++)
            {
                var vv = _shared_var[i];

                all_values.Add(new BTBBBoxedValue()
                {
                    Scope = EBTVarScope.Share,
                    NameId = vv.NameId,
                    Value = BTVarVT.GetBoxedValue(ref _memory, vv.Ptr, vv.VarMeta),
                });
            }

            for (int i = 0; i < _global_var.Length; i++)
            {
                var vv = _global_var[i];

                all_values.Add(new BTBBBoxedValue()
                {
                    Scope = EBTVarScope.Global,
                    NameId = vv.NameId,
                    Value = BTVarVT.GetBoxedValue(ref _memory, vv.Ptr, vv.VarMeta),
                });
            }
        }

        private bool _Find(int name_id, EBTVarScope scope, out BTBlobVarItemIndex index)
        {
            ref MyBlobArray<BTBlobVarItemIndex> var_dict = ref _shared_var;
            if (scope == EBTVarScope.Share)
            {
                var_dict = ref _shared_var;
            }
            else if (scope == EBTVarScope.Global)
            {
                var_dict = ref _global_var;
            }
            else
            {
                index = default;
                return false;
            }

            for (int i = 0; i < var_dict.Length; i++)
            {
                if (var_dict[i].NameId == name_id)
                {
                    index = var_dict[i];
                    return true;
                }
            }

            index = default;
            return false;
        }
    }
}
