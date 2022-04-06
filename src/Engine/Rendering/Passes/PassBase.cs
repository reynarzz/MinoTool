using System;
using System.Text;
using System.Collections.Generic;
using static OpenGL.GL;

namespace MinoTool
{
    internal abstract class PassBase
    {
        internal abstract void DrawPass(Camera camera, List<Renderer> renderers);

        internal virtual void Clear()
        {
            glClearColor(0.2f, 0.2f, 0.2f, 1.0f);
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            glEnable(GL_DEPTH_TEST);
        }

        internal abstract void UpdateViewport(int w, int h);
    }
}