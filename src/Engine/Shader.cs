using GlmNet;
using System;
using static OpenGL.GL;

namespace MinoTool
{
    public unsafe class Shader
    {
        public uint ProgramID { get; private set; }

        private readonly string _vertexCode;
        private readonly string _fragmentCode;

        public Shader(string vertexCode, string fragmentCode)
        {
            _vertexCode = vertexCode;
            _fragmentCode = fragmentCode;

            var vertexShaderID = CreateShader(vertexCode, GL_VERTEX_SHADER);
            var fragmentShaderID = CreateShader(fragmentCode, GL_FRAGMENT_SHADER);

            LinkProgram(vertexShaderID, fragmentShaderID);
        }

        public Shader(Shader shader) : this(shader._vertexCode.ToString(), shader._fragmentCode.ToString())
        {

        }

        // Get a compilated shader ID.
        private uint CreateShader(string source, int shaderType)
        {
            var shader = glCreateShader(shaderType);
            glShaderSource(shader, source);

            var log = glGetShaderInfoLog(shader);

            int success = 0;
            glGetShaderiv(shader, GL_COMPILE_STATUS, &success);

            if (success == 0)
            {
                //int maxLength = 0;
                //glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &maxLength);

                //int logSize = 0;
                //glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &logSize);

                //// The maxLength includes the NULL character
                ////glGetShaderInfoLog(shader, maxLength, &maxLength, &errorLog[0]);

                //LOG.Error("TODO:FIX shaders error handling");
            }

            return shader;
        }

        // Attach the shaders to the shader program.
        private void LinkProgram(uint vertexShader, uint fragmentShader)
        {
            ProgramID = glCreateProgram();
            glAttachShader(ProgramID, vertexShader);
            glAttachShader(ProgramID, fragmentShader);

            glDeleteShader(vertexShader);
            glDeleteShader(fragmentShader);

            glLinkProgram(ProgramID);
        }

        // Use the shader.
        public void Bind()
        {
            glUseProgram(ProgramID);
        }

        // Send this back to the Material class
        internal void SetDataLayout(MeshLayoutInfo info)
        {
            var stride = sizeof(float) * (info.VertsComponentsCount + info.UVComponentsCount /*+ COORDS_PER_NORMAL*/);
            // get actual vPos attrib location.
            var vertexLocation = glGetAttribLocation(ProgramID, "_VPOS");
            var uvLocation = glGetAttribLocation(ProgramID, "_UV");

            // Vpos data format.
            glVertexAttribPointer((uint)vertexLocation, info.VertsComponentsCount, GL_FLOAT, false, stride, NULL);
            glVertexAttribPointer((uint)uvLocation, info.UVComponentsCount, GL_FLOAT, true, stride, (void*)(sizeof(float) * 3));

            // normal
            //glVertexAttribPointer((uint)uvLocation, COORDS_PER_UV, GL_FLOAT, true, stride, (void*)(sizeof(float) * 5));

            // Enable attributes.
            glEnableVertexAttribArray((uint)vertexLocation);
            glEnableVertexAttribArray((uint)uvLocation);
        }

        public void Set(string uniformName, mat4 value)
        {
            var location = glGetUniformLocation(ProgramID, uniformName);

            fixed (float* m = &value.to_array()[0])
            {
                glUniformMatrix4fv(location, 1, false, m);
            }
        }

        public void Dispose()
        {
            glDeleteShader(ProgramID);
        }
    }
}
