using GlmNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace MinoTool
{
    public static class Utils
    {
        /// <summary>Distance from the center of the world.</summary>
        public const float Magnitude = 2.0f;

        ////public static EntityObject GetShapeEntity()
        ////{
        ////    var triangle = BasicMeshFactory.Instance.GetTriangle();

        ////    var vertexCode = File.ReadAllText("Shaders/VertexShader.glsl");
        ////    var fragmentCode = File.ReadAllText("Shaders/FragmentShader.glsl");

        ////    return new EntityObject(triangle, new Material(new Shader(vertexCode, fragmentCode)));
        ////}

        /// <summary>Returns the fractional part of n.</summary>
        public static float Fract(this float n)
        {
            return n - MathF.Floor(n);
        }

        public static vec3 Lerp(this vec3 a, vec3 to, float t)
        {
            t = Clamp01(t);

            return new vec3(a.x + (to.x - a.x) * t, a.y + (to.y - a.y) * t, a.z + (to.z - a.z) * t);
        }

        public static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }
            if (value > 1f)
            {
                return 1f;
            }
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        public static bool IsAvalidJson(this string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException)
            {
                Console.WriteLine("Invalid json, please check.");
                return false;
            }
        }

        public static Vector3 ToV3(this vec3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }

        public static vec3 Tov3(this Vector3 vector)
        {
            return new vec3(vector.x, vector.y, vector.z);
        }

        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        private readonly static object _lock = new object();

        public static T cloneObject<T>(T original, List<string> propertyExcludeList)
        {
            LOG.Error("TODO: Entity cloning doesn't work correctly yet, please fix it.");

            try
            {
                Monitor.Enter(_lock);
                var copy = Activator.CreateInstance(typeof(T));

                PropertyInfo[] piList = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (PropertyInfo pi in piList)
                {
                    if (!propertyExcludeList.Contains(pi.Name))
                    {
                        if (pi.GetValue(copy, null) != pi.GetValue(original, null))
                        {
                            if (pi.CanWrite)
                            {
                                pi.SetValue(copy, pi.GetValue(original, null), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
                            }
                            //Log.L("Can't write: " + pi.Name + ", " + pi.ReflectedType.Name);
                        }
                    }
                }

                FieldInfo[] fiList = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo fi in fiList)
                {
                    if (!propertyExcludeList.Contains(fi.Name))
                    {
                        if (fi.GetValue(copy) != fi.GetValue(original))
                        {
                            fi.SetValue(copy, fi.GetValue(original), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                            //Log.L("field: " + fi.Name + ", " + fi.ReflectedType.Name);
                        }
                    }
                }


                return (T)copy;
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        public static float RandomRange(float minimum, float maximum)
        {
            var randomizer = new Random();

            return (float)(minimum + (float)randomizer.NextDouble() * (maximum - minimum));
        }

        public static int RandomRange(int minimum, int maximum)
        {
            var randomizer = new Random();

            int value = randomizer.Next(minimum, maximum);  // creates a number between 1 and 12

            LOG.Log(value);

            return value;
        }
    }
}