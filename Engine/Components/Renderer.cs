using GlmNet;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MinoTool
{
    public abstract class Renderer : Component
    {
        public Material material { get; set; }
        /// <summary>Tells to the engine that this entity will be pickable (selectable) by the mouse, not matter if the renderer is enabled or not.</summary>
        public bool PickableAlways { get; set; }
        public bool IsGizmo { get; set; }
        public Renderer() 
        {
        }

        /// <summary>
        /// Ready for drawing.
        /// </summary>
        internal virtual void Bind(mat4 viewM, mat4 projM)
        {
            if (material != null)
            {
                material.Bind(gameObject.transform.TransformMatrix, viewM, projM);
            }
            else
            {
                LOG.Log(name + "Doesn't have a material");
            }
        }

        internal virtual void Bind(mat4 modelM, mat4 viewM, mat4 projM)
        {
            material.Bind(modelM, viewM, projM);
        }

        public abstract Mesh GetMesh();

        internal override void _OnDestroyInternal()
        {
            base._OnDestroyInternal();
            LOG.Log("Destroy Renderer");

            //EntitiesManager.Instance.Renderables.RemoveRenderer(this);
        }

        internal void Unbind()
        {
            material.Unbind();
        }
    }
}
