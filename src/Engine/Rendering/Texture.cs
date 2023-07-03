using System;
using System.Collections.Generic;
using System.Text;
using static OpenGL.GL;
using StbSharp;
using System.IO;

namespace MinoTool
{
    public enum TextureFilterMode
    {
        Linear,
        Nearest
    }

    public enum TextureFormat
    {
        RGBA = GL_RGBA,
        BGR = GL_BGR,
        BGRA = GL_BGRA
    }

    public unsafe class Texture : Entity
    {
        private uint _textureID;

        private int _width;
        private int _height;
        public uint TextureID => _textureID;

        public int Width => _width;
        public int Height => _height;
        private int _bindedTexUnit;

        public Texture()
        {

        }

        public Texture(string path, bool flipVertically = true)
        {
            SetImage(path, GL_UNSIGNED_BYTE, flipVertically);
        }

        //internal Texture(string path, int type = GL_UNSIGNED_BYTE)
        //{
        //    SetImage(path, type);
        //}

        internal Texture(int width, int height)
        {
            GenEmptyTex(width, height);
        }

        public Texture(int width, int height, IntPtr pixels, TextureFilterMode filter = TextureFilterMode.Linear, int internalFormat = GL_RGBA8, int format = GL_RGBA)
        {
            GenEmptyTex(width, height, filter);
            
            glActiveTexture(GL_TEXTURE0);
            Bind(0);
            glTexImage2D(GL_TEXTURE_2D, 0, internalFormat, width, height, 0, format, GL_UNSIGNED_BYTE, pixels);
            Unbind();
        }

        public void GenEmptyTex(int width, int height, TextureFilterMode filter = TextureFilterMode.Linear)
        {
            _width = width;
            _height = height;

            _textureID = glGenTexture();

            var mode = GL_LINEAR;

            if (filter == TextureFilterMode.Nearest)
            {
                mode = GL_NEAREST;
            }

            //glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, _textureID);

            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, NULL);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, mode); //GL_LINEAR
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, mode);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

            glBindTexture(GL_TEXTURE_2D, 0);
        }

        internal void SetImage(string path, int type, bool flipVertically = true)
        {
            if (flipVertically)
            {
                StbImage.stbi_set_flip_vertically_on_load(1);
            }

            ImageReader loader = new ImageReader();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Image image = loader.Read(stream, StbImage.STBI_rgb_alpha);

                GenEmptyTex(image.Width, image.Height);

                glBindTexture(GL_TEXTURE_2D, _textureID);

                fixed (void* point = &image.Data[0])
                {
                    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, image.Width, image.Height, 0, GL_RGBA, type, point);
                }

                LOG.Success($"Load: {Path.GetFileNameWithoutExtension(path)}: ({image.Width}, {image.Height})");
                glBindTexture(GL_TEXTURE_2D, 0);
            }
        }

        internal void Bind(int textureUnit)
        {
            if (textureUnit < 0)
            {
                LOG.Error("TextureUnit less that 0: " + textureUnit);
                _bindedTexUnit = 0;
                return;
            }

            _bindedTexUnit = textureUnit;
            glActiveTexture(GL_TEXTURE0 + _bindedTexUnit);
            glBindTexture(GL_TEXTURE_2D, _textureID);
        }

        internal void Unbind()
        {
            glActiveTexture(GL_TEXTURE0 + _bindedTexUnit);
            glBindTexture(GL_TEXTURE_2D, 0);

            _bindedTexUnit = 0;
        }

        public void Dispose()
        {
            glDeleteTexture(_textureID);
        }
    }
}
