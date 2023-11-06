using System;
using System.Collections.Generic;
using System.Diagnostics;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.Log, EBTNode.Action)]
    public struct BTNodeLog : IBTNode<Log>
    {
        public BTNodeAction Base;
        public BTString _text;
        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            Log spec_task = task as Log;
            _text.Ctor(ref context.Allocator, spec_task.text.Value);
        }

        public void Stop(ref BTVM vm)
        {

        }

        public EBTStatus Update(ref BTVM vm)
        {
            string v = _text.GetStr(ref vm);
            UnityEngine.Debug.Log(v);
            return EBTStatus.Success;
        }
    }

    [BTNodeMeta(EBTNode.LogFormat, EBTNode.Action)]
    public struct BTNodeLogFormat : IBTNode<LogFormat>
    {
        public BTNodeAction Base;
        public BTString textFormat;
        public BTVarBase _arg0;
        public bool _valid0;
        public BTVarBase _arg1;
        public bool _valid1;
        public BTVarBase _arg2;
        public bool _valid2;
        public BTVarBase _arg3;
        public bool _valid3;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            LogFormat spec_task = task as LogFormat;

            textFormat.Ctor(ref context.Allocator, spec_task.textFormat.Value);

            _InitArg(ref _arg0, ref _valid0, spec_task.arg0, context);
            _InitArg(ref _arg1, ref _valid1, spec_task.arg1, context);
            _InitArg(ref _arg2, ref _valid2, spec_task.arg2, context);
            _InitArg(ref _arg3, ref _valid3, spec_task.arg3, context);
        }

        private static void _InitArg(ref BTVarBase self_arg, ref bool self_arg_valid, SharedVariable arg, BTBakeContextNode context)
        {
            if (arg == null)
            {
                self_arg_valid = false;
                return;
            }

            self_arg_valid = true;
            self_arg.Ctor(arg, context);
            if (self_arg.Scope == EBTVarScope.Local)
                throw new Exception("只支持Shared");
        }

        public void Stop(ref BTVM vm)
        {

        }

        public EBTStatus Update(ref BTVM vm)
        {
            string v = textFormat.GetStr(ref vm);

            if (_valid3)
            {
                object a0 = _arg0.GetBoxedValue(ref vm);
                object a1 = _arg0.GetBoxedValue(ref vm);
                object a2 = _arg0.GetBoxedValue(ref vm);
                object a3 = _arg0.GetBoxedValue(ref vm);
                UnityEngine.Debug.Log(string.Format(v, a0, a1, a2, a3));
            }
            else if (_valid2)
            {
                object a0 = _arg0.GetBoxedValue(ref vm);
                object a1 = _arg0.GetBoxedValue(ref vm);
                object a2 = _arg0.GetBoxedValue(ref vm);
                UnityEngine.Debug.Log(string.Format(v, a0, a1, a2));
            }
            else if (_valid1)
            {
                object a0 = _arg0.GetBoxedValue(ref vm);
                object a1 = _arg0.GetBoxedValue(ref vm);
                UnityEngine.Debug.Log(string.Format(v, a0, a1));
            }
            else if (_valid0)
            {
                object a0 = _arg0.GetBoxedValue(ref vm);
                UnityEngine.Debug.Log(string.Format(v, a0));
            }
            else
                UnityEngine.Debug.Log(v);
            return EBTStatus.Success;
        }
    }
}
