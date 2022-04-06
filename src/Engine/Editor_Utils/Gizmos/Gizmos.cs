using System;
using System.Collections.Generic;
using GlmNet;
using static OpenGL.GL;

namespace MinoTool
{
    [Obsolete]
    public static class Gizmos
    {
        internal static List<GizmoEntity> Entities { get; set; }
        public static List<MeshRenderer> Renderers { get; private set; }
        private static Mesh _arrowMesh;

        static Gizmos()
        {
            //_arrowMesh = MeshUtility.LoadMesh("_assets/_models/3DSingleArrow.obj", false);
            Entities = new List<GizmoEntity>();
            Renderers = new List<MeshRenderer>();
        }

        public static void DragableArrow(vec3 position, vec3 direction, Action<vec3> result)
        {
            //var gizmo = new ArrowGizmo(Entities.Count + 1, _arrowMesh, position, direction, result);

            //Renderers.Add(gizmo.Renderer);
            //gizmo.Renderer.transform = gizmo.transform;

            ////gizmo.transform.localEulerAngles = LookAt2D(direction);
            //Entities.Add(gizmo);
        }

        internal static void RenderGizmos(Camera cam)
        {
            glDisable(GL_DEPTH_TEST);

            for (int i = 0; i < Entities.Count; i++)
            {
                var gizmo = Entities[i];

                if (Selector.Internal.CurrentGizmo != 0 && Selector.Internal.CurrentGizmo == gizmo.GizmoIndex)
                {
                    gizmo.Update(cam);
                }

                var renderer = gizmo.Renderer;

                renderer.Bind(gizmo.transform.TransformMatrix, cam.Transform_Test.TransformMatrix, cam.Projection);

                glDrawElements(GL_TRIANGLES, gizmo.Mesh.Indices);

                Entities[i].Destroy();
            }

           // glEnable(GL_DEPTH_TEST);

            Entities.Clear();
            Renderers.Clear();

            if (!Input.IsMouseButton(GLFW.MouseButton.Button1))
                Selector.Internal.UnselectGizmo_Internal();
        }

        //private vec3 LookAt2D(vec3 dir)
        //{
        //    var euler = new vec3();
        //    glm.atan(direction.z, direction.x) *
        //}
    }
}