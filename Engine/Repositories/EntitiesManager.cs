using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    internal class EntitiesManager
    {
        private static EntitiesManager _inst;
        public static EntitiesManager Instance => _inst ?? (_inst = new EntitiesManager());

        public EntitiesToRender Renderables { get; private set; }
        public EntitiesToUpdate Behaviours { get; private set; }

        private Dictionary<string, EntityObject> _objects;


        private EntitiesManager()
        {
            Renderables = new EntitiesToRender();
            Behaviours = new EntitiesToUpdate();

            _objects = new Dictionary<string, EntityObject>();
        }

        internal EntityBehaviour CreateEntityBehaviour(Type behaviorType, EntityObject obj)
        {
            var behaviour = (EntityBehaviour)Activator.CreateInstance(behaviorType);
            behaviour._owner_internal = obj;

            behaviour.Awake();
            behaviour.Start();

            return behaviour;
        }

        internal void RegisterComponent(Component component)
        {
            OnComponentAdded(component);
        }

        internal void TOSCENE_AddEntityObj(EntityObject obj, params Type[] components)
        {
            obj.AddedToEntitiesManager = true;

            if(components != null)
            {
                for (int i = 0; i < components.Length; i++)
                {
                    //LOG.Error(components[i].Name);
                    obj.AddComponent(components[i]);
                }
            }
            else
            {
                LOG.LOG_PLAIN("No components for: " + obj.name);
            }

            _objects.Add(obj.EntityID, obj);
        }

        internal EntityObject TOSCENE_CopyEntityObj(EntityObject obj)
        {
            var clone = Utils.cloneObject(obj, new List<string>());

            clone.AddedToEntitiesManager = true;

            var renderer = obj.GetComponent<MeshRenderer>();
            var behaviour = obj.GetComponents<EntityBehaviour>();

            Renderables.AddRenderer(renderer);

            for (int i = 0; i < behaviour.Length; i++)
            {
                Behaviours.AddBehaviour(behaviour[i]);
            }

            _objects.Add(clone.EntityID, clone);

            return clone;
        }

        public void Destroy(ref EntityObject entity)
        {
            entity.IsAlive = false;

            LOG.Log("Destroy: " + entity.name);

            entity.OnDestroy();
            entity._OnDestroyInternal();

            _objects.Remove(entity.EntityID);
        }

        private void OnComponentAdded(Component obj)
        {
            if (obj is Renderer)
            {
                Renderables.AddRenderer(obj as Renderer);
                Console.WriteLine(obj.name);
            }
            else if (obj is EntityBehaviour)
            {
                Behaviours.AddBehaviour(obj as EntityBehaviour);
            }

            obj.OnComponentRemoved += OnComponentRemoved;
        }

        private void OnComponentRemoved(Component obj)
        {
            if (obj is Renderer)
            {
                Renderables.RemoveRenderer(obj as Renderer);
            }
            else if (obj.GetType().IsAssignableFrom(typeof(EntityBehaviour)))
            {
                Behaviours.RemoveBehavior(obj as EntityBehaviour);
            }
        }
    }
}