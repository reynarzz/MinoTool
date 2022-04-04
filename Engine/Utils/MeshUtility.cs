using GlmNet;
using ObjLoader.Loader.Loaders;
using System.Collections.Generic;
using System.IO;

namespace MinoTool
{
    public static class MeshUtility
    {
        static MeshUtility()
        {
        }

        public static Mesh LoadMesh(string path, bool useAssetsRoot = true)
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();

            var assetsRoot = useAssetsRoot ? FilesRepository._root : null;
            var fileStream = new FileStream(assetsRoot + path, FileMode.Open);
            var result = objLoader.Load(fileStream);

            var verts = new List<float>();
            var index = new List<uint>();

            int indexCount = 0;
            var bounds = new MeshBounds();

            for (int i = 0; i < result.Groups.Count; i++)
            {
                var faces = result.Groups[i].Faces;

                for (int j = 0; j < faces.Count; j++)
                {
                    for (int k = 0; k < faces[j].Count; k++)
                    {
                        var face = faces[j][k];

                        index.Add((uint)indexCount++);

                        var vert = result.Vertices[face.VertexIndex - 1];

                        verts.Add(vert.X);
                        verts.Add(vert.Y);
                        verts.Add(vert.Z);

                        if (vert.X < bounds.Min.x)
                        {
                            bounds.Min.x = vert.X;
                        }

                        if (vert.X > bounds.Max.x)
                        {
                            bounds.Max.x = vert.X;
                        }


                        if (vert.Y < bounds.Min.y)
                        {
                            bounds.Min.y = vert.Y;
                        }

                        if (vert.Y > bounds.Max.y)
                        {
                            bounds.Max.y = vert.Y;
                        }

                        if (vert.Z < bounds.Min.z)
                        {
                            bounds.Min.z = vert.Z;
                        }

                        if (vert.Z > bounds.Max.z)
                        {
                            bounds.Max.z = vert.Z;
                        }

                        // if the obj is not uv wraped this will throw an exception.

                        if (result.Textures.Count > face.TextureIndex)
                        {
                            verts.Add(result.Textures[face.TextureIndex - 1].X);
                            verts.Add(result.Textures[face.TextureIndex - 1].Y);
                        }
                        else
                        {
                            verts.Add(0);
                            verts.Add(0);
                        }
                    }
                }
            }

            fileStream.Close();
            var m = new Mesh();
            m.UpdateMeshData(verts.ToArray(), index.ToArray(), OpenGL.GL.GL_TRIANGLES, bounds);

            return m;
            // return new Mesh(verts.ToArray(), index.ToArray(), OpenGL.GL.GL_TRIANGLES, bounds);
        }

        public static Mesh GetCubeLinesMesh(MeshBounds pivot, float scale)
        {
            var vertices = new float[]
            {   //Verts                                                               //uv's
                (pivot.Min.x) * scale, (pivot.Min.y) * scale, (pivot.Min.z) * scale,   0, 0,
                (pivot.Max.x) * scale, (pivot.Min.y) * scale, (pivot.Min.z) * scale,   0, 0,
                (pivot.Max.x) * scale, (pivot.Min.y) * scale, (pivot.Max.z) * scale,   0, 0,
                (pivot.Min.x) * scale, (pivot.Min.y) * scale, (pivot.Max.z) * scale,   0, 0,

                (pivot.Min.x) * scale, (pivot.Max.y) * scale, (pivot.Min.z) * scale,   0, 0,
                (pivot.Max.x) * scale, (pivot.Max.y) * scale, (pivot.Min.z) * scale,   0, 0,
                (pivot.Max.x) * scale, (pivot.Max.y) * scale, (pivot.Max.z) * scale,   0, 0,
                (pivot.Min.x) * scale, (pivot.Max.y) * scale, (pivot.Max.z) * scale,   0, 0,
            };

            var indices = new uint[]
            {
                0, 1, 1, 2, 2, 3, 3, 0,
                4, 5, 5, 6, 6, 7, 7, 4,
                0, 4, 1, 5, 2, 6, 3, 7
            };

            return new Mesh(vertices, indices, OpenGL.GL.GL_LINES);
        }

        /*public static Mesh GetCubeLinesMesh(MeshBounds pivot, float scale)
        {
            var vertices = new float[]
            {
                (-1 + pivot.Min.x) * scale,( -1 + pivot.Min.y) *scale,(-1 + pivot.Min.z) *scale,   0, 0,
               (1 + pivot.Max.x) *scale,(-1+ pivot.Min.y) *scale,(-1+ pivot.Min.z) *scale,   0, 0,
              (1 + pivot.Max.x) *scale,( -1+ pivot.Min.y) *scale, (1 + pivot.Max.z) *scale,   0, 0,
               (-1+ pivot.Min.x) *scale,( -1+ pivot.Min.y) *scale, (1 + pivot.Max.z) *scale,   0, 0,

              (-1+ pivot.Min.x) *scale, (1 + pivot.Max.y) *scale, (-1+ pivot.Min.z) *scale,   0, 0,
               (1 + pivot.Max.x) *scale, (1 + pivot.Max.y) *scale, (-1+ pivot.Min.z) *scale,   0, 0,
               (1 + pivot.Max.x) *scale, (1 + pivot.Max.y) *scale, (1 + pivot.Max.z) *scale,   0, 0,
              (-1+ pivot.Min.x) *scale, (1 + pivot.Max.y) *scale, (1 + pivot.Max.z) *scale,   0, 0,
            };

            var indices = new uint[]
            {
                0, 1, 1, 2, 2, 3, 3, 0,
                4, 5, 5, 6, 6, 7, 7, 4,
                0, 4, 1, 5, 2, 6, 3, 7
            };

            return new Mesh(vertices, indices, OpenGL.GL.GL_LINES);
        }*/
        public static Mesh GetTriangle()
        {
            var vertices = new float[]
            {
                -1f, -1f, 0f,
                0.0f, 1f,0f,
                1f, -1f,0f,
            };

            var indices = new uint[] { 0, 1, 2 };

            return new Mesh(vertices, indices);
        }

        public static Mesh GetQuadMesh()
        {
            var vertices = new float[]
            {              //uv
                 -1f, -1f, 0,  0, 0,
                 1f,  -1f, 0,  1, 0,
                 1f,  1f,  0,  1, 1,
                 -1f, 1f,  0,  0, 1,
            };

            var indices = new uint[] { 0, 1, 2, 2, 3, 0 };

            return new Mesh(vertices, indices);
        }

        public static Mesh GetCube()
        {
            //For testing
            return GetQuadMesh();
        }

        public static Mesh GetHexagon()
        {
            // A hexagon is a circle with 6 vertices.
            return GetCircle(6);
        }

        public static Mesh GetCircle(int verticesCount = 80)
        {
            // 'x' and 'y' components
            var components = 3;

            var vertices = new float[verticesCount * components];
            var indices = new uint[verticesCount];

            var degree = 360f / verticesCount;

            // current vector component ('x' or 'y')
            var component = 0;

            for (int i = 0; i < verticesCount; i++)
            {
                var rad = glm.radians(degree * i);

                // sets 'x' vertex position.
                vertices[component++] = glm.cos(rad);

                // sets 'y' vertex position.
                vertices[component++] = glm.sin(rad);

                vertices[component++] = 0;
            }

            var index = 0;

            for (int i = 0; i < indices.Length; i += 2)
            {
                indices[index] = 0;
                indices[index++] = (uint)i;

                if (index < indices.Length)
                    indices[index++] = (uint)i + 1;
            }

            return new Mesh(vertices, indices, OpenGL.GL.GL_TRIANGLE_FAN);
        }
    }
}