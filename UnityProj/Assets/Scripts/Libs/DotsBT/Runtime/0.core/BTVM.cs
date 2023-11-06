namespace DotsBT
{
    public struct BTVM
    {
        public BTMemory Memory;

        public Unity.Core.TimeData Time;
        public BTEntityCommandBuffer Ecb;
        public BTEcsLookup EcsLookup;
        public BTBlackBoard BlackBoard; //运行的时候会有
        public BTNodeVT.NodeActionDictRT NodeActionVT;
        public BTDebugStatusArray DebugStatusArray;
    }
}
