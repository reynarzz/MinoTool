using System;
using System.Collections.Generic;
using System.Text;
using static OpenGL.GL;

namespace MinoTool
{
    internal unsafe class FrameBuffer
    {
        private uint _bufferID;
        private Texture _colorTex;
        private uint _depthID;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public uint ColotTex => _colorTex.TextureID;

        public FrameBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            _bufferID = glGenFramebuffer();
            glBindFramebuffer(GL_FRAMEBUFFER, _bufferID);

            _colorTex = new Texture(width, height);

            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, _colorTex.TextureID, 0);

            _depthID = glGenTexture();
            glBindTexture(GL_TEXTURE_2D, _depthID);
            glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH24_STENCIL8, width, height, 0, GL_DEPTH_STENCIL, GL_UNSIGNED_INT_24_8, NULL);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

            glBindTexture(GL_TEXTURE_2D, 0);

            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_STENCIL_ATTACHMENT, GL_TEXTURE_2D, _depthID, 0);

            if (glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
                LOG.Error("ERROR::FRAMEBUFFER:: Framebuffer is not complete!");

            glBindFramebuffer(0);
        }

        internal void Bind()
        {
            _colorTex.Bind(0);
            glBindFramebuffer(GL_FRAMEBUFFER, _bufferID);
        }

        internal void Unbind()
        {
            _colorTex.Unbind();
            glBindFramebuffer(0);
        }

        internal void DeleteFrameBuffer()
        {
            Unbind();

            glDeleteFramebuffer(_bufferID);
            glDeleteTexture(_colorTex.TextureID);
            glDeleteTexture(_depthID);
        }
    }
}
