using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MinoTool
{
    internal class EntitiesToUpdate
    {
        private Dictionary<string, List<EntityBehaviour>> _entities;

        internal EntitiesToUpdate()
        {
            _entities = new Dictionary<string, List<EntityBehaviour>>();
        }

        internal void AddBehaviour(EntityBehaviour entity)
        {
            if (_entities.TryGetValue(entity.EntityID, out var bevaiourList))
            {
                bevaiourList.Add(entity);
            }
            else
            {
                _entities.Add(entity.EntityID, new List<EntityBehaviour>() { entity });
            }
        }

        internal void RemoveBehavior(EntityBehaviour entity)
        {
            if (_entities.TryGetValue(entity.EntityID, out var behaviors))
            {
                LOG.Log(behaviors.Count);
                behaviors.Remove(entity);
                Console.WriteLine("Remove behaviour: " + entity.GetType().Name + ", total: " + behaviors.Count);

                if (behaviors.Count == 0)
                {
                    _entities.Remove(entity.EntityID);
                }
            }
            else
            {
                Console.WriteLine("Doesn't exist behavior: " + entity.GetType().Name);
            }
        }

        //internal void RemoveAllBehaviorsFromObj(EntityObject entity)
        //{
        //    var behaviors = _entities[entity.EntityID];

        //    for (int i = 0; i < behaviors.Count; i++)
        //    {
        //        behaviors[i].OnDestroy();
        //    }

        //    _entities[entity.EntityID].Clear();

        //    _entities.Remove(entity.EntityID);
        //}

        //internal void Awake()
        //{
        //    foreach (var entity in _entities.Values)
        //    {
        //        for (int i = 0; i < entity.Count; i++)
        //        {
        //            entity[i].Awake();
        //        }
        //    }
        //}

        //internal void Start()
        //{
        //    foreach (var entity in _entities.Values)
        //    {
        //        for (int i = 0; i < entity.Count; i++)
        //        {
        //            entity[i].Start();
        //        }
        //    }
        //}

        internal void Update(Time time)
        {
            foreach (var entity in _entities.Values)
            {
                for (int i = 0; i < entity.Count; i++)
                {
                    entity[i].Update(time);
                }
            }
        }
    }
}
