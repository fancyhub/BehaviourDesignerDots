//Auto Gen
using System;
using System.Collections.Generic;
using AOT;
using Unity.Burst;
namespace DotsBT
{
    //[BurstCompile]
    public unsafe static partial class BTNodeVT
    {

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

		private static readonly Dictionary<Type, NodeInfo> _BDNodeInfoDict = new(){
			{typeof(DotsBT.TimerInterrupt),new (EBTNode.TimerInterrupt,EBTNode.Action,typeof(DotsBT.BTNodeTimerInterrupt),sizeof(DotsBT.BTNodeTimerInterrupt))},
			{typeof(BehaviorDesigner.Runtime.Tasks.ParallelSelector),new (EBTNode.ParallelSelector,EBTNode.Composite,typeof(DotsBT.BTNodeParallelSelector),sizeof(DotsBT.BTNodeParallelSelector))},
			{typeof(BehaviorDesigner.Runtime.Tasks.UntilFailure),new (EBTNode.UntilFailure,EBTNode.Decorator,typeof(DotsBT.BTNodeUntilFailure),sizeof(DotsBT.BTNodeUntilFailure))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Parallel),new (EBTNode.Parallel,EBTNode.Composite,typeof(DotsBT.BTNodeParallel),sizeof(DotsBT.BTNodeParallel))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Repeater),new (EBTNode.Repeater,EBTNode.Decorator,typeof(DotsBT.BTNodeRepeater),sizeof(DotsBT.BTNodeRepeater))},
			{typeof(BehaviorDesigner.Runtime.Tasks.RandomSelector),new (EBTNode.RandomSelector,EBTNode.Composite,typeof(DotsBT.BTNodeRandomSelector),sizeof(DotsBT.BTNodeRandomSelector))},
			{typeof(DotsBT.CaseIntComparison),new (EBTNode.CaseIntComparison,EBTNode.Decorator,typeof(DotsBT.BTNodeCaseIntComparison),sizeof(DotsBT.BTNodeCaseIntComparison))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Inverter),new (EBTNode.Inverter,EBTNode.Decorator,typeof(DotsBT.BTNodeInverter),sizeof(DotsBT.BTNodeInverter))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Unity.Math.BoolComparison),new (EBTNode.UnityMathBoolComparison,EBTNode.Conditional,typeof(DotsBT.BTNodeUnityMathBoolComparison),sizeof(DotsBT.BTNodeUnityMathBoolComparison))},
			{typeof(BehaviorDesigner.Runtime.Tasks.RandomSequence),new (EBTNode.RandomSequence,EBTNode.Composite,typeof(DotsBT.BTNodeRandomSequence),sizeof(DotsBT.BTNodeRandomSequence))},
			{typeof(DotsBT.TestReturnAction),new (EBTNode.TestReturnAction,EBTNode.Action,typeof(DotsBT.BTNodeTestReturnAction),sizeof(DotsBT.BTNodeTestReturnAction))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform.Rotate),new (EBTNode.Rotate,EBTNode.NodeBase,typeof(DotsBT.UnityTransform.BTNodeRotate),sizeof(DotsBT.UnityTransform.BTNodeRotate))},
			{typeof(DotsBT.TimeRepeater),new (EBTNode.TimeRepeater,EBTNode.Decorator,typeof(DotsBT.BTNodeTimeRepeater),sizeof(DotsBT.BTNodeTimeRepeater))},
			{typeof(DotsBT.TimerCheck),new (EBTNode.TimerCheck,EBTNode.Action,typeof(DotsBT.BTNodeTimerCheck),sizeof(DotsBT.BTNodeTimerCheck))},
			{typeof(DotsBT.TimerReset),new (EBTNode.TimerReset,EBTNode.Action,typeof(DotsBT.BTNodeTimerReset),sizeof(DotsBT.BTNodeTimerReset))},
			{typeof(BehaviorDesigner.Runtime.Tasks.RandomProbability),new (EBTNode.RandomProbability,EBTNode.None,typeof(DotsBT.BTNodeRandomProbability),sizeof(DotsBT.BTNodeRandomProbability))},
			{typeof(BehaviorDesigner.Runtime.Tasks.UntilSuccess),new (EBTNode.UntilSuccess,EBTNode.Decorator,typeof(DotsBT.BTNodeUntilSuccess),sizeof(DotsBT.BTNodeUntilSuccess))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug.LogFormat),new (EBTNode.LogFormat,EBTNode.Action,typeof(DotsBT.BTNodeLogFormat),sizeof(DotsBT.BTNodeLogFormat))},
			{typeof(DotsBT.TestDecorator),new (EBTNode.TestDecorator,EBTNode.Decorator,typeof(DotsBT.BTNodeTestDecorator),sizeof(DotsBT.BTNodeTestDecorator))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Wait),new (EBTNode.Wait,EBTNode.Action,typeof(DotsBT.BTNodeWait),sizeof(DotsBT.BTNodeWait))},
			{typeof(DotsBT.TimerStart),new (EBTNode.TimerStart,EBTNode.Action,typeof(DotsBT.BTNodeTimerStart),sizeof(DotsBT.BTNodeTimerStart))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Selector),new (EBTNode.Selector,EBTNode.Composite,typeof(DotsBT.BTNodeSelector),sizeof(DotsBT.BTNodeSelector))},
			{typeof(BehaviorDesigner.Runtime.Tasks.ReturnFailure),new (EBTNode.ReturnFailure,EBTNode.Decorator,typeof(DotsBT.BTNodeReturnFailure),sizeof(DotsBT.BTNodeReturnFailure))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody.SetPosition),new (EBTNode.SetPosition,EBTNode.NodeBase,typeof(DotsBT.UnityTransform.BTNodeSetPosition),sizeof(DotsBT.UnityTransform.BTNodeSetPosition))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Unity.Math.IntComparison),new (EBTNode.UnityMathIntComparison,EBTNode.Conditional,typeof(DotsBT.BTNodeUnityMathIntComparison),sizeof(DotsBT.BTNodeUnityMathIntComparison))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Log),new (EBTNode.Log,EBTNode.Action,typeof(DotsBT.BTNodeLog),sizeof(DotsBT.BTNodeLog))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.CompareSharedBool),new (EBTNode.CompareSharedBool,EBTNode.NodeBase,typeof(DotsBT.SharedVariables.BTNodeCompareSharedBool),sizeof(DotsBT.SharedVariables.BTNodeCompareSharedBool))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Sequence),new (EBTNode.Sequence,EBTNode.Composite,typeof(DotsBT.BTNodeSequence),sizeof(DotsBT.BTNodeSequence))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Decorator),new (EBTNode.Decorator,EBTNode.NodeBase,typeof(DotsBT.BTNodeDecorator),sizeof(DotsBT.BTNodeDecorator))},
			{typeof(DotsBT.CaseBoolComparison),new (EBTNode.CaseBoolComparison,EBTNode.Decorator,typeof(DotsBT.BTNodeCaseBoolComparison),sizeof(DotsBT.BTNodeCaseBoolComparison))},
			{typeof(BehaviorDesigner.Runtime.Tasks.BehaviorTreeReference),new (EBTNode.BehaviorTreeReference,EBTNode.Decorator,typeof(DotsBT.SubTree.BTNodeBehaviorTreeReference),sizeof(DotsBT.SubTree.BTNodeBehaviorTreeReference))},
			{typeof(BehaviorDesigner.Runtime.Tasks.Unity.Math.SetInt),new (EBTNode.UnityMathSetInt,EBTNode.Action,typeof(DotsBT.BTNodeUnityMathSetInt),sizeof(DotsBT.BTNodeUnityMathSetInt))},
			{typeof(BehaviorDesigner.Runtime.Tasks.ParallelComplete),new (EBTNode.ParallelComplete,EBTNode.Composite,typeof(DotsBT.BTNodeParallelComplete),sizeof(DotsBT.BTNodeParallelComplete))},
			{typeof(BehaviorDesigner.Runtime.Tasks.ReturnSuccess),new (EBTNode.ReturnSuccess,EBTNode.Decorator,typeof(DotsBT.BTNodeReturnSuccess),sizeof(DotsBT.BTNodeReturnSuccess))},
		};

		private static readonly Dictionary<EBTNode, NodeInfo> _BTNodeInfoDict = new(){
			{EBTNode.TimerInterrupt,new (EBTNode.TimerInterrupt,EBTNode.Action,typeof(DotsBT.BTNodeTimerInterrupt),sizeof(DotsBT.BTNodeTimerInterrupt))},
			{EBTNode.ParallelSelector,new (EBTNode.ParallelSelector,EBTNode.Composite,typeof(DotsBT.BTNodeParallelSelector),sizeof(DotsBT.BTNodeParallelSelector))},
			{EBTNode.UntilFailure,new (EBTNode.UntilFailure,EBTNode.Decorator,typeof(DotsBT.BTNodeUntilFailure),sizeof(DotsBT.BTNodeUntilFailure))},
			{EBTNode.Parallel,new (EBTNode.Parallel,EBTNode.Composite,typeof(DotsBT.BTNodeParallel),sizeof(DotsBT.BTNodeParallel))},
			{EBTNode.Repeater,new (EBTNode.Repeater,EBTNode.Decorator,typeof(DotsBT.BTNodeRepeater),sizeof(DotsBT.BTNodeRepeater))},
			{EBTNode.RandomSelector,new (EBTNode.RandomSelector,EBTNode.Composite,typeof(DotsBT.BTNodeRandomSelector),sizeof(DotsBT.BTNodeRandomSelector))},
			{EBTNode.CaseIntComparison,new (EBTNode.CaseIntComparison,EBTNode.Decorator,typeof(DotsBT.BTNodeCaseIntComparison),sizeof(DotsBT.BTNodeCaseIntComparison))},
			{EBTNode.Inverter,new (EBTNode.Inverter,EBTNode.Decorator,typeof(DotsBT.BTNodeInverter),sizeof(DotsBT.BTNodeInverter))},
			{EBTNode.Conditional,new (EBTNode.Conditional,EBTNode.NodeBase,typeof(DotsBT.BTNodeConditional),sizeof(DotsBT.BTNodeConditional))},
			{EBTNode.UnityMathBoolComparison,new (EBTNode.UnityMathBoolComparison,EBTNode.Conditional,typeof(DotsBT.BTNodeUnityMathBoolComparison),sizeof(DotsBT.BTNodeUnityMathBoolComparison))},
			{EBTNode.RandomSequence,new (EBTNode.RandomSequence,EBTNode.Composite,typeof(DotsBT.BTNodeRandomSequence),sizeof(DotsBT.BTNodeRandomSequence))},
			{EBTNode.TestReturnAction,new (EBTNode.TestReturnAction,EBTNode.Action,typeof(DotsBT.BTNodeTestReturnAction),sizeof(DotsBT.BTNodeTestReturnAction))},
			{EBTNode.Rotate,new (EBTNode.Rotate,EBTNode.NodeBase,typeof(DotsBT.UnityTransform.BTNodeRotate),sizeof(DotsBT.UnityTransform.BTNodeRotate))},
			{EBTNode.TimeRepeater,new (EBTNode.TimeRepeater,EBTNode.Decorator,typeof(DotsBT.BTNodeTimeRepeater),sizeof(DotsBT.BTNodeTimeRepeater))},
			{EBTNode.TimerCheck,new (EBTNode.TimerCheck,EBTNode.Action,typeof(DotsBT.BTNodeTimerCheck),sizeof(DotsBT.BTNodeTimerCheck))},
			{EBTNode.Composite,new (EBTNode.Composite,EBTNode.NodeBase,typeof(DotsBT.BTNodeComposite),sizeof(DotsBT.BTNodeComposite))},
			{EBTNode.TimerReset,new (EBTNode.TimerReset,EBTNode.Action,typeof(DotsBT.BTNodeTimerReset),sizeof(DotsBT.BTNodeTimerReset))},
			{EBTNode.RandomProbability,new (EBTNode.RandomProbability,EBTNode.None,typeof(DotsBT.BTNodeRandomProbability),sizeof(DotsBT.BTNodeRandomProbability))},
			{EBTNode.UntilSuccess,new (EBTNode.UntilSuccess,EBTNode.Decorator,typeof(DotsBT.BTNodeUntilSuccess),sizeof(DotsBT.BTNodeUntilSuccess))},
			{EBTNode.LogFormat,new (EBTNode.LogFormat,EBTNode.Action,typeof(DotsBT.BTNodeLogFormat),sizeof(DotsBT.BTNodeLogFormat))},
			{EBTNode.TestDecorator,new (EBTNode.TestDecorator,EBTNode.Decorator,typeof(DotsBT.BTNodeTestDecorator),sizeof(DotsBT.BTNodeTestDecorator))},
			{EBTNode.Wait,new (EBTNode.Wait,EBTNode.Action,typeof(DotsBT.BTNodeWait),sizeof(DotsBT.BTNodeWait))},
			{EBTNode.TimerStart,new (EBTNode.TimerStart,EBTNode.Action,typeof(DotsBT.BTNodeTimerStart),sizeof(DotsBT.BTNodeTimerStart))},
			{EBTNode.Selector,new (EBTNode.Selector,EBTNode.Composite,typeof(DotsBT.BTNodeSelector),sizeof(DotsBT.BTNodeSelector))},
			{EBTNode.ReturnFailure,new (EBTNode.ReturnFailure,EBTNode.Decorator,typeof(DotsBT.BTNodeReturnFailure),sizeof(DotsBT.BTNodeReturnFailure))},
			{EBTNode.SetPosition,new (EBTNode.SetPosition,EBTNode.NodeBase,typeof(DotsBT.UnityTransform.BTNodeSetPosition),sizeof(DotsBT.UnityTransform.BTNodeSetPosition))},
			{EBTNode.NodeBase,new (EBTNode.NodeBase,EBTNode.None,typeof(DotsBT.BTNodeBase),sizeof(DotsBT.BTNodeBase))},
			{EBTNode.UnityMathIntComparison,new (EBTNode.UnityMathIntComparison,EBTNode.Conditional,typeof(DotsBT.BTNodeUnityMathIntComparison),sizeof(DotsBT.BTNodeUnityMathIntComparison))},
			{EBTNode.Log,new (EBTNode.Log,EBTNode.Action,typeof(DotsBT.BTNodeLog),sizeof(DotsBT.BTNodeLog))},
			{EBTNode.CompareSharedBool,new (EBTNode.CompareSharedBool,EBTNode.NodeBase,typeof(DotsBT.SharedVariables.BTNodeCompareSharedBool),sizeof(DotsBT.SharedVariables.BTNodeCompareSharedBool))},
			{EBTNode.Action,new (EBTNode.Action,EBTNode.NodeBase,typeof(DotsBT.BTNodeAction),sizeof(DotsBT.BTNodeAction))},
			{EBTNode.Sequence,new (EBTNode.Sequence,EBTNode.Composite,typeof(DotsBT.BTNodeSequence),sizeof(DotsBT.BTNodeSequence))},
			{EBTNode.Decorator,new (EBTNode.Decorator,EBTNode.NodeBase,typeof(DotsBT.BTNodeDecorator),sizeof(DotsBT.BTNodeDecorator))},
			{EBTNode.CaseBoolComparison,new (EBTNode.CaseBoolComparison,EBTNode.Decorator,typeof(DotsBT.BTNodeCaseBoolComparison),sizeof(DotsBT.BTNodeCaseBoolComparison))},
			{EBTNode.BehaviorTreeReference,new (EBTNode.BehaviorTreeReference,EBTNode.Decorator,typeof(DotsBT.SubTree.BTNodeBehaviorTreeReference),sizeof(DotsBT.SubTree.BTNodeBehaviorTreeReference))},
			{EBTNode.UnityMathSetInt,new (EBTNode.UnityMathSetInt,EBTNode.Action,typeof(DotsBT.BTNodeUnityMathSetInt),sizeof(DotsBT.BTNodeUnityMathSetInt))},
			{EBTNode.ParallelComplete,new (EBTNode.ParallelComplete,EBTNode.Composite,typeof(DotsBT.BTNodeParallelComplete),sizeof(DotsBT.BTNodeParallelComplete))},
			{EBTNode.ReturnSuccess,new (EBTNode.ReturnSuccess,EBTNode.Decorator,typeof(DotsBT.BTNodeReturnSuccess),sizeof(DotsBT.BTNodeReturnSuccess))},
		};

		#region NodeAction_Ctor
		public delegate void NodeAction_Ctor(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context);
		private static readonly Dictionary<DotsBT.EBTNode, NodeAction_Ctor> _NodeAction_CtorDict = new()
		{
			{DotsBT.EBTNode.TimerInterrupt,_NodeAction_Ctor_TimerInterrupt},
			{DotsBT.EBTNode.ParallelSelector,_NodeAction_Ctor_ParallelSelector},
			{DotsBT.EBTNode.UntilFailure,_NodeAction_Ctor_UntilFailure},
			{DotsBT.EBTNode.Parallel,_NodeAction_Ctor_Parallel},
			{DotsBT.EBTNode.Repeater,_NodeAction_Ctor_Repeater},
			{DotsBT.EBTNode.RandomSelector,_NodeAction_Ctor_RandomSelector},
			{DotsBT.EBTNode.CaseIntComparison,_NodeAction_Ctor_CaseIntComparison},
			{DotsBT.EBTNode.Inverter,_NodeAction_Ctor_Inverter},
			{DotsBT.EBTNode.UnityMathBoolComparison,_NodeAction_Ctor_UnityMathBoolComparison},
			{DotsBT.EBTNode.RandomSequence,_NodeAction_Ctor_RandomSequence},
			{DotsBT.EBTNode.TestReturnAction,_NodeAction_Ctor_TestReturnAction},
			{DotsBT.EBTNode.Rotate,_NodeAction_Ctor_Rotate},
			{DotsBT.EBTNode.TimeRepeater,_NodeAction_Ctor_TimeRepeater},
			{DotsBT.EBTNode.TimerCheck,_NodeAction_Ctor_TimerCheck},
			{DotsBT.EBTNode.TimerReset,_NodeAction_Ctor_TimerReset},
			{DotsBT.EBTNode.RandomProbability,_NodeAction_Ctor_RandomProbability},
			{DotsBT.EBTNode.UntilSuccess,_NodeAction_Ctor_UntilSuccess},
			{DotsBT.EBTNode.LogFormat,_NodeAction_Ctor_LogFormat},
			{DotsBT.EBTNode.TestDecorator,_NodeAction_Ctor_TestDecorator},
			{DotsBT.EBTNode.Wait,_NodeAction_Ctor_Wait},
			{DotsBT.EBTNode.TimerStart,_NodeAction_Ctor_TimerStart},
			{DotsBT.EBTNode.Selector,_NodeAction_Ctor_Selector},
			{DotsBT.EBTNode.ReturnFailure,_NodeAction_Ctor_ReturnFailure},
			{DotsBT.EBTNode.SetPosition,_NodeAction_Ctor_SetPosition},
			{DotsBT.EBTNode.UnityMathIntComparison,_NodeAction_Ctor_UnityMathIntComparison},
			{DotsBT.EBTNode.Log,_NodeAction_Ctor_Log},
			{DotsBT.EBTNode.CompareSharedBool,_NodeAction_Ctor_CompareSharedBool},
			{DotsBT.EBTNode.Sequence,_NodeAction_Ctor_Sequence},
			{DotsBT.EBTNode.Decorator,_NodeAction_Ctor_Decorator},
			{DotsBT.EBTNode.CaseBoolComparison,_NodeAction_Ctor_CaseBoolComparison},
			{DotsBT.EBTNode.BehaviorTreeReference,_NodeAction_Ctor_BehaviorTreeReference},
			{DotsBT.EBTNode.UnityMathSetInt,_NodeAction_Ctor_UnityMathSetInt},
			{DotsBT.EBTNode.ParallelComplete,_NodeAction_Ctor_ParallelComplete},
			{DotsBT.EBTNode.ReturnSuccess,_NodeAction_Ctor_ReturnSuccess},
		};
		private static void _NodeAction_Ctor_TimerInterrupt(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeTimerInterrupt*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_ParallelSelector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeParallelSelector*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_UntilFailure(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeUntilFailure*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Parallel(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeParallel*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Repeater(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeRepeater*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_RandomSelector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeRandomSelector*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_CaseIntComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeCaseIntComparison*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Inverter(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeInverter*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_UnityMathBoolComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeUnityMathBoolComparison*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_RandomSequence(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeRandomSequence*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_TestReturnAction(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeTestReturnAction*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Rotate(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.UnityTransform.BTNodeRotate*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_TimeRepeater(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeTimeRepeater*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_TimerCheck(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeTimerCheck*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_TimerReset(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeTimerReset*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_RandomProbability(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeRandomProbability*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_UntilSuccess(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeUntilSuccess*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_LogFormat(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeLogFormat*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_TestDecorator(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeTestDecorator*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Wait(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeWait*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_TimerStart(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeTimerStart*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Selector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeSelector*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_ReturnFailure(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeReturnFailure*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_SetPosition(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.UnityTransform.BTNodeSetPosition*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_UnityMathIntComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeUnityMathIntComparison*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Log(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeLog*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_CompareSharedBool(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.SharedVariables.BTNodeCompareSharedBool*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Sequence(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeSequence*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_Decorator(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeDecorator*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_CaseBoolComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeCaseBoolComparison*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_BehaviorTreeReference(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.SubTree.BTNodeBehaviorTreeReference*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_UnityMathSetInt(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeUnityMathSetInt*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_ParallelComplete(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeParallelComplete*)p)->Ctor(ref vm, task, context);}
		private static void _NodeAction_Ctor_ReturnSuccess(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm,  BehaviorDesigner.Runtime.Tasks.Task task,  DotsBT.BTBakeContextNode context){((DotsBT.BTNodeReturnSuccess*)p)->Ctor(ref vm, task, context);}
		#endregion

		#region NodeAction_Update
		public delegate DotsBT.EBTStatus NodeAction_Update(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm);
		public static readonly Dictionary<DotsBT.EBTNode, NodeAction_Update> _NodeAction_UpdateDict = new()
		{
			{DotsBT.EBTNode.TimerInterrupt,_NodeAction_Update_TimerInterrupt},
			{DotsBT.EBTNode.ParallelSelector,_NodeAction_Update_ParallelSelector},
			{DotsBT.EBTNode.UntilFailure,_NodeAction_Update_UntilFailure},
			{DotsBT.EBTNode.Parallel,_NodeAction_Update_Parallel},
			{DotsBT.EBTNode.Repeater,_NodeAction_Update_Repeater},
			{DotsBT.EBTNode.RandomSelector,_NodeAction_Update_RandomSelector},
			{DotsBT.EBTNode.CaseIntComparison,_NodeAction_Update_CaseIntComparison},
			{DotsBT.EBTNode.Inverter,_NodeAction_Update_Inverter},
			{DotsBT.EBTNode.UnityMathBoolComparison,_NodeAction_Update_UnityMathBoolComparison},
			{DotsBT.EBTNode.RandomSequence,_NodeAction_Update_RandomSequence},
			{DotsBT.EBTNode.TestReturnAction,_NodeAction_Update_TestReturnAction},
			{DotsBT.EBTNode.Rotate,_NodeAction_Update_Rotate},
			{DotsBT.EBTNode.TimeRepeater,_NodeAction_Update_TimeRepeater},
			{DotsBT.EBTNode.TimerCheck,_NodeAction_Update_TimerCheck},
			{DotsBT.EBTNode.TimerReset,_NodeAction_Update_TimerReset},
			{DotsBT.EBTNode.RandomProbability,_NodeAction_Update_RandomProbability},
			{DotsBT.EBTNode.UntilSuccess,_NodeAction_Update_UntilSuccess},
			{DotsBT.EBTNode.LogFormat,_NodeAction_Update_LogFormat},
			{DotsBT.EBTNode.TestDecorator,_NodeAction_Update_TestDecorator},
			{DotsBT.EBTNode.Wait,_NodeAction_Update_Wait},
			{DotsBT.EBTNode.TimerStart,_NodeAction_Update_TimerStart},
			{DotsBT.EBTNode.Selector,_NodeAction_Update_Selector},
			{DotsBT.EBTNode.ReturnFailure,_NodeAction_Update_ReturnFailure},
			{DotsBT.EBTNode.SetPosition,_NodeAction_Update_SetPosition},
			{DotsBT.EBTNode.UnityMathIntComparison,_NodeAction_Update_UnityMathIntComparison},
			{DotsBT.EBTNode.Log,_NodeAction_Update_Log},
			{DotsBT.EBTNode.CompareSharedBool,_NodeAction_Update_CompareSharedBool},
			{DotsBT.EBTNode.Sequence,_NodeAction_Update_Sequence},
			{DotsBT.EBTNode.Decorator,_NodeAction_Update_Decorator},
			{DotsBT.EBTNode.CaseBoolComparison,_NodeAction_Update_CaseBoolComparison},
			{DotsBT.EBTNode.BehaviorTreeReference,_NodeAction_Update_BehaviorTreeReference},
			{DotsBT.EBTNode.UnityMathSetInt,_NodeAction_Update_UnityMathSetInt},
			{DotsBT.EBTNode.ParallelComplete,_NodeAction_Update_ParallelComplete},
			{DotsBT.EBTNode.ReturnSuccess,_NodeAction_Update_ReturnSuccess},
		};
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_TimerInterrupt(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeTimerInterrupt*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_ParallelSelector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeParallelSelector*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_UntilFailure(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeUntilFailure*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Parallel(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeParallel*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Repeater(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeRepeater*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_RandomSelector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeRandomSelector*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_CaseIntComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeCaseIntComparison*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Inverter(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeInverter*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_UnityMathBoolComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeUnityMathBoolComparison*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_RandomSequence(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeRandomSequence*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_TestReturnAction(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeTestReturnAction*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Rotate(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.UnityTransform.BTNodeRotate*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_TimeRepeater(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeTimeRepeater*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_TimerCheck(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeTimerCheck*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_TimerReset(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeTimerReset*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_RandomProbability(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeRandomProbability*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_UntilSuccess(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeUntilSuccess*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_LogFormat(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeLogFormat*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_TestDecorator(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeTestDecorator*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Wait(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeWait*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_TimerStart(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeTimerStart*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Selector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeSelector*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_ReturnFailure(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeReturnFailure*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_SetPosition(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.UnityTransform.BTNodeSetPosition*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_UnityMathIntComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeUnityMathIntComparison*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Log(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeLog*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_CompareSharedBool(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.SharedVariables.BTNodeCompareSharedBool*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Sequence(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeSequence*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_Decorator(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeDecorator*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_CaseBoolComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeCaseBoolComparison*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_BehaviorTreeReference(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.SubTree.BTNodeBehaviorTreeReference*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_UnityMathSetInt(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeUnityMathSetInt*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_ParallelComplete(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeParallelComplete*)p)->Update(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Update))]
		private static DotsBT.EBTStatus _NodeAction_Update_ReturnSuccess(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){return ((DotsBT.BTNodeReturnSuccess*)p)->Update(ref vm);}
		#endregion

		#region NodeAction_Stop
		public delegate void NodeAction_Stop(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm);
		public static readonly Dictionary<DotsBT.EBTNode, NodeAction_Stop> _NodeAction_StopDict = new()
		{
			{DotsBT.EBTNode.TimerInterrupt,_NodeAction_Stop_TimerInterrupt},
			{DotsBT.EBTNode.ParallelSelector,_NodeAction_Stop_ParallelSelector},
			{DotsBT.EBTNode.UntilFailure,_NodeAction_Stop_UntilFailure},
			{DotsBT.EBTNode.Parallel,_NodeAction_Stop_Parallel},
			{DotsBT.EBTNode.Repeater,_NodeAction_Stop_Repeater},
			{DotsBT.EBTNode.RandomSelector,_NodeAction_Stop_RandomSelector},
			{DotsBT.EBTNode.CaseIntComparison,_NodeAction_Stop_CaseIntComparison},
			{DotsBT.EBTNode.Inverter,_NodeAction_Stop_Inverter},
			{DotsBT.EBTNode.UnityMathBoolComparison,_NodeAction_Stop_UnityMathBoolComparison},
			{DotsBT.EBTNode.RandomSequence,_NodeAction_Stop_RandomSequence},
			{DotsBT.EBTNode.TestReturnAction,_NodeAction_Stop_TestReturnAction},
			{DotsBT.EBTNode.Rotate,_NodeAction_Stop_Rotate},
			{DotsBT.EBTNode.TimeRepeater,_NodeAction_Stop_TimeRepeater},
			{DotsBT.EBTNode.TimerCheck,_NodeAction_Stop_TimerCheck},
			{DotsBT.EBTNode.TimerReset,_NodeAction_Stop_TimerReset},
			{DotsBT.EBTNode.RandomProbability,_NodeAction_Stop_RandomProbability},
			{DotsBT.EBTNode.UntilSuccess,_NodeAction_Stop_UntilSuccess},
			{DotsBT.EBTNode.LogFormat,_NodeAction_Stop_LogFormat},
			{DotsBT.EBTNode.TestDecorator,_NodeAction_Stop_TestDecorator},
			{DotsBT.EBTNode.Wait,_NodeAction_Stop_Wait},
			{DotsBT.EBTNode.TimerStart,_NodeAction_Stop_TimerStart},
			{DotsBT.EBTNode.Selector,_NodeAction_Stop_Selector},
			{DotsBT.EBTNode.ReturnFailure,_NodeAction_Stop_ReturnFailure},
			{DotsBT.EBTNode.SetPosition,_NodeAction_Stop_SetPosition},
			{DotsBT.EBTNode.UnityMathIntComparison,_NodeAction_Stop_UnityMathIntComparison},
			{DotsBT.EBTNode.Log,_NodeAction_Stop_Log},
			{DotsBT.EBTNode.CompareSharedBool,_NodeAction_Stop_CompareSharedBool},
			{DotsBT.EBTNode.Sequence,_NodeAction_Stop_Sequence},
			{DotsBT.EBTNode.Decorator,_NodeAction_Stop_Decorator},
			{DotsBT.EBTNode.CaseBoolComparison,_NodeAction_Stop_CaseBoolComparison},
			{DotsBT.EBTNode.BehaviorTreeReference,_NodeAction_Stop_BehaviorTreeReference},
			{DotsBT.EBTNode.UnityMathSetInt,_NodeAction_Stop_UnityMathSetInt},
			{DotsBT.EBTNode.ParallelComplete,_NodeAction_Stop_ParallelComplete},
			{DotsBT.EBTNode.ReturnSuccess,_NodeAction_Stop_ReturnSuccess},
		};
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_TimerInterrupt(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeTimerInterrupt*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_ParallelSelector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeParallelSelector*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_UntilFailure(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeUntilFailure*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Parallel(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeParallel*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Repeater(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeRepeater*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_RandomSelector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeRandomSelector*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_CaseIntComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeCaseIntComparison*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Inverter(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeInverter*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_UnityMathBoolComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeUnityMathBoolComparison*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_RandomSequence(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeRandomSequence*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_TestReturnAction(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeTestReturnAction*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Rotate(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.UnityTransform.BTNodeRotate*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_TimeRepeater(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeTimeRepeater*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_TimerCheck(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeTimerCheck*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_TimerReset(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeTimerReset*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_RandomProbability(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeRandomProbability*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_UntilSuccess(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeUntilSuccess*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_LogFormat(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeLogFormat*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_TestDecorator(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeTestDecorator*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Wait(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeWait*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_TimerStart(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeTimerStart*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Selector(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeSelector*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_ReturnFailure(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeReturnFailure*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_SetPosition(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.UnityTransform.BTNodeSetPosition*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_UnityMathIntComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeUnityMathIntComparison*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Log(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeLog*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_CompareSharedBool(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.SharedVariables.BTNodeCompareSharedBool*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Sequence(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeSequence*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_Decorator(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeDecorator*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_CaseBoolComparison(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeCaseBoolComparison*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_BehaviorTreeReference(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.SubTree.BTNodeBehaviorTreeReference*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_UnityMathSetInt(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeUnityMathSetInt*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_ParallelComplete(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeParallelComplete*)p)->Stop(ref vm);}
		[BurstCompile]
		[AOT.MonoPInvokeCallback(typeof(NodeAction_Stop))]
		private static void _NodeAction_Stop_ReturnSuccess(DotsBT.BTNodeBase* p, ref DotsBT.BTVM vm){((DotsBT.BTNodeReturnSuccess*)p)->Stop(ref vm);}
		#endregion

	}
}
