using GlmNet;
using ImGuiNET;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static OpenGL.GL;
using System.Windows.Input;

namespace MinoTool
{
    /// <summary>
    /// A modified version of Veldrid.ImGui's ImGuiRenderer.
    /// Manages input for ImGui and handles rendering ImGui's DrawLists with Veldrid.
    /// </summary>
    public class ImGuiController : IDisposable
    {
        private bool _frameBegun;

        private uint _vertexArray;
        private uint _vertexBuffer;
        private int _vertexBufferSize;
        private uint _indexBuffer;
        private int _indexBufferSize;

        private Texture _fontTexture;
        private Shader _shader;

        private int _windowWidth;
        private int _windowHeight;

        private System.Numerics.Vector2 _scaleFactor = System.Numerics.Vector2.One;

        /// <summary>
        /// Constructs a new ImGuiController.
        /// </summary>
        public ImGuiController(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;

            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            var io = ImGui.GetIO();
            io.Fonts.AddFontDefault();

            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

            CreateDeviceResources();

            SetPerFrameImGuiData(1f / 60f);
            
            ImGui.NewFrame();
            _frameBegun = true;
        }

        public void WindowResized(int width, int height)
        {
            LOG.Warning("Resized");
            _windowWidth = width;
            _windowHeight = height;
        }

        public void DestroyDeviceObjects()
        {
            Dispose();
        }

        public void CreateDeviceResources()
        {
            unsafe
            {
                _vertexArray = glGenVertexArray();
                glBindVertexArray(_vertexArray);
                glBindVertexArray(0);

                _vertexBufferSize = 10000;
                _indexBufferSize = 2000;

                //Util.CreateVertexBuffer("ImGui", out _vertexBuffer);
                _vertexBuffer = glGenBuffer();
                //Util.CreateElementBuffer("ImGui", out _indexBuffer);
                _indexBuffer = glGenBuffer();

                glBindBuffer(GL_ARRAY_BUFFER, _vertexArray);
                glBufferData(GL_ARRAY_BUFFER, _vertexBufferSize, IntPtr.Zero, GL_DYNAMIC_DRAW);
                glBindBuffer(GL_ARRAY_BUFFER, _indexBuffer);
                glBufferData(GL_ARRAY_BUFFER, _indexBufferSize, IntPtr.Zero, GL_DYNAMIC_DRAW);
                glBindBuffer(GL_ARRAY_BUFFER, 0);

                // If using opengl 4.5 this could be a better way of doing it so that we are not modifying the bound buffers
                // GL.NamedBufferData(_vertexBuffer, _vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                // GL.NamedBufferData(_indexBuffer, _indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

                RecreateFontDeviceTexture();

                string VertexSource = @"#version 330 core

uniform mat4 projection_matrix;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;

out vec4 color;
out vec2 texCoord;

void main()
{
    gl_Position = projection_matrix * vec4(in_position, 0, 1);
    color = in_color;
    texCoord = in_texCoord;
}";
                string FragmentSource = @"#version 330 core

uniform sampler2D in_fontTexture;

in vec4 color;
in vec2 texCoord;

out vec4 outputColor;

void main()
{
    outputColor = color * texture(in_fontTexture, texCoord);
}";
                _shader = new Shader(VertexSource, FragmentSource);


                glBindVertexArray(_vertexArray);

                glBindBuffer(GL_ARRAY_BUFFER, _vertexBuffer);
                glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, _indexBuffer);

                int stride = Unsafe.SizeOf<ImDrawVert>();

                glEnableVertexAttribArray(0);
                glVertexAttribPointer(0, 2, GL_FLOAT, false, stride, null);
                glEnableVertexAttribArray(1);
                glVertexAttribPointer(1, 2, GL_FLOAT, false, stride, (void*)(sizeof(float) * 2));
                glEnableVertexAttribArray(2);
                glVertexAttribPointer(2, 4, GL_UNSIGNED_BYTE, true, stride, (void*)(sizeof(float) * 4));

                glBindVertexArray(0);
                glBindBuffer(GL_ARRAY_BUFFER, 0);
                // We don't need to unbind the element buffer as that is connected to the vertex array
                // And you should not touch the element buffer when there is no vertex array bound.

                //Util.CheckGLError("End of ImGui setup");
            }

        }
        private static ImFontPtr _font;

        /// <summary>
        /// Recreates the device texture used to render text.
        /// </summary>
        public void RecreateFontDeviceTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            _font = io.Fonts.AddFontFromFileTTF("_assets/Fonts/Inconsolata-Regular.ttf", 15.0f);


            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

            _fontTexture = new Texture(width, height, pixels);

            io.Fonts.SetTexID(new IntPtr(_fontTexture.TextureID));

            io.Fonts.ClearTexData();

        }

        /// <summary>
        /// Renders the ImGui draw list data.
        /// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
        /// or index data has increased beyond the capacity of the existing buffers.
        /// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
        /// </summary>
        public void Render()
        {
            if (_frameBegun)
            {
                _frameBegun = false;

                ImGui.Render();


                RenderImDrawData(ImGui.GetDrawData());

                //Util.CheckGLError("ImGui Controller");
            }
        }

        /// <summary>
        /// Updates ImGui input and IO configuration state.
        /// </summary>
        public void Update(/*GameWindow wnd,*/ float deltaSeconds)
        {

            //if (_frameBegun)
            //{
            //    ImGui.Render();
            //}

            SetPerFrameImGuiData(deltaSeconds);



            _frameBegun = true;
            //ImGui.NewFrame();
        }

        /// <summary>
        /// Sets per-frame data based on the associated window.
        /// This is called by Update(float).
        /// </summary>
        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(
                _windowWidth / _scaleFactor.X,
                _windowHeight / _scaleFactor.Y);
            io.DisplayFramebufferScale = _scaleFactor;
            io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
        }

        private void RenderImDrawData(ImDrawDataPtr draw_data)
        {
            unsafe
            {
                uint vertexOffsetInVertices = 0;
                uint indexOffsetInElements = 0;

                if (draw_data.CmdListsCount == 0)
                {
                    return;
                }

                uint totalVBSize = (uint)(draw_data.TotalVtxCount * Unsafe.SizeOf<ImDrawVert>());
                if (totalVBSize > _vertexBufferSize)
                {
                    int newSize = (int)Math.Max(_vertexBufferSize * 1.5f, totalVBSize);

                    glBindBuffer(GL_ARRAY_BUFFER, _vertexBuffer);
                    glBufferData(GL_ARRAY_BUFFER, newSize, IntPtr.Zero, GL_DYNAMIC_DRAW);
                    glBindBuffer(GL_ARRAY_BUFFER, 0);

                    _vertexBufferSize = newSize;

                    Console.WriteLine($"Resized vertex buffer to new size {_vertexBufferSize}");
                }

                uint totalIBSize = (uint)(draw_data.TotalIdxCount * sizeof(ushort));
                if (totalIBSize > _indexBufferSize)
                {
                    int newSize = (int)Math.Max(_indexBufferSize * 1.5f, totalIBSize);

                    glBindBuffer(GL_ARRAY_BUFFER, _indexBuffer);
                    glBufferData(GL_ARRAY_BUFFER, newSize, IntPtr.Zero, GL_DYNAMIC_DRAW);
                    glBindBuffer(GL_ARRAY_BUFFER, 0);

                    _indexBufferSize = newSize;

                    Console.WriteLine($"Resized index buffer to new size {_indexBufferSize}");
                }


                for (int i = 0; i < draw_data.CmdListsCount; i++)
                {
                    ImDrawListPtr cmd_list = draw_data.CmdListsRange[i];

                    glBindBuffer(GL_ARRAY_BUFFER, _vertexBuffer);
                    glBufferSubData(GL_ARRAY_BUFFER, (int)(vertexOffsetInVertices * Unsafe.SizeOf<ImDrawVert>()), cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmd_list.VtxBuffer.Data);

                    // Util.CheckGLError($"Data Vert {i}");

                    glBindBuffer(GL_ARRAY_BUFFER, _indexBuffer);
                    glBufferSubData(GL_ARRAY_BUFFER, (int)(indexOffsetInElements * sizeof(ushort)), cmd_list.IdxBuffer.Size * sizeof(ushort), cmd_list.IdxBuffer.Data);

                    // Util.CheckGLError($"Data Idx {i}");

                    vertexOffsetInVertices += (uint)cmd_list.VtxBuffer.Size;
                    indexOffsetInElements += (uint)cmd_list.IdxBuffer.Size;
                }
                glBindBuffer(GL_ARRAY_BUFFER, 0);

                // Setup orthographic projection matrix into our constant buffer
                ImGuiIOPtr io = ImGui.GetIO();

                mat4 mvp = glm.ortho(
                    0.0f,
                    io.DisplaySize.X,
                    io.DisplaySize.Y,
                    0.0f,
                    -1.0f,
                    1.0f);

                _shader.Bind();

                fixed (float* m = &mvp.to_array()[0])
                {
                    glUniformMatrix4fv(glGetUniformLocation(_shader.ProgramID, "projection_matrix"), 1, false, m);
                }

                glUniform1i(glGetUniformLocation(_shader.ProgramID, "in_fontTexture"), 0);
                //  Util.CheckGLError("Projection");

                glBindVertexArray(_vertexArray);
                // Util.CheckGLError("VAO");

                draw_data.ScaleClipRects(io.DisplayFramebufferScale);

                glEnable(GL_BLEND);
                glEnable(GL_SCISSOR_TEST);
                glBlendEquation(GL_FUNC_ADD);
                glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

                glDisable(GL_CULL_FACE);
                glDisable(GL_DEPTH_TEST);

                // Render command lists
                int vtx_offset = 0;
                int idx_offset = 0;

                for (int n = 0; n < draw_data.CmdListsCount; n++)
                {
                    ImDrawListPtr cmd_list = draw_data.CmdListsRange[n];
                    for (int cmd_i = 0; cmd_i < cmd_list.CmdBuffer.Size; cmd_i++)
                    {
                        ImDrawCmdPtr pcmd = cmd_list.CmdBuffer[cmd_i];
                        if (pcmd.UserCallback != IntPtr.Zero)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            glActiveTexture(GL_TEXTURE0);
                            glBindTexture(GL_TEXTURE_2D, (uint)pcmd.TextureId);
                            //  Util.CheckGLError("Texture");

                            // We do _windowHeight - (int)clip.W instead of (int)clip.Y because gl has flipped Y when it comes to these coordinates
                            var clip = pcmd.ClipRect;
                            glScissor((int)clip.X, _windowHeight - (int)clip.W, (int)(clip.Z - clip.X), (int)(clip.W - clip.Y));
                            //  Util.CheckGLError("Scissor");

                            glDrawElementsBaseVertex(GL_TRIANGLES, (int)pcmd.ElemCount, GL_UNSIGNED_SHORT, (void*/*IntPtr*/)(idx_offset * sizeof(ushort)), vtx_offset);
                            //  Util.CheckGLError("Draw");
                        }

                        idx_offset += (int)pcmd.ElemCount;
                    }
                    vtx_offset += cmd_list.VtxBuffer.Size;
                }

                glBindVertexArray(0);
                glDisable(GL_BLEND);
                glDisable(GL_SCISSOR_TEST);
            }

        }

        /// <summary>
        /// Frees all graphics resources used by the renderer.
        /// </summary>
        public void Dispose()
        {
            _fontTexture.Dispose();
            _shader.Dispose();
        }
    }
}
