using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.Wait, EBTNode.Action)]
    public struct BTNodeWait : IBTNode<Wait>
    {
        public BTNodeBase Base;
        public BTVarFloat waitTime;
        public BTVarBool randomWait;
        public BTVarFloat randomWaitMin;
        public BTVarFloat randomWaitMax;

        private float _time; //倒计时
        private bool _running;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            var spc = task as Wait;
            waitTime.Ctor(spc.waitTime, context);
            randomWait.Ctor(spc.randomWait, context);
            randomWaitMin.Ctor(spc.randomWaitMin, context);
            randomWaitMax.Ctor(spc.randomWaitMax, context);
        }


        public EBTStatus Update(ref BTVM vm)
        {
            if (!_running)
            {
                _running = true;

                if (randomWait.GetValue(ref vm))
                    _time = Unity.Mathematics.math.lerp(randomWaitMin.GetValue(ref vm), randomWaitMax.GetValue(ref vm), MyMath.RandomFloat());
                else
                    _time = waitTime.GetValue(ref vm);
            }

            _time -= vm.Time.DeltaTime;
            if (_time > 0)
                return EBTStatus.Running;

            _running = false;
            _time = 0;
            return EBTStatus.Success;
        }


        public void Stop(ref BTVM vm)
        {
            if (!_running)
                return;
            _running = true;
            _time = 0;
        }
    }
}