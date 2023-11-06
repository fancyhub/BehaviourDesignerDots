using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    public class TimeRepeater : Decorator
    {
        public SharedFloat time;

        [Tooltip("子节点如果返回failure, 直接返回failure")]
        public bool endOnFailure;

        [Tooltip("如果时间到了, 直接stop子节点, 如果false,等子节点结束,再结束自己")]
        public bool stopOnTimeout;
    }

    [BTNodeMeta(EBTNode.TimeRepeater, EBTNode.Decorator)]
    public unsafe struct BTNodeTimeRepeater : IBTNode<TimeRepeater>
    {
        public BTNodeDecorator Base;
        public BTVarFloat _time;
        public bool _endOnFailure;
        public bool _stopOnTimeout;
        public bool _running;
        public double _expire_time;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            TimeRepeater spec_task = task as TimeRepeater;
            _time.Ctor(spec_task.time, context);            
            _endOnFailure = spec_task.endOnFailure;
            _stopOnTimeout = spec_task.stopOnTimeout;
            _running = false;
            _expire_time = 0;
        }

        public EBTStatus Update(ref BTVM vm)
        {
            //第一次进入
            if (!_running)
            {
                _expire_time = vm.Time.ElapsedTime + _time.GetValue(ref vm);
                _running = true;
            }

            //先执行
            EBTStatus child_status = Base.UpdateDecoratedNode(ref vm);
            if (child_status == EBTStatus.Failure)
            {
                _running = false;
                return EBTStatus.Failure;
            }

            //检查是否结束
            if (vm.Time.ElapsedTime < _expire_time)
                return EBTStatus.Running;

            //如果子节点正在运行
            if (child_status == EBTStatus.Running)
            {
                if (!_stopOnTimeout)
                    return EBTStatus.Running;

                _running = false;
                Base.Stop(ref vm);
                return EBTStatus.Success;
            }

            _running = false;
            return EBTStatus.Success;
        }

        public void Stop(ref BTVM vm)
        {
            if (!_running)
                return;
            _running = false;
            Base.Stop(ref vm);
        }
    }
}
