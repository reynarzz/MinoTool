using GlmNet;
using System;
using System.Runtime.InteropServices;
using static OpenGL.GL;

namespace MinoTool
{
    public struct MeshBounds
    {
        public vec3 Min;
        public vec3 Max;
    }

    public unsafe class Mesh : IDisposable
    {
        public uint[] Indices { get; private set; }

        private uint _vArray;
        public MeshBounds Bounds { get; private set; }

        public int DrawMode { get; private set; }
        private uint _vBuffer;
        private float[] _vertices;

        public Mesh() { }
        
        public void UpdateMeshData(float[] vertices, uint[] indices, int drawMode = GL_TRIANGLES, MeshBounds bounds = default)
        {
            Indices = indices;
            Bounds = bounds;
            DrawMode = drawMode;

            if(_vArray <= 0)
            {
                _vArray = glGenVertexArray();
                _vBuffer = glGenBuffer();
            }

            glBindVertexArray(_vArray);
            glBindBuffer(GL_ARRAY_BUFFER, _vBuffer);

            fixed (float* vData = &vertices[0])
            {
                //if (_vertices != null)
                //{
                //    glBufferSubData(GL_ARRAY_BUFFER, 0, vertices.Length * sizeof(float), vData);

                //}
                //else
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vData, GL_STATIC_DRAW);
                }

                _vertices = vertices;
            }

            glBindVertexArray(0);
        }

        public Mesh(float[] vertices, uint[] indices, int drawMode = GL_TRIANGLES, MeshBounds bounds = default)
        {
            _vertices = vertices;
            Indices = indices;
            Bounds = bounds;
            DrawMode = drawMode;

            _vArray = glGenVertexArray();
            glBindVertexArray(_vArray);


            _vBuffer = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, _vBuffer);

            //_alloc = GCHandle.Alloc(_vertices, GCHandleType.Pinned);

            //var add = _alloc.AddrOfPinnedObject();
            fixed (float* vData = &_vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vData, GL_STATIC_DRAW);
            }

            glBindVertexArray(0);
        }

        public void Bind()
        {
            glBindBuffer(GL_ARRAY_BUFFER, _vBuffer);
            glBindVertexArray(_vArray);
            //glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, _indexBuffer);

            //fixed (float* vData = &_vertices[0])
            //{
            //    glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * _vertices.Length, vData);
            //}
        }

        internal void Unbind()
        {
            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindVertexArray(0);
        }

        internal void Destroy()
        {
            //if (_alloc.IsAllocated)
            //_alloc.Free();
            glDeleteVertexArray(_vArray);
            glDeleteBuffer(_vBuffer);
            _vertices = null;
            Indices = null;
        }
        
        public void Dispose()
        {
            Destroy();
        }
    }
}