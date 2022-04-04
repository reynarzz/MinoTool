using System;
using System.Collections.Generic;
using System.Text;
using GlmNet;

namespace MinoTool
{
    public class LineRenderer : Renderer
    {
        private Mesh _mesh;
        private List<float> _points;
        private const int vertexDataCount = 5; //vertices, uv
        public int Count => _points.Count / vertexDataCount;
        public LineRenderer()
        {
            _mesh = new Mesh();
            _points = new List<float>();
            base.material = MaterialFactory.Instance.GetVPMaterial();
        }

        internal override void Bind(mat4 modelM, mat4 viewM, mat4 projM)
        {
            _mesh.Bind();
            base.Bind(modelM, viewM, projM);
        }

        internal override void Bind(mat4 viewM, mat4 projM)
        {
            _mesh.Bind();
            base.Bind(viewM, projM);
        }

        public void AddPoint(vec3 pos)
        {
            // Vertex
            _points.Add(pos.x);
            _points.Add(pos.y);
            _points.Add(pos.z);

            // UV
            _points.Add(0);
            _points.Add(0);

            ////normals
            //_points.Add(0);
            //_points.Add(0);
            //_points.Add(0);

            var indices = new uint[_points.Count / vertexDataCount * 2 - 1];

            uint prevIdx = 0;
            for (int i = 0; i < indices.Length; i += 2)
            {
                indices[i] = prevIdx;
                if (indices.Length > i + 1)
                    indices[i + 1] = ++prevIdx;
            }

            _mesh.UpdateMeshData(_points.ToArray(), indices, OpenGL.GL.GL_LINES);
        }

        public void UpdatePoint(int index, vec3 pos)
        {
            _points[index * vertexDataCount + 0] = pos.x;
            _points[index * vertexDataCount + 1] = pos.y;
            _points[index * vertexDataCount + 2] = pos.z;

            var indices = new uint[_points.Count / vertexDataCount * 2 - 1];

            uint prevIdx = 0;
            for (int i = 0; i < indices.Length; i += 2)
            {
                indices[i] = prevIdx;
                if (indices.Length > i + 1)
                    indices[i + 1] = ++prevIdx;
            }

            _mesh.UpdateMeshData(_points.ToArray(), indices, OpenGL.GL.GL_LINES);
        }

        public vec3 GetPointPosition(int index)
        {
            var startIndex = index * vertexDataCount;
            return new vec3(_points[startIndex], _points[startIndex + 1], _points[startIndex + 2]);
        }

        public void RemovePoint(int index)
        {
            _points.RemoveAt(index * vertexDataCount + 0);
            _points.RemoveAt(index * vertexDataCount + 1);
            _points.RemoveAt(index * vertexDataCount + 2);
        }

        public override Mesh GetMesh()
        {
            return _mesh;
        }
    }
}
