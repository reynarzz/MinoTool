using GLFW;
using System;

using static OpenGL.GL;

namespace MinoTool
{
    public static class Screen
    {
        public static int Heigh { get; internal set; }
        public static int Width { get; internal set; }

        internal const int UI_RefHeigh = 540;
        internal const int UI_RefWidth = 960;
    }

    public class AppWindow
    {
        public event Action<Window, int, int> OnViewportSizeChanged;

        /// <summary>Screen width in pixels.</summary>
        public int Width { get; private set; }

        /// <summary>Screen height in pixels.</summary>
        public int Height { get; private set; }


        public Window WindowInfo { get; private set; }

        // The garbage collector will not collect this randomly.
        private SizeCallback _sizeCallback;

        /// <summary>
        /// Creates and returns a handle to a GLFW window with a current OpenGL context.
        /// </summary>
        public Window CreateWindow(string name, int width, int height)
        {
            Width = width;
            Height = height;

            InitContext();

            // Create window, make the OpenGL context current on the thread, and import graphics functions
            WindowInfo = Glfw.CreateWindow(width, height, name, Monitor.None, Window.None);

            // Center window
            var screen = Glfw.PrimaryMonitor.WorkArea;

            //--Glfw.SetWindowSizeLimits(WindowInfo, 1000, 500, screen.Width, screen.Height);
           

            var x = (screen.Width - width) / 2;
            var y = (screen.Height - height) / 2;

            Screen.Width = Width;
            Screen.Heigh = Height;

            var ratio = screen.Width / screen.Height;
            // Screen.UI_Width = ;
            // Screen.UI_Heigh = Height;

            Glfw.SetWindowPosition(WindowInfo, x, y);

            Glfw.MakeContextCurrent(WindowInfo);
            Import(Glfw.GetProcAddress);

            _sizeCallback = ResizeCallback;

            Glfw.SetWindowSizeCallback(WindowInfo, _sizeCallback);

            return WindowInfo;
        }

        private static void InitContext()
        {
            // Context
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);

            // Open gl ver: 3.3
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);

            // Core profile, at least a vao is required.
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);

            Glfw.WindowHint(Hint.Doublebuffer, true);

            // Show window borders and buttons.
            Glfw.WindowHint(Hint.Decorated, true);


            Glfw.WindowHint(Hint.Samples, 4);
        }

        private void ResizeCallback(Window window, int w, int h)
        {
            Width = w;
            Height = h;

            Screen.Width = w;
            Screen.Heigh = h;

            glViewport(0, 0, w, h);

            OnViewportSizeChanged?.Invoke(window, w, h);
        }
    }
}
