using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public abstract class EntityBehaviour : Behaviour
    {
        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update(Time time) { }

        public EntityBehaviour() : base()
        {
        }

        internal override void _OnDestroyInternal()
        {
            EntitiesManager.Instance.Behaviours.RemoveBehavior(this);
            
            base._OnDestroyInternal();
            LOG.Log("Destroy behaviour");
        }
    }
}