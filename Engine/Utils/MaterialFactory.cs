using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public class MaterialFactory : Singleton<MaterialFactory>
    {

        private string _unlitVert = @"#version 330 core

layout (location = 0) in vec3 _VPOS;
layout (location = 1) in vec2 _UV;

uniform mat4 _MVP;
out vec2 _uv;

void main()
{
_uv = _UV;
    gl_Position = _MVP * vec4(_VPOS, 1.0);
}";

        private string _unlitFrag = @"#version 330 core

out vec4 fragColor;

uniform vec4 _Color;
uniform sampler2D _tex0;
uniform sampler2D _tex1;
uniform sampler2D _tex2;
in vec2 _uv;

void main()
{

    fragColor = _Color;
}";

        private string _vert = @"#version 330 core

layout (location = 0) in vec3 _VPOS;
layout (location = 1) in vec2 _UV;

uniform mat4 _MVP;
out vec2 _uv;

void main()
{
_uv = _UV;
    gl_Position = _MVP * vec4(_VPOS, 1.0);
}";

        private string _frag = @"#version 330 core

out vec4 fragColor;

uniform vec4 _Color;
uniform sampler2D _tex0;
uniform sampler2D _tex1;
uniform sampler2D _tex2;
in vec2 _uv;

void main()
{
vec4 c = texture2D(_tex0, _uv);

 if(c.x == 0 && c.y == 0 && c.z == 0)
{
 c = vec4(1.);
}
    fragColor = c * _Color;
}";

        internal Shader GetDefaultShader()
        {
            return new Shader(_vert, _frag);
        }

        private Material _defaultMat;

        public Material GetDefaultMaterial()
        {
            _defaultMat = new Material(GetDefaultShader());

            return _defaultMat;
        }

        public Material GetUnlitMaterial()
        {
            var mat = new Material(new Shader(_unlitVert, _unlitFrag));

            return mat;
        }


        private string _uivert = @"#version 330 core

layout (location = 0) in vec3 _VPOS;
layout (location = 1) in vec2 _UV;

uniform mat4 _MVP;
out vec2 _uv;

void main()
{
_uv = _UV;
    gl_Position = _MVP * vec4(_VPOS, 1.0);
}";

        private string _uifrag = @"#version 330 core

out vec4 fragColor;

uniform vec4 _Color;
uniform sampler2D _tex0;
uniform sampler2D _tex1;
uniform sampler2D _tex2;
in vec2 _uv;

void main()
{
vec4 c = texture2D(_tex0, _uv);

 if(c.x == 0 && c.y == 0 && c.z == 0)
{
 c = vec4(1.);
}
    fragColor = c * _Color;
}";



        public Shader GetUIShader()
        {
            return new Shader(_uivert, _uifrag);
        }


        private string _scrVert = @"#version 330 core

layout (location = 0) in vec3 _VPOS;
layout (location = 1) in vec2 _UV;

uniform mat4 _MVP;
out vec2 _uv;

void main()
{
_uv = _UV;
    gl_Position = vec4(_VPOS, 1.0);
}";

        private string _scrFrag = @"#version 330 core

out vec4 fragColor;

uniform vec4 _Color;
uniform sampler2D _tex0;
in vec2 _uv;

void main()
{
vec4 c = texture2D(_tex0, _uv);

    fragColor = c/* + ((vec4(0.0274, 0.0705, 0.1215, 1) + vec4(0.8980, 0.1294, 0.1921, 1)) / 2)*/;
}";

        internal Shader GetScreenSizeQuadShader()
        {
            return new Shader(_scrVert, _scrFrag);
        }


        private string _vpVert = @"#version 330 core

layout (location = 0) in vec3 _VPOS;
layout (location = 1) in vec2 _UV;

uniform mat4 _VP;
out vec2 _uv;

void main()
{
_uv = _UV;
    gl_Position = _VP * vec4(_VPOS, 1.0);
}";

        private string _vpFrag = @"#version 330 core

out vec4 fragColor;

uniform vec4 _Color;
uniform sampler2D _tex0;
uniform sampler2D _tex1;
uniform sampler2D _tex2;
in vec2 _uv;

void main()
{
    fragColor = vec4(1.0,1.0,1.0,1.0);
}";

        internal Material GetVPMaterial()
        {
            return new Material(new Shader(_vpVert, _vpFrag));
        }
    }
}