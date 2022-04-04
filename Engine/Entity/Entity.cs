using System;
using System.Collections.Generic;
using System.Text;
using GlmNet;

namespace MinoTool
{
    [Serializable]
    public abstract class Entity
    {
        public virtual string name { get; set; }

        /// <summary>Unique, inmmutable ID of this entity (component or obj, or etc.) during the execution of the program.</summary>
        public string EntityID { get; private set; }

        public Entity()
        {
            EntityID = Guid.NewGuid().ToString();
            //Log.L("ID: " + EntityID);
        }

        public T Instantiate<T>(T obj) where T : Entity
        {
            LOG.Error("Cannot instantiate now");
            return null;
            //--throw new Exception("TODO");
        }

        public T Instantiate<T>(T obj, Vector3 position) where T : Entity
        {
            throw new Exception("TODO");
        }

        public T Instantiate<T>(T obj, Vector3 position, Quaternion orientation) where T : Entity
        {
            throw new Exception("TODO");
        }

        public EntityBehaviour Instantiate(EntityBehaviour obj, Vector3 position, Vector3 euler, Transform parent)
        {
            throw new Exception("TODO");
        }

        public static EntityObject Instantiate(EntityObject obj, Vector3 position = default, Vector3 euler = default, Transform parent = default)
        {
            var entity = default(EntityObject);

            if(obj is EntityObject)
            {
                entity = EntitiesManager.Instance.TOSCENE_CopyEntityObj(obj);

                entity.transform.position = position.Tov3();
                entity.transform.eulerAngles = euler.Tov3();
                entity.transform.parent = parent;
            }

            return entity;
        }

        internal virtual void _OnDestroyInternal() { }

        public virtual void OnDestroy() { }

        public void DestroyImmediate(EntityObject obj)
        {
            EntityObject.DestroyImmediate(obj);
        }
    }
}