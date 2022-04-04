using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public class GizmoEntity : Entity
    {
        public MeshRenderer Renderer { get; set; }
        public Transform transform { get; set; }
        public Mesh Mesh { get; set; }
        public int GizmoIndex { get; private set; }
        public GizmoEntity(int gizmoIndex, Mesh mesh)
        {
            GizmoIndex = gizmoIndex;
            Mesh = mesh;
            Renderer = new MeshRenderer(mesh, MaterialFactory.Instance.GetUnlitMaterial());
            transform = new Transform();
            Renderer.IsGizmo = true;
            Renderer.name = "Gizmo";
        }
        internal virtual void Update(Camera camera) { }

        //public void OnClickDown()
        //{

        //}

        //public void OnClickUp()
        //{

        //}


        internal void Destroy()
        {
            Renderer.Dispose();
        }
    }
}
