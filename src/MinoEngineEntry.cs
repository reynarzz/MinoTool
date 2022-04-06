using System;
using System.Collections.Generic;
using System.Text;
using GLFW;
using GlmNet;
using MinoTool.ThirdParty;
using MinoGUI;

namespace MinoTool
{
    internal unsafe class MinoEngineEntry
    {
        private AppWindow _appWindow;
        private MinoRenderer _renderer;
        private Time _time;
        private Camera _mainCamera;
        private Assets _assets;
        private InputManager _input;
        private EngineEditorUI _engineEditorUI;

        public MinoInfo Info => new MinoInfo() { Version = "1.0" };

        private DearImGuiWindow _dearImGuiWindow;
        private MinoApp _appSandbox;
        private bool _isRunning = false;
        public bool IsRunning => _isRunning;


        /// <summary>Closes the mino app.</summary>
        public void Close()
        {
            _isRunning = false;
        }

        private void Initialize<T>(string appName, int width = 800, int height = 600) where T : MinoApp, new()
        {
            Screen.Width = width;
            Screen.Heigh = height;

            _appSandbox = new T();
            _appSandbox.Time = _time;
            _appSandbox.MainCamera = _mainCamera;
            _appSandbox.Assets = _assets;
            _appSandbox.Input = _input;

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

        public void Run<T>(string appName, GraphicsBackend backend, int srcWidth, int scrHeight) where T : MinoApp, new()
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
    }
}
