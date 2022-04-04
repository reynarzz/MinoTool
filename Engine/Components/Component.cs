using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    [Serializable]
    public abstract class Component : Entity
    {
        internal EntityObject _owner_internal { get; set; }
        public virtual EntityObject gameObject => _owner_internal;
        private Transform _defaultTransform;

        public Transform transform
        {
            get
            {
                if (_owner_internal != null)
                    return _owner_internal.transform;

                return _defaultTransform;
            }
            internal set
            {
                _defaultTransform = value;
            }
        }
        private string _defaultName;
        public override string name
        {
            get
            {
                if (_owner_internal != null)
                    return _owner_internal.name;

                return _defaultName;
            }
            set
            {
                if (_owner_internal != null)
                    _owner_internal.name = value;

                _defaultName = value;
            }
        }

        public bool enabled { get; set; } = true;

        public event Action<Component> OnComponentRemoved;

        public Component()
        {
        }

        internal override void _OnDestroyInternal()
        {
            OnComponentRemoved?.Invoke(this);
            LOG.LOG_PLAIN("Destroy: " + name);
            OnComponentRemoved = null;
        }
    }
}