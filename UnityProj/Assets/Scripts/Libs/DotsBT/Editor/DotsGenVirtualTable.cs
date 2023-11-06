#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;

namespace DotsBT.ED
{
    public unsafe static class EdBTUtil
    {
        public const string C_Node_FILE = "Assets/Scripts/Libs/DotsBT/Runtime/Nodes/BTNodeVT.AutoGen.cs";
        public const string C_Var_FILE = "Assets/Scripts/Libs/DotsBT/Runtime/Variables/BTVarVT.AutoGen.cs";

        [UnityEditor.MenuItem("Tools/Behaivor Designer(Dots)/Gen Virtual Table",priority =200)]
        public static void Gen()
        {
            GenNodeVT(true);
            GenVarVT(true);

            UnityEditor.AssetDatabase.ImportAsset(C_Node_FILE);
            UnityEditor.AssetDatabase.ImportAsset(C_Var_FILE);
        }

        [UnityEditor.MenuItem("Tools/Behaivor Designer(Dots)/Clear Virtual Table", priority = 200)]
        public static void Clear()
        {
            GenNodeVT(false);
            GenVarVT(false);
            UnityEditor.AssetDatabase.ImportAsset(C_Node_FILE);
            UnityEditor.AssetDatabase.ImportAsset(C_Var_FILE);
        }

        public class EdBTFuncParamInfo
        {
            public string Name;
            public string Prefix; // ref, in, out 
            public string TypeName;

            public string GetDeclareStr()
            {
                return $"{Prefix} {TypeName} {Name}";
            }
            public string GetCallStr()
            {
                return $"{Prefix} {Name}";
            }

            public static EdBTFuncParamInfo Create(System.Reflection.ParameterInfo info)
            {
                EdBTFuncParamInfo ret = new EdBTFuncParamInfo();
                ret.Name = info.Name;
                ret.TypeName = info.ParameterType.FullName;
                if (ret.TypeName.EndsWith("&"))
                {
                    ret.Prefix = "ref";
                    ret.TypeName = ret.TypeName.Substring(0, ret.TypeName.Length - 1);
                }
                else if (info.IsIn && info.IsOut)
                    ret.Prefix = "ref";
                else if (info.IsIn)
                    ret.Prefix = "in";
                else if (info.IsOut)
                    ret.Prefix = "out";
                return ret;
            }
        }

        public class EdBTFuncInfo
        {
            public string Name;
            public Type RetType;
            public List<EdBTFuncParamInfo> ParamList = new List<EdBTFuncParamInfo>();


            public string GetDeclareStr()
            {
                if (RetType == null)
                    return "void";
                else
                    return RetType.FullName;
            }
        }

        public class EdBTNodeInfo
        {
            public EBTNode SelfMeta;
            public EBTNode ParentMeta;
            public Type SelfType;
            public List<Type> BDTypes = new List<Type>();

            public bool IsAllAbstract()
            {
                if (BDTypes.Count == 0)
                    return true;
                foreach (var p in BDTypes)
                    if (!p.IsAbstract)
                        return false;
                return true;
            }
        }

        public class EdBTVarInfo
        {
            public EBTVar Meta;
            public Type Type;
            public Type ValueType;

            public Type BD_Type;
            public Type BD_ValueType;
        }    

        public static void GenNodeVT(bool gen)
        {
            List<EdBTNodeInfo> node_list;
            List<EdBTFuncInfo> func_list= _EdGetFuncList<IBTNode>();
            //VersionControlTools.Checkout(C_Node_FILE);

            if (gen)
            {
                node_list = _EDCollectNodeInfos();
            }
            else
            {
                node_list = new List<EdBTNodeInfo>();
            }

            using System.IO.StreamWriter sw = new System.IO.StreamWriter(C_Node_FILE);

            sw.WriteLine(@"//Auto Gen
using System;
using System.Collections.Generic;
using AOT;
using Unity.Burst;
namespace DotsBT
{
    //[BurstCompile]
    public unsafe static partial class BTNodeVT
    {");

            _Write_NodeTypeMap(sw, node_list);

            foreach (var func in func_list)
            {
                _Write_NodeFuncDict(sw, func, node_list);
            }

            sw.WriteLine("\t}\n}");
        }

        public static void GenVarVT(bool gen)
        {
            List<EdBTVarInfo> var_list = null;
            if (gen)
                var_list = _EdCollectVarInfos();
            else
                var_list = new List<EdBTVarInfo>();

            //VersionControlTools.Checkout(C_Var_FILE);
            using System.IO.StreamWriter sw = new System.IO.StreamWriter(C_Var_FILE);

            sw.WriteLine(@"//Auto Gen
using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
namespace DotsBT
{
    public unsafe static partial class BTVarVT
    {");

            _Write_VarTypeMap(sw, var_list);
            WriteBakeVarFunc(sw, var_list);

            sw.WriteLine("\t}\n}");
        }

        private static void _Write_NodeFuncDict(StreamWriter sw, EdBTFuncInfo func_info, List<EdBTNodeInfo> node_list)
        {
            sw.WriteLine($"\t\t#region NodeAction_{func_info.Name}");

            string name_delegate = $"NodeAction_{func_info.Name}";

            string str_pararm_declare_list = "";//参数列表的声明
            {
                str_pararm_declare_list = $"{typeof(BTNodeBase).FullName}* p";
                foreach (var p in func_info.ParamList)
                {
                    str_pararm_declare_list += $", {p.GetDeclareStr()}";
                }
            }

            string str_param_call_list = "";//参数列表的调用
            {
                for (int i = 0; i < func_info.ParamList.Count; i++)
                {
                    var p = func_info.ParamList[i];
                    if (i != 0)
                        str_param_call_list += ",";
                    str_param_call_list += $"{p.GetCallStr()}";
                }
            }

            //写Delegate
            sw.WriteLine($"\t\tpublic delegate {func_info.GetDeclareStr()} {name_delegate}({str_pararm_declare_list});");

            var isCtor = func_info.Name == "Ctor";
            //写Dict
            {
                string line_format;
                if (isCtor)
                {
                    line_format = $"{typeof(EBTNode).FullName}.{{0}},_NodeAction_{func_info.Name}_{{0}}";
                    sw.WriteLine($"\t\tprivate static readonly Dictionary<{typeof(EBTNode).FullName}, {name_delegate}> _NodeAction_{func_info.Name}Dict = new()");
                }
                else
                {
                    line_format =
                        $"{typeof(EBTNode).FullName}.{{0}},_NodeAction_{func_info.Name}_{{0}}";
                    sw.WriteLine($"\t\tpublic static readonly Dictionary<{typeof(EBTNode).FullName}, NodeAction_{func_info.Name}> _NodeAction_{func_info.Name}Dict = new()");
                }
                sw.WriteLine("\t\t{");
                foreach (EdBTNodeInfo node in node_list)
                {
                    if (node.IsAllAbstract())
                        continue;
                    if (isCtor)
                    {
                        sw.WriteLine($"\t\t\t{{{string.Format(line_format, node.SelfMeta)}}},");
                    }
                    else
                    {
                        sw.WriteLine($"\t\t\t{{{string.Format(line_format, node.SelfMeta)}}},");
                    }
                }
                sw.WriteLine("\t\t};");
            }

            //写函数列表
            {
                foreach (EdBTNodeInfo node in node_list)
                {
                    if (node.IsAllAbstract())
                        continue;
                    if (!isCtor)
                    {
                        //sw.Write($"\t\t[BurstCompile]\n\t\t[AOT.MonoPInvokeCallback(typeof(NodeAction_{func_info.Name}))]\n");
                    }
                    sw.Write($"\t\tprivate static {func_info.GetDeclareStr()} _NodeAction_{func_info.Name}_{node.SelfMeta}({str_pararm_declare_list})");
                    sw.Write("{");
                    if (func_info.RetType != null)
                        sw.Write("return ");

                    sw.Write($"(({node.SelfType.FullName}*)p)->{func_info.Name}({str_param_call_list});");
                    sw.WriteLine("}");
                }
            }
            sw.WriteLine("\t\t#endregion\n");
        }

        private static void _Write_NodeTypeMap(StreamWriter sw, List<EdBTNodeInfo> list)
        {
            sw.WriteLine(@"
        public struct NodeInfo
        {
            public EBTNode Meta;
            public EBTNode ParentMeta;
            public Type SelfType;
            public int SelfSize;
            public NodeInfo(EBTNode meta, EBTNode parent_meta, Type self_type, int self_size)
            {
                this.Meta = meta;
                this.ParentMeta = parent_meta;
                this.SelfType = self_type;
                this.SelfSize = self_size;
            }
        }
");

            sw.WriteLine("\t\tprivate static readonly Dictionary<Type, NodeInfo> _BDNodeInfoDict = new(){");

            foreach (var p in list)
            {
                foreach (var p2 in p.BDTypes)
                {
                    if (p2.IsAbstract)
                        continue;
                    sw.Write("\t\t\t{");
                    sw.Write($"typeof({p2.FullName})");
                    sw.Write(",");
                    sw.Write($"new (EBTNode.{p.SelfMeta},EBTNode.{p.ParentMeta},typeof({p.SelfType.FullName}),sizeof({p.SelfType.FullName}))");
                    sw.WriteLine("},");
                }
            }
            sw.WriteLine("\t\t};\n");



            sw.WriteLine("\t\tprivate static readonly Dictionary<EBTNode, NodeInfo> _BTNodeInfoDict = new(){");
            foreach (var p in list)
            {
                sw.Write("\t\t\t{");
                sw.Write($"EBTNode.{p.SelfMeta}");
                sw.Write(",");
                sw.Write($"new (EBTNode.{p.SelfMeta},EBTNode.{p.ParentMeta},typeof({p.SelfType.FullName}),sizeof({p.SelfType.FullName}))");
                sw.WriteLine("},");
            }
            sw.WriteLine("\t\t};\n");
        }

        private static void _Write_VarTypeMap(StreamWriter sw, List<EdBTVarInfo> list)
        {

            sw.WriteLine(@"
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
");
            sw.WriteLine("\t\tprivate static readonly Dictionary<Type, VarInfo> _BDVarInfoDict = new(){");
            foreach (var p in list)
            {
                sw.Write("\t\t\t{");
                sw.Write($"typeof({p.BD_Type.FullName})");
                sw.Write(",");
                sw.Write($"VarInfo.Create<{p.Type.FullName},{p.ValueType.FullName},{p.BD_Type.FullName},{p.BD_ValueType.FullName}>(EBTVar.{p.Meta})");
                sw.WriteLine("},");
            }
            sw.WriteLine("\t\t};\n");

            sw.WriteLine("\t\tprivate static readonly Dictionary<EBTVar, VarInfo> _BTVarInfoDict = new(){");
            foreach (var p in list)
            {
                sw.Write("\t\t\t{");
                sw.Write($"EBTVar.{p.Meta}");
                sw.Write(",");
                sw.Write($"VarInfo.Create<{p.Type.FullName},{p.ValueType.FullName},{p.BD_Type.FullName},{p.BD_ValueType.FullName}>(EBTVar.{p.Meta})");
                sw.WriteLine("},");
            }
            sw.WriteLine("\t\t};\n");
        }

        private static void WriteBakeVarFunc(StreamWriter sw, List<EdBTVarInfo> list)
        {
            sw.Write(@"        public static BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            EBTVar meta = GetMeta(var);
            switch (meta)
            {
");
            foreach (var item in list)
            {
                sw.WriteLine($"\t\t\t\tcase EBTVar.{item.Meta}: return new {item.Type.FullName}().BakeVar(context, var);");
            }

            sw.Write(@"                default:
                    throw new Exception($""未知类型 {meta}"");
            }
        }
");
        }

        private static List<EdBTFuncInfo> _EdGetFuncList<T>()
        {
            List<EdBTFuncInfo> ret = new List<EdBTFuncInfo>();
            Type t = typeof(T);
            var all_methods = t.GetMethods();

            foreach (System.Reflection.MethodInfo p in all_methods)
            {
                EdBTFuncInfo info = new EdBTFuncInfo();
                ret.Add(info);
                info.Name = p.Name;
                info.RetType = p.ReturnType;
                if (info.RetType.FullName == "System.Void")
                    info.RetType = null;

                foreach (var p2 in p.GetParameters())
                {
                    info.ParamList.Add(EdBTFuncParamInfo.Create(p2));
                }
            }
            return ret;
        }

        public static List<EdBTNodeInfo> _EDCollectNodeInfos()
        {
            List<EdBTNodeInfo> ret = new List<EdBTNodeInfo>();

            Type node_type = typeof(IBTNode<>);

            UnityEditor.TypeCache.TypeCollection colletions = UnityEditor.TypeCache.GetTypesDerivedFrom(node_type);
            foreach (Type p in colletions)
            {
                if (!p.IsValueType)
                    continue;

                var attr_objs = p.GetCustomAttributes(typeof(BTNodeMetaAttribute), false);
                if (attr_objs == null || attr_objs.Length == 0)
                    continue;

                BTNodeMetaAttribute attr = attr_objs[0] as BTNodeMetaAttribute;
                if (attr == null)
                    continue;

                EdBTNodeInfo info = new EdBTNodeInfo();
                info.SelfMeta = attr.SelfMeta;
                info.ParentMeta = attr.ParentMeta;
                info.SelfType = p;

                foreach (var interface_type in p.GetInterfaces())
                {
                    if (interface_type.IsGenericType && interface_type.GetGenericTypeDefinition() == node_type)
                    {
                        info.BDTypes.Add(interface_type.GetGenericArguments()[0]);
                    }
                }
                ret.Add(info);
            }

            //检查
            {
                HashSet<EBTNode> meta_dict = new HashSet<EBTNode>();
                HashSet<Type> bd_type_dict = new HashSet<Type>();
                foreach (var p in ret)
                {
                    if (!meta_dict.Add(p.SelfMeta))
                    {
                        throw new Exception($"{p.SelfMeta.GetType()}.{p.SelfMeta} 重复出现多次");
                    }

                    foreach (var p2 in p.BDTypes)
                    {
                        if (!bd_type_dict.Add(p2))
                            throw new Exception($"{p2} 重复出现多次");
                    }
                }
            }
            return ret;
        }

        public static List<EdBTVarInfo> _EdCollectVarInfos()
        {
            List<EdBTVarInfo> ret = new List<EdBTVarInfo>();

            Type var_type = typeof(IBTVar<,,>);

            UnityEditor.TypeCache.TypeCollection colletions = UnityEditor.TypeCache.GetTypesDerivedFrom(var_type);
            foreach (Type p in colletions)
            {
                if (!p.IsValueType)
                    continue;

                var attr_objs = p.GetCustomAttributes(typeof(BTVarMetaAttribute), false);
                if (attr_objs == null || attr_objs.Length == 0)
                    continue;

                BTVarMetaAttribute attr = attr_objs[0] as BTVarMetaAttribute;
                if (attr == null)
                    continue;

                EdBTVarInfo info = new EdBTVarInfo();
                info.Meta = attr.Meta;
                info.Type = p;

                foreach (var interface_type in p.GetInterfaces())
                {
                    if (interface_type.IsGenericType && interface_type.GetGenericTypeDefinition() == var_type)
                    {
                        var args = interface_type.GetGenericArguments();
                        info.ValueType = args[0];
                        info.BD_ValueType = args[1];
                        info.BD_Type = args[2];
                    }
                }
                ret.Add(info);
            }


            //检查
            {
                HashSet<EBTVar> meta_dict = new HashSet<EBTVar>();
                HashSet<Type> bd_type_dict = new HashSet<Type>();
                foreach (var p in ret)
                {
                    if (!meta_dict.Add(p.Meta))
                    {
                        throw new Exception($"{p.Meta.GetType()}.{p.Meta} 重复出现多次");
                    }

                    if (!bd_type_dict.Add(p.BD_Type))
                        throw new Exception($"{p.BD_Type} 重复出现多次");
                }
            }
            return ret;
        }
    }
}
#endif