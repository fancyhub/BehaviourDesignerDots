
using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;

namespace DotsBT
{
    public interface IBTVar
    {
        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var);
    }

    public interface IBTVar<TValue, TBDValue, TBDType> : IBTVar
        where TValue : unmanaged
        where TBDType : SharedVariable<TBDValue>
    {
        public TValue GetValue(ref BTVM vm);
        public bool SetValue(ref BTVM vm, TValue value);
    }

    public interface IBTVar<TValue, TBDType> : IBTVar<TValue, TValue, TBDType>
        where TValue : unmanaged
        where TBDType : SharedVariable<TValue>
    {
    }

    public enum EBTVarScope
    {
        Local,
        Share,
        Global,
    }

    public enum EBTVarLinkStatus
    {
        None,   //不需要
        Need,   //需要Link,但是还没有做
        Linked, //需要Link, 已经link了
    }

    public struct BTVarBase
    {
        public short _meta;
        public byte _scope; //EBTVarScope 
        public byte _link_status; //EBTVarLink
        public int NameId;
        public BTPtr LinkedPtr; //变量的Ptr, Shared 或者 Global        

        public void Ctor(SharedVariable shared_var, BTBakeContextNode context)
        {
            if (shared_var.IsDynamic)
                throw new Exception("不支持 Dynamic 变量 " + shared_var.Name);
            // if (shared_var.IsGlobal)
            //     throw new Exception("现在不支持 Global 变量 " + shared_var.Name);

            LinkedPtr = BTPtr.Null;
            NameId = 0;
            LinkStatus = EBTVarLinkStatus.None;
            Scope = EBTVarScope.Local;
            Meta = BTVarVT.GetMeta(shared_var);

            if (shared_var.IsShared)
            {
                if (shared_var.IsGlobal)
                {
                    Scope = EBTVarScope.Global;
                }
                else
                {
                    Scope = EBTVarScope.Share;
                }

                LinkStatus = EBTVarLinkStatus.Need;
                NameId = BTUtil.Name2Id(shared_var.Name);
            }


            context.UseVar(shared_var, Scope, ref LinkedPtr);
        }

        public object GetBoxedValue(ref BTVM vm)
        {
            BTPtr ptr = Link(ref vm);
            return BTVarVT.GetBoxedValue(ref vm.Memory, ptr, Meta);
        }

        public EBTVarScope Scope { get => (EBTVarScope)_scope; set => _scope = (byte)value; }
        public EBTVarLinkStatus LinkStatus { get => (EBTVarLinkStatus)_link_status; set => _link_status = (byte)value; }
        public EBTVar Meta { get => (EBTVar)_meta; set => _meta = (short)value; }

        public BTPtr Link(ref BTVM vm)
        {
            switch (LinkStatus)
            {
                default:
                case EBTVarLinkStatus.None:
                    return BTPtr.Null;

                case EBTVarLinkStatus.Linked:
                    return LinkedPtr;

                case EBTVarLinkStatus.Need:
                    LinkStatus = EBTVarLinkStatus.Linked;
                    var newptr = vm.Linkvar(NameId, Scope);
                    UnityEngine.Debug.Assert(newptr == LinkedPtr, $"link的结果不对, {newptr},{LinkedPtr}");
                    LinkedPtr = newptr;
                    return LinkedPtr;
            }
        }
    }

    public struct BTVarBase<TData> where TData : unmanaged
    {
        public BTVarBase Base;
        public TData Value;

        public void Ctor(SharedVariable shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var, context);
        }

        public EBTVarScope Scope { get => Base.Scope; set => Base.Scope = value; }

        public unsafe TData GetValue(ref BTVM vm)
        {
            BTPtr shared_ptr = Base.Link(ref vm);

            if (shared_ptr.IsNull)
                return Value;

            TData* p = vm.Get<TData>(shared_ptr, false);
            if (p == null)
                return default;
            return *p;
        }

        public unsafe bool SetValue(ref BTVM vm, TData value)
        {
            switch (Scope)
            {
                default:
                    UnityEngine.Debug.LogError($"未处理的 Scope， 报错");
                    return false;

                case EBTVarScope.Global:
                    UnityEngine.Debug.LogError($"全局变量不允许写入");
                    return false;
                case EBTVarScope.Local:
                    UnityEngine.Debug.LogError($"写入本地变量是无意义的， 报错");
                    return false;

                case EBTVarScope.Share:
                    BTPtr shared_ptr = Base.Link(ref vm);
                    if (shared_ptr.IsNull)
                    {
                        UnityEngine.Debug.LogError($"Shared Ptr Is Null");
                        return false;
                    }
                    TData* p = vm.Get<TData>(shared_ptr, true);
                    if (p == null)
                        return false;
                    *p = value;
                    return true;
            }
        }

        public BTPtr Link(ref BTVM vm)
        {
            return Base.Link(ref vm);
        }
    }
}
