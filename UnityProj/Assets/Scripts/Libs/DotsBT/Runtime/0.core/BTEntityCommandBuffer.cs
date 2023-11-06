using Unity.Collections;
using Unity.Entities;

namespace DotsBT
{
    public struct BTEntityCommandBuffer
    {
        public int _sort_key;
        public EntityCommandBuffer.ParallelWriter _ecb;

        public BTEntityCommandBuffer(EntityCommandBuffer.ParallelWriter ecb_writer, int sort_key)
        {
            _ecb = ecb_writer;
            _sort_key = sort_key;
        }
         
        public void RemoveComponent<T>(Entity e) where T : unmanaged, IComponentData
        {
            if (e == Entity.Null) throw new System.Exception("Null Entity");
            _ecb.RemoveComponent<T>(_sort_key, e);
        }

        public void SetComponent<T>(Entity e, T component) where T : unmanaged, IComponentData
        {
            if (e == Entity.Null) throw new System.Exception("Null Entity");
            _ecb.SetComponent<T>(_sort_key, e, component);
        }

        public void SetComponentEnabled<T>(Entity e, bool enabled) where T : unmanaged, IEnableableComponent
        {
            if (e == Entity.Null) throw new System.Exception("Null Entity");
            _ecb.SetComponentEnabled<T>(_sort_key, e, enabled);
        }

        public void AddComponent<T>(Entity e, T component) where T : unmanaged, IComponentData
        {
            if (e == Entity.Null) throw new System.Exception("Null Entity");
            _ecb.AddComponent(_sort_key, e, component);
        }

        public void AddSharedComponent<T>(Entity e, T component) where T : unmanaged, ISharedComponentData
        {
            if (e == Entity.Null) throw new System.Exception("Null Entity");
            _ecb.AddSharedComponent(_sort_key, e, component);
        }

        public void SetSharedComponent<T>(Entity e, T component) where T : unmanaged, ISharedComponentData
        {
            if (e == Entity.Null) throw new System.Exception("Null Entity");
            _ecb.SetSharedComponent(_sort_key, e, component);
        }

        public Entity CreateEntity()
        {
            return _ecb.CreateEntity(_sort_key);
        }

        public Entity CreateEntity<T1>(T1 component) where T1 : unmanaged, IComponentData
        {
            Entity e = _ecb.CreateEntity(_sort_key);
            _ecb.AddComponent<T1>(_sort_key, e, component);
            return e;
        }

        public Entity CreateEntity<T1, T2>(T1 component1, T2 component2) where T1 : unmanaged, IComponentData where T2 : unmanaged, IComponentData
        {
            Entity e = _ecb.CreateEntity(_sort_key);

            ComponentTypeSet set = new ComponentTypeSet(ComponentType.ReadWrite<T1>(), ComponentType.ReadWrite<T2>());
            _ecb.AddComponent(_sort_key, e, set);

            _ecb.SetComponent<T1>(_sort_key, e, component1);
            _ecb.SetComponent<T2>(_sort_key, e, component2);
            return e;
        }
    }
}