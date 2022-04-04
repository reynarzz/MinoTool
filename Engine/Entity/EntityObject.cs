using System;
using System.Collections.Generic;
using System.Linq;

namespace MinoTool
{
    /// <summary>An entity that exist in the scene</summary>
    public class EntityObject : Entity
    {
        public Dictionary<Type, Component> _components { get; set; }
        public bool activeSelf { get; set; }
        public bool activeInHierarchy { get; set; }

        public Transform transform { get; private set; }

        internal bool AddedToEntitiesManager { get; set; }

        public EntityObject() : this("EmptyEntity", default(Type[])) { }
        public EntityObject(params Type[] components) : this("EmptyEntity", components) { }
        public EntityObject(string name) : this(name, default(Type[])) { }

        internal bool IsAlive { get; set; }

        public EntityObject(string name, params Type[] components)
        {
            _components = new Dictionary<Type, Component>();
            transform = new Transform(this);
            
            this.name = name;
            IsAlive = true;

            EntitiesManager.Instance.TOSCENE_AddEntityObj(this, components);
        }

        public T AddComponent<T>() where T : Component
        {
            if (!CheckIfAlive())
                return default;

            return AddComponent(typeof(T)) as T;
        }

        public Component AddComponent(Type type)
        {
            if (!CheckIfAlive())
                return default;

            var component = default(Component);

            if (!_components.ContainsKey(type))
            {
                if (!type.IsSubclassOf(typeof(EntityBehaviour)))
                {
                    // Get a new component
                    component = ComponentsFactory.Instance.CreateInternalComponent(type, this);
                }
                else
                {
                    //LOG.Warning("Create entity");
                    component = EntitiesManager.Instance.CreateEntityBehaviour(type, this);
                }

                _components.Add(type, component);

                if (AddedToEntitiesManager)
                {
                    //-LOG.Log("Register: " + component.GetType().Name);
                    EntitiesManager.Instance.RegisterComponent(component);
                }
            }
            else
            {
                component = GetComponent(type);

                LOG.Warning("Tried to add an existent component: " + component.GetType().Name);
            }

            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            if (!CheckIfAlive())
                return default;

            return GetComponent(typeof(T)) as T;
        }

        public Component GetComponent(Type type)
        {
            if (!CheckIfAlive())
                return default;

            for (int i = 0; i < _components.Values.Count; i++)
            {
                var comp = _components.Values.ElementAt(i);
                if (isValidComponent(comp, type))
                {
                    return comp;
                }
            }

            return default;
        }

        public T[] GetComponents<T>() where T : Component
        {
            if (!CheckIfAlive())
                return default;

            var components = new List<T>();
            var type = typeof(T);

            for (int i = 0; i < _components.Values.Count; i++)
            {
                var comp = _components.Values.ElementAt(i);
                if (isValidComponent(comp, type))
                {
                    components.Add(comp as T);
                }
            }

            return components.ToArray();
        }

        private bool isValidComponent(Component comp, Type type)
        {
            return comp.GetType() == type || comp.GetType().IsAssignableFrom(type) || comp.GetType().IsSubclassOf(type);
        }

        public static void DestroyImmediate(EntityObject gameObject)
        {
            EntitiesManager.Instance.Destroy(ref gameObject);
        }

        public void SetActive(bool enabled)
        {
            activeInHierarchy = enabled;
            activeSelf = enabled;

            foreach (var item in _components)
            {
                item.Value.enabled = enabled;
            }
        }


        public T GetComponentInChildren<T>()
        {
            throw new Exception("TODO");
        }

        internal override void _OnDestroyInternal()
        {
            foreach (var component in _components)
            {
                component.Value._OnDestroyInternal();
            }

            transform._OnDestroyInternal();

            _components.Clear();
        }

        private bool CheckIfAlive()
        {
            if (!IsAlive)
            {
                LOG.Error("Trying to use the destroyed object \"" + name + "\"");
            }

            return IsAlive;
        }
    }
}