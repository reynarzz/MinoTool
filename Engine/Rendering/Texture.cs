using System;
using System.Collections.Generic;
using System.Text;
using static OpenGL.GL;
using StbSharp;
using System.IO;

namespace MinoTool
{
    public unsafe class Texture : Entity
    {
        private uint _textureID;

        private int _width;
        private int _height;
        internal uint TextureID => _textureID;

        public int Width => _width;
        public int Height => _height;
        private int _bindedTexUnit;

        public Texture(string path)
        {
            SetImage(path, GL_UNSIGNED_BYTE);
        }

        //internal Texture(string path, int type = GL_UNSIGNED_BYTE)
        //{
        //    SetImage(path, type);
        //}

        internal Texture(int width, int height)
        {
            GenEmptyTex(width, height);
        }

        internal Texture(int width, int height, IntPtr pixels)
        {
            GenEmptyTex(width, height);

            glActiveTexture(GL_TEXTURE0);
            Bind(0);
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, pixels);
            Unbind();
        }

        private void GenEmptyTex(int width, int height)
        {
            _width = width;
            _height = height;

            _textureID = glGenTexture();

            //glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D, _textureID);

            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, NULL);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR); //GL_LINEAR
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

            glBindTexture(GL_TEXTURE_2D, 0);
        }

        internal void SetImage(string path, int type)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);

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

        internal void Dispose()
        {
            glDeleteTexture(_textureID);
        }
    }
}
