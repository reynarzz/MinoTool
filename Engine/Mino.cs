using Dear_ImGui_Sample;
using GLFW;
using GlmNet;
using MinoGUI;
using MinoEngine.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MinoTool
{
    public class MinoInfo
    {
        public string Version { get; internal set; }
        public IEnumerable<string> Contributors { get; internal set; }
    }

    public static unsafe class Mino
    {
        private static AppWindow _appWindow;
        private static MinoRenderer _renderer;
        private static Time _time;
        private static Camera _mainCamera;
        private static Assets _assets;
        private static InputManager _input;
        private static EngineEditorUI _engineEditorUI;

        public static MinoInfo Info => new MinoInfo() { Version = "1.0" };

        private static DearImGuiWindow _dearImGuiWindow;
        private static MinoApp _appSandbox;
        private static bool _isRunning = false;
        public static bool IsRunning => _isRunning;

        /// <summary>Closes the mino app.</summary>
        public static void Close()
        {
            _isRunning = false;
        }

        private static void Initialize<T>(string appName, int width = 800, int height = 600) where T : MinoApp, new()
        {
            Screen.Width = width;
            Screen.Heigh = height;

            _appSandbox = new T();
            _appSandbox._time = _time;
            _appSandbox._mainCamera = _mainCamera;
            _appSandbox._assets = _assets;
            _appSandbox._assets = _assets;
            _appSandbox._input = _input;

            var window = new AppWindow();

            window.CreateWindow(appName, width, height);

            //_sandbox = InitSandbox();

            _appWindow = window;
            _mainCamera = new Camera(_appWindow);
            _mainCamera.Transform_Test.position = new vec3(0, -15, -75);
            _mainCamera.Transform_Test.eulerAngles = new vec3(30, 0, 0);
            _time = new Time();
            _renderer = new MinoRenderer(window, EntitiesManager.Instance.Renderables) { MainCamera = _mainCamera };

            _assets = new Assets();
            _engineEditorUI = new EngineEditorUI(_appSandbox.OnToolbarGUI);
            _input = new InputManager(window);


            // Start sanbox.
            _appSandbox.OnAppStart();

            _dearImGuiWindow = new DearImGuiWindow(window.Width, window.Height);

            window.OnViewportSizeChanged += (win, w, h) =>
            {
                _dearImGuiWindow.OnResize(w, h);
            };
        }

        public static void Run<T>(string appName, int srcWidth = 800, int scrHeight = 600) where T : MinoApp, new()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                Initialize<T>(appName, srcWidth, scrHeight);

                ImGuiGLFW.ImGui_ImplGlfw_Init(_appWindow.WindowInfo);


                while (!Glfw.WindowShouldClose(_appWindow.WindowInfo) && _isRunning)
                {
                    // Update Time class
                    _time.Update();

                    // this has to be before "NewFrame() call"
                    //_dearImGuiWindow.UpdateImGuiInput();

                    IMGUI.NewFrame();

                    ImGuiGLFW.ImGui_ImplGlfw_UpdateMousePosAndButtons();
                    ImGuiGLFW.ImGui_ImplGlfw_UpdateMouseCursor();

                    _engineEditorUI.Update(_time);
                    _appSandbox.OnUpdate();

                    _appSandbox.OnGUI();

                    _input.Update();

                    // Update entities.
                    EntitiesManager.Instance.Behaviours.Update(_time);

                    // updating the camera directly, test, TODO: delete.
                    _mainCamera.Update(_time);

                    OpenGL.GL.glEnable(OpenGL.GL.GL_MULTISAMPLE);

                    // Update graphics.
                    _renderer.Draw();

                    _dearImGuiWindow.OnRenderFrame(_time);
                    //_uiSystem.Update(_time);

                    IMGUI.Render();

                    Glfw.SwapBuffers(_appWindow.WindowInfo);

                    Glfw.PollEvents();
                }

                _appSandbox.OnQuit();
                IMGUI.DestroyContext();
                Glfw.Terminate();
            }
            else
            {
                LOG.Error("Mino is already running!");
            }
        }

        private static void ImGuiUI()
        {
            var col = new System.Numerics.Vector3(1f, 0f, 0f);
            float f = 0.0f;
            int counter = 0;
            bool checkbox = false;

            IMGUI.Begin("Hello, world!");                          // Create a window called "Hello, world!" and append into it.

            IMGUI.Text("This is some useful text.");               // Display some text (you can use a format strings too)
            IMGUI.Checkbox("Demo Window", ref checkbox);      // Edit bools storing our window open/close state
            IMGUI.Checkbox("Another Window", ref checkbox);

            IMGUI.SliderFloat("float", ref f, 0.0f, 1.0f);            // Edit 1 float using a slider from 0.0f to 1.0f
            IMGUI.ColorEdit3("clear color", ref col); // Edit 3 floats representing a color

            if (IMGUI.Button("Button"))                            // Buttons return true when clicked (most widgets return true when edited/activated)
                counter++;
            IMGUI.SameLine();
            IMGUI.Text($"counter = {counter}");

            IMGUI.Text($"Application average {1000.0f / IMGUI.GetIO().Framerate}.3f ms/frame ({IMGUI.GetIO().Framerate}.1f FPS)");
            IMGUI.End();

            IMGUI.Begin("Selected obj");

            if (Selector.Current != null)
            {
                IMGUI.Text(Selector.Current.name);
            }
            else
            {
                IMGUI.Text("None");
            }
            IMGUI.End();
        }


        //private MinoApp InitSandbox()
        //{
        //    var types = FindDerivedTypes(Assembly.LoadFrom("Editor"), typeof(MinoApp));

        //    if (types.Count() > 1)
        //    {
        //        for (int i = 0; i < types.Count(); i++)
        //        {
        //            Console.WriteLine("ApplicationEntry: " + types.ElementAt(i).Name);
        //        }

        //        throw new System.Exception("More than one entry point for the application was found");
        //    }
        //    else
        //    {
        //        var sandbox = (MinoApp)Activator.CreateInstance(types.ElementAt(0));

        //        sandbox._mino = this;

        //        return sandbox;
        //    }
        //}

        //private IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        //{
        //    return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
        //}
    }
}