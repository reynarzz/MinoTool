using System;
using System.Collections.Generic;
using System.Text;
using static OpenGL.GL;

namespace MinoTool
{
    // Renders every object with a particular color for color picking.
    internal class RenderEntityIDPass : PassBase
    {
        private readonly ColorID _colorID;
        private readonly Shader _defaultShader;
        private FrameBuffer _frameBuffer;
        private byte[] _colorBytes;

        public uint FrameBufferTexID => _frameBuffer.ColotTex;
        public RenderEntityIDPass()
        {
            _colorID = new ColorID();
            _defaultShader = MaterialFactory.Instance.GetDefaultShader();
            _colorBytes = new byte[4 * 16];
            _frameBuffer = new FrameBuffer(Screen.Width, Screen.Heigh);
        }

        internal override void DrawPass(Camera camera, List<Renderer> renderers)
        {
            _frameBuffer.Bind();
            base.Clear();

            glViewport(0, 0, _frameBuffer.Width, _frameBuffer.Height);

            for (int i = 0; i < renderers.Count; i++)
            {
                // Background Color.
                var renderer = renderers[i];

                if ((renderer.enabled || renderer.PickableAlways) && (Selector.Current != renderer.gameObject) || renderer.IsGizmo)
                {
                    var mesh = renderer.GetMesh();
                    if (renderer.IsGizmo)
                    {
                        glDisable(GL_DEPTH_TEST);
                    }
                    else
                    {
                        glEnable(GL_DEPTH_TEST);

                    }
                    mesh.Bind();
                    //renderer.Bind(camera.Transform_Test.TransformMatrix, camera.Projection);

                    _defaultShader.Bind();
                    _defaultShader.SetDataLayout(new MeshLayoutInfo() { VertsComponentsCount = 3, UVComponentsCount = 2 });
                    var mvpM = camera.Projection * camera.Transform_Test.TransformMatrix * renderer.transform.TransformMatrix;

                    // MVP matrix
                    _defaultShader.Set("_MVP", mvpM);

                    var color = _colorID.GetColor(i);

                    var c = glGetUniformLocation(_defaultShader.ProgramID, "_Color");
                    glUniform4f(c, color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);

                    // Draw shape.
                    glDrawElements(mesh.DrawMode, mesh.Indices);

                    renderer.Unbind();
                }
            }

            glViewport(0, 0, Screen.Width, Screen.Heigh);

            glReadPixels((int)Input.MousePosition.x, Screen.Heigh - (int)Input.MousePosition.y, 1, 1, GL_RGBA, GL_UNSIGNED_BYTE, _colorBytes);

            var colors = new ColorInt(_colorBytes[0], _colorBytes[1], _colorBytes[2], _colorBytes[3]);

            _frameBuffer.Unbind();
            _frameBuffer.DeleteFrameBuffer();

            if (_colorID.GetIndex(colors, out var index))
            {
                var pickedRenderer = renderers[index];
                Selector.Internal.SelectByID(pickedRenderer.EntityID, pickedRenderer.IsGizmo);
            }
            else
            {
                Selector.UnselectAll();
            }
        }

        internal override void UpdateViewport(int w, int h)
        {
            _frameBuffer.DeleteFrameBuffer();
            _frameBuffer = new FrameBuffer(w, h);
        }
    }
}
