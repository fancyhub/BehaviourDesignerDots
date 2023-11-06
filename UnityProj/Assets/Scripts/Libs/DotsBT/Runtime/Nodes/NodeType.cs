using System;
using System.Collections.Generic;

namespace DotsBT
{
    public enum EBTNode : ushort
    {
        None,
        NodeBase,

        //Composite 开始
        Composite = 100,
        Sequence,
        Parallel,
        Selector,
        ParallelSelector,
        ParallelComplete,
        RandomSelector,
        RandomSequence,

        //Decorator
        Decorator = 200,
        Repeater,
        Inverter,
        ReturnSuccess,
        ReturnFailure,
        UntilSuccess,
        UntilFailure,
        CustomSubTree,
        BehaviorTreeReference,
        CaseIntComparison,
        TimeRepeater,
        CaseBoolComparison,


        //Action
        Action = 300,
        Rotate,
        SetPosition,
        CompareSharedBool,
        Wait,
        UnityMathSetInt,
        Log,
        LogFormat,
        MonsterToPoint,
        TimerStart,
        TimerInterrupt,
        TimerReset,
        TimerCheck,

        //Conditional
        Conditional = 400,
        RandomProbability,
        UnityMathIntComparison,
        CustomIntComparison,
        CustomFloatComparison,
        UnityMathBoolComparison,



        //Common
        CheckTargetAlive = 500,
        SpawnerEnable,

        //Monster
        MonsterChaseTarget = 600,
        MonsterCheckTargetDistance,
        MonsterPatrolBegin,
        MonsterPatrolMoveTo,
        MonsterPatrolNext,
        MonsterCastAbility,
        MonsterFaceToTarget,
        MonsterTarget_SearchStart,
        MonsterTarget_SearchStop,
        MonsterTarget_Change,
        MonsterTarget_IsSenseListEmpty,
        MonsterTarget_SenseCompare,
        MonsterTarget_IsListEmpty,
        MonsterSendInputButton,
        MonsterHasBoxActiveTag,
        MonsterStopMove,
        MonsterSetBendTarget,

        //Bot
        BotChaseTarget = 1000,
        BotOpenDoor = 1001,
        BotStopMove = 1002,
        BotToPoint = 1003,

        //Test
        TestDecorator = 10000,
        TestReturnAction,
    }


    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class BTNodeMetaAttribute : Attribute
    {
        public EBTNode SelfMeta;
        public EBTNode ParentMeta;

        public BTNodeMetaAttribute(EBTNode type, EBTNode parentType = EBTNode.None)
        {
            SelfMeta = type;
            ParentMeta = parentType;
        }
    }
}
