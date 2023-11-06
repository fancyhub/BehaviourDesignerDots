using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;

namespace DotsBT.Debugger.ED
{
    public static class EdUnitCollector
    {
        public static List<EdUnit> Collect()
        {
            Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp;
            EntityQueryBuilder entity_builder = new EntityQueryBuilder(allocator)
                                                .WithAll<DotsBT.BTAssetCompData>();
            List<EdUnit> ret = new();
            foreach (var world in World.All)
            {
                NativeArray<Entity> e_list = entity_builder.Build(world.EntityManager).ToEntityArray(allocator);
                foreach (var e in e_list)
                {
                    BTAssetCompData asset_comp_data = world.EntityManager.GetComponentData<BTAssetCompData>(e);

                    var unit = EdUnit.Create(0, world, e, null);
                    unit.Asset = BTBaker.EdGetWeakRefObj(asset_comp_data.Image);
                    unit.HasRunData = world.EntityManager.HasComponent<BTComponentRunTimeData>(e);
                    if (unit.HasRunData)
                    {
                        unit.HasRunData = world.EntityManager.GetComponentData<BTComponentRunTimeData>(e).Valid;
                    }
                    ret.Add(unit);
                }
            }
            return ret;
        }
    }

}
