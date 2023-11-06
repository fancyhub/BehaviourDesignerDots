using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [Serializable]
    public struct TimerData
    {
        public enum EStatus
        {
            None,
            Running,
            Interrupt,
        }

        public double StartTime;
        public float Duration;
        public EStatus Status;

        public void Start(double elapsed_time, float duration)
        {
            StartTime = elapsed_time;
            Duration = duration;
            Status = EStatus.Running;
        }

        public void Reset()
        {
            StartTime = 0;
            Duration = 0;
            Status = EStatus.None;
        }

        public void Interrupt(double elapsed_time)
        {
            if (Status != EStatus.Running)
                return;

            if (elapsed_time >= (StartTime + Duration))
                return;
            Status = EStatus.Interrupt;
        }
    }

    public class SharedTimerData : SharedVariable<TimerData>
    {
        public static implicit operator SharedTimerData(TimerData value) { return new SharedTimerData { mValue = value }; }
    }

    // 会把状态 设置为 Running
    [TaskCategory("Timer")]
    [TaskDescription("Action, 开始")]
    public class TimerStart : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedTimerData timer;
        public SharedFloat duration;
    }

    [TaskCategory("Timer")]
    [TaskDescription("Action, 打断")]
    public class TimerInterrupt : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedTimerData timer;
    }


    [TaskCategory("Timer")]
    [TaskDescription("Action, 重置")]
    public class TimerReset : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedTimerData timer;
    }


    [TaskCategory("Timer")]
    [TaskDescription("Condition, 检查Timer 是否已经到时间了")]
    public class TimerCheck : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedTimerData timer;
        public bool SuccOnInterrupt;
        public bool ResetOnSucc;
    }


    /// ////////////////////////////////////////////////////////////////

    [BTVarMeta(EBTVar.VarTimerData)]
    public struct BTVarTimerData : IBTVar<TimerData, SharedTimerData>
    {
        public BTVarBase<TimerData> Base;
        public void Ctor(SharedTimerData shared_var, BTBakeContextNode context)
        {
            Base.Ctor(shared_var, context);
            Base.Value = shared_var.Value;
        }

        public BTPtr BakeVar(BTBakeContextVar context, SharedVariable var)
        {
            return context.Allocator.AllocAndSet((var as SharedTimerData).Value);
        }

        public TimerData GetValue(ref BTVM vm)
        {
            return Base.GetValue(ref vm);
        }

        public bool SetValue(ref BTVM vm, TimerData value) => Base.SetValue(ref vm, value);
    }

    [BTNodeMeta(EBTNode.TimerStart, EBTNode.Action)]
    public struct BTNodeTimerStart : IBTNode<TimerStart>
    {
        public BTNodeAction Base;
        public BTVarTimerData _timer;
        public BTVarFloat _duration;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            TimerStart spec_task = task as TimerStart;
            _timer.Ctor(spec_task.timer, context);
            _duration.Ctor(spec_task.duration, context);
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            var timer = _timer.GetValue(ref vm);
            timer.Start(vm.Time.ElapsedTime, _duration.GetValue(ref vm));
            _timer.SetValue(ref vm, timer);
            return EBTStatus.Success;
        }
    }

    [BTNodeMeta(EBTNode.TimerReset, EBTNode.Action)]
    public struct BTNodeTimerReset : IBTNode<TimerReset>
    {
        public BTNodeAction Base;
        public BTVarTimerData _timer;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            TimerReset spec_task = task as TimerReset;
            _timer.Ctor(spec_task.timer, context);
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            var timer = _timer.GetValue(ref vm);
            timer.Reset();
            _timer.SetValue(ref vm, timer);
            return EBTStatus.Success;
        }
    }


    [BTNodeMeta(EBTNode.TimerInterrupt, EBTNode.Action)]
    public struct BTNodeTimerInterrupt : IBTNode<TimerInterrupt>
    {
        public BTNodeAction Base;
        public BTVarTimerData _timer;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            TimerInterrupt spec_task = task as TimerInterrupt;
            _timer.Ctor(spec_task.timer, context);
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            var timer = _timer.GetValue(ref vm);
            timer.Interrupt(vm.Time.ElapsedTime);
            _timer.SetValue(ref vm, timer);
            return EBTStatus.Success;
        }
    }

    [BTNodeMeta(EBTNode.TimerCheck, EBTNode.Action)]
    public struct BTNodeTimerCheck : IBTNode<TimerCheck>
    {
        public BTNodeAction Base;
        public BTVarTimerData _timer;
        public bool _SuccOnInterrupt;
        public bool _ResetOnSucc;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            TimerCheck spec_task = task as TimerCheck;
            _timer.Ctor(spec_task.timer, context);
            _SuccOnInterrupt = spec_task.SuccOnInterrupt;
            _ResetOnSucc = spec_task.ResetOnSucc;
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            var timer = _timer.GetValue(ref vm);
            if (!_IsSucc(in timer, vm.Time.ElapsedTime, _SuccOnInterrupt))
                return EBTStatus.Failure;

            if (_ResetOnSucc)
            {
                timer.Reset();
                _timer.SetValue(ref vm, timer);
            }
            return EBTStatus.Success;
        }

        public static bool _IsSucc(in TimerData data, double time_now, bool SuccOnInterrupt)
        {
            switch (data.Status)
            {
                default:
                    return false;
                case TimerData.EStatus.None:
                    return false;

                case TimerData.EStatus.Interrupt:
                    if (SuccOnInterrupt)
                        return true;
                    return (data.Duration + data.StartTime) <= time_now;
                case TimerData.EStatus.Running:
                    return (data.Duration + data.StartTime) <= time_now;
            }
        }
    }
}
