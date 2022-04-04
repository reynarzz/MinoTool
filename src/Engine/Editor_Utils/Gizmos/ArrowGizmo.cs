using GlmNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    internal class ArrowGizmo : GizmoEntity
    {
        private readonly Action<vec3> result;
        private vec3 pos;
        private vec3 dir;

        public ArrowGizmo(int gizmoIndex, Mesh mesh, vec3 position, vec3 direction, Action<vec3> result) : base(gizmoIndex, mesh)
        {
            pos = position;
            dir = direction;
            this.result = result;
            transform.position = pos;

            if (direction.x > 0)
            {
                transform.eulerAngles = new vec3(0, 0, 90);
            }
            else if (direction.z > 0)
            {
                transform.eulerAngles = new vec3(90, 0, 0);
            }

            transform.localScale = new vec3(2.3f, 2.3f, 2.3f);
            Renderer.material.color = new Color(direction.x, direction.y, direction.z, 1);
        }

        internal override void Update(Camera camera)
        {
            if (Input.IsMouseButton(GLFW.MouseButton.Button1))
            {
                var mg = (Input.MousePositionDelta.x + -Input.MousePositionDelta.y) / 2f;

                var avg = (camera.Right + camera.Up + camera.Forward) / 3;

                pos = transform.position += avg * mg * dir * 20;

                result(transform.position);
            }
        }
    }
}