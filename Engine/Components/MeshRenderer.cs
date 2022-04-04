using GlmNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public sealed class MeshRenderer : Renderer
    {
        private Mesh _mesh;

        internal MeshRenderer(Mesh mesh, Material material) 
        {
            _mesh = mesh;
            this.material = material;
        }

        public MeshRenderer SetMesh(Mesh mesh)
        {
            //_mesh.Destroy();
            _mesh = mesh;

            return this;
        }

        public override Mesh GetMesh()
        {
            return _mesh;
        }

        internal override void Bind(mat4 viewM, mat4 projM)
        {
            _mesh.Bind();
            base.Bind(viewM, projM);
        }

        internal override void Bind(mat4 model, mat4 viewM, mat4 projM)
        {
            _mesh.Bind();
            base.Bind(model, viewM, projM);
        }

        internal void Dispose()
        {
            Unbind();
            material.Dispose();
        }
    }
}