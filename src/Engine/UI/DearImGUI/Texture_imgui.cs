using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;

namespace Dear_ImGui_Sample
{
    public class Texture_ImGui : IDisposable
    {
        public uint TextureID { get; set; }

        public Texture_ImGui(int width, int height, IntPtr data, bool generateMipmaps = false, bool srgb = false)
        {
            var Width = width;
            var Height = height;
            //var InternalFormat = srgb ? Srgb8Alpha8 : SizedInternalFormat.Rgba8;
            var MipmapLevels = generateMipmaps == false ? 1 : (int)Math.Floor(Math.Log(Math.Max(Width, Height), 2));

            TextureID = glGenTexture();
            glBindTexture(GL_TEXTURE_2D, TextureID);

            //glTexStorage2D(GL_TEXTURE_2D, MipmapLevels, InternalFormat, Width, Height);
            //glTexImage2D(GL_TEXTURE_2D, MipmapLevels, GL_RGBA8, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE)
            //glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, Width, Height, GL_BGRA, GL_UNSIGNED_BYTE, data);

            //glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);

            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data);
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, Width, Height, 0, GL_BGRA, GL_UNSIGNED_BYTE, data);
            //GL.TexImage2D(TextureTarget.Texture2D, i, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data);

            //glTexImage2D(target, i, internalformat, width, height, 0, format, type, NULL);
            width = Math.Max(1, (width / 2));
            height = Math.Max(1, (height / 2));

            // GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, PixelFormat.Bgra, PixelType.UnsignedByte, data);
            glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, Width, Height, GL_BGRA, GL_UNSIGNED_BYTE, data);

            if (generateMipmaps)
                glGenerateMipmap(GL_TEXTURE_2D);


            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAX_LEVEL, MipmapLevels - 1);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
            //SetWrap(GL_TEXTURE_WRAP_S, GL_REPEAT);
            //SetWrap(GL_TEXTURE_WRAP_T, GL_REPEAT);
            glBindTexture(GL_TEXTURE_2D, 0);
        }

        public void SetWrap(int coord, int mode)
        {
            glBindTexture(GL_TEXTURE_2D, TextureID);
            glTexParameteri(GL_TEXTURE_2D, coord, mode);
            glBindTexture(GL_TEXTURE_2D, 0);
        }

        public void Dispose()
        {
            glDeleteTexture(TextureID);
        }
    }
}
