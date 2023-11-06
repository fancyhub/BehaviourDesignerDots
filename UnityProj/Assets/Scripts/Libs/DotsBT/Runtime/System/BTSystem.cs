using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace DotsBT
{
    //[DisableAutoCreation]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class BTSystemGroup : ComponentSystemGroup
    {

    }


    [UpdateInGroup(typeof(BTSystemGroup))]
    public partial struct BTSetupSystem : ISystem
    {

    }


    [UpdateInGroup(typeof(BTSystemGroup))]
    public partial struct BTExeSystem : ISystem
    {

    }

    [UpdateInGroup(typeof(BTSystemGroup))]
    public partial struct BTCleanSystem : ISystem
    {

    }
}
