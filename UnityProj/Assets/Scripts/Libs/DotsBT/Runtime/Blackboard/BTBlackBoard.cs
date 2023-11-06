using System;
using System.Collections.Generic;
using Unity.Entities;

namespace DotsBT
{
    public struct BTBBBoxedValue
    {
        public int NameId; //NameId -> string 的映射关系, 编辑器自己到对应的资源里面获取
        public EBTVarScope Scope;
        public object Value;
    }

    public unsafe interface IBTBlackBoard
    {
        //link的时候用的
        public BTPtr LinkVar(int name_id, EBTVarScope scope);

        public Entity GetOwnerEntity();

        //这个是给外部使用的
        public T GetValue<T>(int name_id, EBTVarScope scope) where T : unmanaged;

        //这个是给外部使用的
        public bool SetSharedValue<T>(int name_id, T value) where T : unmanaged;

        //这个是给编辑器使用的
        public void EdGetBoxedValues(List<BTBBBoxedValue> all_values);
    }
}
