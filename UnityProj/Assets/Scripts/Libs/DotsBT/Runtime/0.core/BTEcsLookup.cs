using Unity.Collections;
using Unity.Entities;

namespace DotsBT
{
    //TODO: 后续需要再这里添加需要的所有对象
    public struct BTEcsLookup
    {
        [ReadOnly] public ComponentLookup<Unity.Transforms.LocalTransform> LocalTransform;

        public void Init(ref SystemState state)
        {
            LocalTransform = state.GetComponentLookup<Unity.Transforms.LocalTransform>();
        }

        public void Update(ref SystemState state)
        {
            LocalTransform.Update(ref state);
        }
    }
}