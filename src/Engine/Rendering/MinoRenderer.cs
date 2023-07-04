using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;

using static OpenGL.GL;

namespace MinoTool
{
    public class MinoRenderer
    {
        /// <summary>Camera to render to (if any)</summary>
        public Camera MainCamera { get; set; }

        private EntitiesToRender _container;
        private List<MeshRenderer> _selectedObjects;
        private FrameBuffer _mainFrameBuffer;
        private Mesh _quad;
        private Material _screenSpaceQuadMaterial;
        private RenderEntityIDPass _current_IDPass;
        private bool _renderPickUp;

        // Public API
        public static Color BackgroundColor { get; set; } = new Color(0.2f, 0.2f, 0.2f, 1);

        internal MinoRenderer(AppWindow window, EntitiesToRender container)
        {
            _container = container;
            _selectedObjects = new List<MeshRenderer>();

            _current_IDPass = new RenderEntityIDPass();
            ObjectSelectorInternal.Inst.OnObjectSelectionArrayChanged += OnObjectSelectedChanged;

            _mainFrameBuffer = new FrameBuffer(Screen.Width, Screen.Heigh);
            _screenSpaceQuadMaterial = new Material(MaterialFactory.Instance.GetScreenSizeQuadShader());
            _quad = MeshUtility.GetQuadMesh();

            Input.OnMouseButtonDown += OnMouseDown;
            window.OnViewportSizeChanged += OnViewportScreenChanged;
        }

        private void OnViewportScreenChanged(Window arg1, int w, int h)
        {
            _current_IDPass.UpdateViewport(w, h);

            _mainFrameBuffer.DeleteFrameBuffer();

            _mainFrameBuffer = new FrameBuffer(Screen.Width, Screen.Heigh);
        }

        private void OnMouseDown(MouseButton obj)
        {
            if (obj == MouseButton.Button1 && !Input.IsKey(Keys.LeftAlt))
            {
                _renderPickUp = true;
                // Render pickobject pass.
                var renderers = _container._objectsToRender.Values.ToList();
                renderers.AddRange(Gizmos.Renderers);

                _current_IDPass.DrawPass(MainCamera, renderers);
            }
        }

        private void OnObjectSelectedChanged(EntityObject[] obj)
        {
            _selectedObjects.Clear();

            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] != null)
                {
                    var renderer = obj[i].GetComponent<MeshRenderer>();

                    if (renderer != null)
                    {
                        _selectedObjects.Add(renderer);
                    }
                }
            }
        }

        internal void Draw()
        {
            // Clear default frame buffer.
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            glClearColor(BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, BackgroundColor.a);

            //--_mainFrameBuffer.Bind();

            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            glViewport(0, 0, _mainFrameBuffer.Width, _mainFrameBuffer.Height);

            glClearColor(BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, BackgroundColor.a);
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            glEnable(GL_DEPTH_TEST);
            //glDepthFunc(GL_LEQUAL);

            glEnable(GL_BLEND);
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            for (int i = 0; i < _container.Count; i++)
            {
                // Background Color.
                var renderer = _container.GetRenderer(i);
                
                if (renderer.enabled)
                {
                    glBindTexture(GL_TEXTURE_2D, 0);

                    var mesh = renderer.GetMesh();

                    renderer.Bind(MainCamera.Transform_Test.TransformMatrix, MainCamera.Projection);
                    // Draw shape.
                    glDrawElements(mesh.DrawMode, mesh.Indices);

                    renderer.Unbind();
                }
            }

            glLineWidth(1.5f);

            //Render Selected Objects
            for (int i = 0; i < _selectedObjects.Count; i++)
            {
                var renderer = _selectedObjects[i];

                var mesh = renderer.GetMesh();

                var sizeAnim = 1 + (GlmNet.glm.sin(Time.time * 8) + 1) * 0.5f * 0.1f;

                using (var wiredMesh = MeshUtility.GetCubeLinesMesh(mesh.Bounds, sizeAnim))
                {
                    wiredMesh.Bind();

                    renderer.material.Bind(renderer.transform.TransformMatrix, MainCamera.Transform_Test.TransformMatrix, MainCamera.Projection);
                    glBindTexture(GL_TEXTURE_2D, 0);
                    var defColor = renderer.material.color;

                    renderer.material.color = Color.white;

                    glDrawElements(GL_LINES, wiredMesh.Indices);

                    // Set obj defaults.
                    renderer.material.color = defColor;
                    renderer.Unbind();
                }
            }
            Gizmos.RenderGizmos(MainCamera);

            _mainFrameBuffer.Unbind();

            // Start drawing to a texture.

            glViewport(0, 0, Screen.Width, Screen.Heigh);

            //--_quad.Bind();

            //--_screenSpaceQuadMaterial.Bind(GlmNet.mat4.identity(), GlmNet.mat4.identity(), GlmNet.mat4.identity());
            //_screenSpaceQuadMaterial.color = Color.red;

            glActiveTexture(0);
            //--glBindTexture(GL_TEXTURE_2D, _mainFrameBuffer.ColotTex);

            //--glDrawElements(GL_TRIANGLES, _quad.Indices);

            //--_screenSpaceQuadMaterial.Unbind();
        }
    }
}