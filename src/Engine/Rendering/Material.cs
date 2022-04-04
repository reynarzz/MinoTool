using GlmNet;
using static OpenGL.GL;

namespace MinoTool
{
    internal struct MeshLayoutInfo
    {
        public int VertsComponentsCount { get; set; }
        public int UVComponentsCount { get; set; }
    }

    // A Material, makes sending data to shaders easier.
    public unsafe class Material : Entity
    {

        private const int COORDS_PER_VERTEX = 3;
        private const int COORDS_PER_UV = 2;
        private const int COORDS_PER_NORMAL = 3;

        private const int FLOAT_BYTES = 4;

        private Shader _shader;
        public Texture[] Textures { get; set; }

        private Color _col = Color.white;
        /// <summary>Color of this material.</summary>
        internal uint ProgramID => _shader.ProgramID;

        public Color color
        {
            get
            {
                return _col;
            }
            set
            {
                _col = value;
                Set("_Color", new vec4(_col.r, _col.g, _col.b, _col.a));
            }
        }

       

        public Material(Shader shader)
        {
            _shader = shader;
        }

        public Material(Material mat)
        {
            color = mat.color;
            _shader = new Shader(mat._shader);
        }

        public void Set(string uniformName, vec4 value)
        {
            var location = glGetUniformLocation(_shader.ProgramID, uniformName);
            glUniform4f(location, value.x, value.y, value.z, value.w);
        }

        public void Set(string uniformName, Color value)
        {
            var location = glGetUniformLocation(_shader.ProgramID, uniformName);
            glUniform4f(location, value.r, value.g, value.b, value.a);
        }
        public void Set(string uniformName, int value)
        {
            var location = glGetUniformLocation(_shader.ProgramID, uniformName);
            glUniform1i(location, value);
        }

        

        internal void Bind(mat4 modelM, mat4 viewM, mat4 projM)
        {
            Bind(modelM, viewM, projM, new MeshLayoutInfo() { VertsComponentsCount = COORDS_PER_VERTEX, UVComponentsCount = COORDS_PER_UV });
        }

        internal void Bind(mat4 modelM, mat4 viewM, mat4 projM, MeshLayoutInfo info)
        {
            _shader.Bind();

            BindTextures();

            var mvp = projM * viewM * modelM;

            // MVP matrix
            _shader.Set("_MVP", mvp);
            _shader.Set("_VP", projM * viewM);

            // Shape color
            Set("_Color", new vec4(color.r, color.g, color.b, color.a));

            _shader.SetDataLayout(info);
        }

        private void BindTextures()
        {
            if (Textures != null)
            {
                for (int i = 0; i < Textures.Length; i++)
                {
                    var tex = Textures[i];
                    tex.Bind(i);

                    var loc = glGetUniformLocation(_shader.ProgramID, $"_tex{i}");

                    if (loc >= 0)
                    {
                        glUniform1i(loc, i);
                    }
                    else
                    {
                        LOG.Error($"_tex{i} doesn't exist");
                    }
                }
            }
        }

       
        internal void Unbind()
        {
            UnbindTextures();
            // Not use current shader.
            glUseProgram(0);
        }

        private void UnbindTextures()
        {
            if (Textures != null)
            {
                for (int i = 0; i < Textures.Length; i++)
                {
                    Textures[i].Unbind();
                }
            }
        }

        internal override void _OnDestroyInternal()
        {
            base._OnDestroyInternal();

            Unbind();

            Textures = null;
        }

        internal void Dispose()
        {
            _OnDestroyInternal();
            Textures = null;
            glDeleteProgram(_shader.ProgramID);
        }
    }
}
