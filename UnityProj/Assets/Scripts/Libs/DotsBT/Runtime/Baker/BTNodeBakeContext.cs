using System;
using System.Collections;
using System.Collections.Generic;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.Entities;
using UnityEngine;

namespace DotsBT
{
    //BTNodeBakeContext
    public class BTBakeContextNode
    {
        public BTMemoryAllocator Allocator;
        public short NodeIdGenIndex;
        public List<string> NodeNames = new List<string>();
        public UsedVar Var = new UsedVar();

        public void UseVar(SharedVariable var, EBTVarScope scope, ref BTPtr ptr)
        {
            BTPtr ptr_self = Allocator.GetPtr(ref ptr);
            if (ptr_self == BTPtr.Null)
            {
                UnityEngine.Debug.LogError("指针错误");
                return;
            }

            Var.UseVar(var, scope, ref ptr, ptr_self);
        }

        public class UsedVar
        {
            public Dictionary<string, SharedVariable> AllSharedVar = new Dictionary<string, SharedVariable>();
            public Dictionary<string, BTPtr> UsedSharedVar = new();

            public Dictionary<string, SharedVariable> AllGlobalvar = new Dictionary<string, SharedVariable>();
            public Dictionary<string, BTPtr> UsedGlobalVar = new();

            public void SetAllVar(List<SharedVariable> shared_var, List<SharedVariable> global_var)
            {
                AllSharedVar.Clear();
                AllGlobalvar.Clear();
                UsedSharedVar.Clear();
                UsedGlobalVar.Clear();

                if(shared_var!=null)
                {
                    foreach (var p in shared_var)
                    {
                        AllSharedVar.Add(p.Name, p);
                    }
                }
                

                foreach (var p in global_var)
                {
                    AllGlobalvar.Add(p.Name, p);
                }
            }

            public BTAssetVar[] GetUsedVarList()
            {
                List<BTAssetVar> ret = new List<BTAssetVar>();
                foreach (var p in UsedSharedVar)
                {
                    SharedVariable v = AllSharedVar[p.Key];
                    if (!BTVarVT.GetInfo(v, out var info))
                    {
                        throw new Exception($"找不到 对应的 Var 类型  {v.GetType()}");
                    }

                    ret.Add(new BTAssetVar()
                    {
                        Scope = EBTVarScope.Share,
                        Name = v.Name,
                        NameId = BTUtil.Name2Id(v.Name),
                        VarMeta = info.Meta,
                        ValueSize = info.ValueSize,
                        LinkPtr = p.Value,
                    });
                }

                foreach (var p in UsedGlobalVar)
                {
                    SharedVariable v = AllGlobalvar[p.Key];
                    if (!BTVarVT.GetInfo(v, out var info))
                    {
                        throw new Exception($"找不到 对应的 Var 类型  {v.GetType()}");
                    }

                    ret.Add(new BTAssetVar()
                    {
                        Scope = EBTVarScope.Global,
                        Name = v.Name,
                        NameId = BTUtil.Name2Id(v.Name),
                        VarMeta = info.Meta,
                        ValueSize = info.ValueSize,
                        LinkPtr = p.Value,
                    });
                }

                return ret.ToArray();
            }

            public void UseVar(SharedVariable var, EBTVarScope scope, ref BTPtr ptr, BTPtr ptr_self)
            {
                Dictionary<string, SharedVariable> dict = null;
                Dictionary<string, BTPtr> set = null;

                switch (scope)
                {
                    default:
                    case EBTVarScope.Local:
                        return;

                    case EBTVarScope.Share:
                        {
                            dict = AllSharedVar;
                            set = UsedSharedVar;
                        }
                        break;
                    case EBTVarScope.Global:
                        {
                            dict = AllGlobalvar;
                            set = UsedGlobalVar;
                        }
                        break;
                }

                dict.TryGetValue(var.Name, out var old_p);
                if (old_p == null)
                {
                    throw new Exception($"找不到 变量 {scope} :  {var.Name}  IsShared: {var.IsShared}  IsGlobal: {var.IsGlobal}  IsDynamic: {var.IsDynamic}");
                }

                if (old_p.GetType() != var.GetType())
                {
                    throw new Exception($"变量类型不一致 {var.Name}");
                }

                if (set.TryGetValue(var.Name, out var old_ptr))
                {
                    ptr = old_ptr;
                    set[var.Name] = ptr_self;
                }
                else
                {
                    ptr = BTPtr.Null;
                    set[var.Name] = ptr_self;
                }
            }
        }

    }

    public class BTBakeContextVar
    {
        public BTMemoryAllocator Allocator;
        public IBaker Baker;

        public List<BTBlobEntity> EntityList = new List<BTBlobEntity>();
    }
}
