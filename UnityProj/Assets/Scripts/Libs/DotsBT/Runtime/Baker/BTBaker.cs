using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Entities.Serialization;

namespace DotsBT
{
    public unsafe static class BTBaker
    {
        public const int C_BAKER_BUFF_SIZE = 10240;

        public static BTBlobVars BakeSharedVars(IBaker owner, Behavior bd_behavior_tree)
        {
            return BakeVars(owner, GetSharedVariables(bd_behavior_tree), EBTVarScope.Share);
        }

        public static BTBlobVars BakeGlobalVars(IBaker owner, List<SharedVariable> list)
        {
            return BakeVars(owner, list, EBTVarScope.Global);
        }

        public static List<SharedVariable> GetSharedVariables(Behavior bd_behavior_tree)
        {
            BehaviorSource src = bd_behavior_tree.GetBehaviorSource();
            src.ExDeserialization();
            return src.Variables;
        }

        public static BTAssetData BakeAsset(ExternalBehavior asset)
        {
            BehaviorSource src = asset.GetBehaviorSource();
            src.ExDeserialization();

            byte* temp = stackalloc byte[C_BAKER_BUFF_SIZE];
            BTSegMemory memory = new BTSegMemory(EBTMemSeg.Exe, true, temp, C_BAKER_BUFF_SIZE);

            var context = new BTBakeContextNode();
            context.Allocator = new BTMemoryAllocator(memory, 4);

            context.Var.SetAllVar(src.Variables, GlobalVariables.Instance.GetAllVariables());

            BTVM vm = new BTVM()
            {
                Memory = new BTMemory() { Exe = memory }
            };

            BTPtr root = BTPtr.Null;
            if (src.RootTask != null)
                root = vm.NewNode(src.RootTask, context);

            BTAssetData ret = new BTAssetData();
            ret.Name = asset.name;
            ret.Data = memory.Clone<byte>(context.Allocator.AllocatedSize);
            ret.RootTask = root;
            ret.NodeName = context.NodeNames.ToArray();
            ret.UsedVarList = context.Var.GetUsedVarList();
            return ret;
        }

        public static BTBlobVars BakeVars(IBaker baker, List<SharedVariable> var_list, EBTVarScope scope)
        {
            byte* temp = stackalloc byte[C_BAKER_BUFF_SIZE];
            EBTMemSeg seg = EBTMemSeg.SharedVar;
            if (scope == EBTVarScope.Global)
                seg = EBTMemSeg.GlobalVar;

            BTSegMemory memory = new BTSegMemory(seg, true, temp, C_BAKER_BUFF_SIZE);
            BTBakeContextVar context = new BTBakeContextVar();
            context.Baker = baker;
            context.Allocator = new BTMemoryAllocator(memory);
            Dictionary<int, (string Name, BTBlobVarItemIndex Blob)> dict = new();

            if(var_list!=null)
            {
                foreach (var var in var_list)
                {
                    string name = var.Name;
                    int NameId = BTUtil.Name2Id(name);
                    if (dict.TryGetValue(NameId, out var item))
                        throw new Exception("已经注册过了 " + name);


                    if (!BTVarVT.GetInfo(var, out var info))
                    {
                        throw new Exception("");
                    }

                    item.Name = name;
                    item.Blob.NameId = NameId;
                    item.Blob.VarlueSize = (ushort)info.ValueSize;
                    item.Blob.VarMeta = info.Meta;
                    item.Blob.Ptr = BTVarVT.BakeVar(context, var);

                    dict.Add(NameId, item);
                }
            }

            BTBlobVars ret = new BTBlobVars();

            ret.EntityList = BTBlobEntityList.Create(context.EntityList);

            {
                List<BTBlobVarItemIndex> name_list = new List<BTBlobVarItemIndex>(dict.Count);
                foreach (var p in dict)
                {
                    //BTLog.E($"添加 {p.Value.Name}  {p.Value.Blob.NameId} {p.Value.Blob.VarSize} {p.Value.Blob.VarType}  {p.Value.Blob.Ptr}");
                    name_list.Add(p.Value.Blob);
                }
                ret.NameList = MyBlobArray.Create(name_list);
            }
            ret.Data = MyBlobArray.Create(memory.Clone<byte>(context.Allocator.AllocatedSize));
            return ret;
        }


#if UNITY_EDITOR

        public static UnityEngine.Object EdLoadRes(Unity.Entities.Hash128 guid, Type type)
        {
            if (!guid.IsValid)
                return null;
            string str_guid = guid.ToString();

            string str_path = UnityEditor.AssetDatabase.GUIDToAssetPath(str_guid);
            if (string.IsNullOrEmpty(str_path))
                return null;

            return UnityEditor.AssetDatabase.LoadAssetAtPath(str_path, type);
        }

        public static T EdLoadRes<T>(Unity.Entities.Hash128 guid) where T : UnityEngine.Object
        {
            if (!guid.IsValid)
                return null;
            string str_guid = guid.ToString();

            string str_path = UnityEditor.AssetDatabase.GUIDToAssetPath(str_guid);
            if (string.IsNullOrEmpty(str_path))
                return null;

            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(str_path);
        }

        public static T EdGetWeakRefObj<T>(Unity.Entities.Content.WeakObjectReference<T> self) where T : UnityEngine.Object
        {            
            var self_str = self.ToString();
            int start_index = self_str.LastIndexOf(' ');
            int end_index = self_str.LastIndexOf(':');
            if (start_index < 0 || end_index < 0)
                return null;

            start_index++;
            var str_guid = self_str.Substring(start_index,end_index- start_index);

            string str_path = UnityEditor.AssetDatabase.GUIDToAssetPath(str_guid);
            if (string.IsNullOrEmpty(str_path))
                return null;

            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(str_path);         
        }

        public static void EdSetWeakRefObj<T>(ref Unity.Entities.Content.WeakObjectReference<T> self, T obj) where T : UnityEngine.Object
        {
            self = new WeakObjectReference<T>(obj);            
        }

        public static Unity.Entities.Content.WeakObjectReference<T> CreateWeakObjRef<T>(T obj) where T : UnityEngine.Object
        {
            return new Unity.Entities.Content.WeakObjectReference<T>(obj);
        }
#endif
    }
}
