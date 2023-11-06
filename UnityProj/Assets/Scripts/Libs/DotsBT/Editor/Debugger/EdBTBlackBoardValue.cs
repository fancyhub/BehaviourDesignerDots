using System;
using Sirenix.OdinInspector;

namespace DotsBT.Debugger.ED
{
    [Serializable]
    public class EdBTBlackBoardValue
    {
        public string Name;
        public bool Used;
        public EBTVarScope Scope;
        [ShowInInspector]
        public object Value;
    }
}
