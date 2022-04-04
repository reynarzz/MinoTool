using System;
using System.Collections.Generic;

namespace MinoTool
{
    public class ComponentsFactory : Singleton<ComponentsFactory>
    {
        private Dictionary<Type, Func<Component>> _components;

        public ComponentsFactory()
        {
            _components = new Dictionary<Type, Func<Component>>()
            {
                { typeof(MeshRenderer), GetMeshRenderer },
                { typeof(LineRenderer), GetLineRenderer },

            };
        }

        internal Component CreateInternalComponent(Type type, EntityObject owner)
        {
            var callback = _components[type];

            var componet = callback();
            componet._owner_internal = owner;

            return componet;
        }

        private LineRenderer GetLineRenderer()
        {
            return new LineRenderer();
        }

        private MeshRenderer GetMeshRenderer()
        {
            return new MeshRenderer(MeshUtility.GetQuadMesh(), MaterialFactory.Instance.GetDefaultMaterial());
        }
    }
}
