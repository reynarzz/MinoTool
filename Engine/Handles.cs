using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{

	public enum CompareFunction
	{
		/// <summary>
		///   <para>Depth or stencil test is disabled.</para>
		/// </summary>
		Disabled,
		/// <summary>
		///   <para>Never pass depth or stencil test.</para>
		/// </summary>
		Never,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is less than old one.</para>
		/// </summary>
		Less,
		/// <summary>
		///   <para>Pass depth or stencil test when values are equal.</para>
		/// </summary>
		Equal,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is less or equal than old one.</para>
		/// </summary>
		LessEqual,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is greater than old one.</para>
		/// </summary>
		Greater,
		/// <summary>
		///   <para>Pass depth or stencil test when values are different.</para>
		/// </summary>
		NotEqual,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is greater or equal than old one.</para>
		/// </summary>
		GreaterEqual,
		/// <summary>
		///   <para>Always pass depth or stencil test.</para>
		/// </summary>
		Always
	}

	public class Handles
    {
        public static Color color;
        public static CompareFunction zTest;

        public static void Label(Vector3 p, string v)
        {

        }

        public static void DrawWireCube(Vector3 center, Vector3 vector3)
        {
            throw new NotImplementedException();
        }

        public class DrawingScope : IDisposable
        {
            public DrawingScope(Color color)
            {
                throw new Exception("TODO");
            }

            public void Dispose()
            {
            }
        }

        public static Vector3 Slider(Vector3Int arrowPosiX, Vector3 right)
        {
            throw new NotImplementedException();
        }
    }
}
