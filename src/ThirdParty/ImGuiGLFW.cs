using System;
using System.Collections.Generic;
using System.Text;
using GLFW;
using MinoGUI;

namespace MinoTool.ThirdParty
{
    public static class ImGuiGLFW
    {
        static Window g_Window;    // Main window
        static double g_Time = 0.0;
        static bool[] g_MouseJustPressed = new bool[8];
        static Cursor[] g_MouseCursors = new Cursor[(int)ImGuiMouseCursor.COUNT];

        // Chain GLFW callbacks: our callbacks will call the user's previously installed callbacks, if any.
        static MouseButtonCallback g_PrevUserCallbackMousebutton = null;
        static MouseCallback g_PrevUserCallbackScroll = null;
        static KeyCallback g_PrevUserCallbackKey = null;
        static CharCallback g_PrevUserCallbackChar = null;
        private static ClientApi g_ClientApi;
        private static readonly int FLT_MAX = 1;

        private static MouseButtonCallback _mouseButtonCallback;
        private static MouseCallback _scrollCallback;
        private static KeyCallback _keyCallback;
        private static CharCallback _charCallback;


        static ImGuiGLFW()
        {
            //Cache the callbacks so the garbage collector cannot remove these
            _mouseButtonCallback = ImGui_ImplGlfw_MouseButtonCallback;
            _scrollCallback = ImGui_ImplGlfw_ScrollCallback;

            _keyCallback = ImGui_ImplGlfw_KeyCallback;
            _charCallback = ImGui_ImplGlfw_CharCallback;
        }

        public static bool ImGui_ImplGlfw_Init(Window window, bool install_callbacks = true, ClientApi client_api = ClientApi.OpenGL)
        {
            g_Window = window;
            g_Time = 0.0;
            // Setup back-end capabilities flags
            var io = IMGUI.GetIO();
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;         // We can honor GetMouseCursor() values (optional)
            io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;          // We can honor io.WantSetMousePos requests (optional, rarely used)
            //io.BackendPlatformName = "imgui_impl_glfw";

            // Keyboard mapping. ImGui will use those indices to peek into the io.KeysDown[] array.
            io.KeyMap[(int)ImGuiKey.Tab] = (int)Keys.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keys.Left;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keys.Right;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keys.Up;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keys.Down;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)Keys.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)Keys.PageDown;
            io.KeyMap[(int)ImGuiKey.Home] = (int)Keys.Home;
            io.KeyMap[(int)ImGuiKey.End] = (int)Keys.End;
            io.KeyMap[(int)ImGuiKey.Insert] = (int)Keys.Insert;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)Keys.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)Keys.Backspace;
            io.KeyMap[(int)ImGuiKey.Space] = (int)Keys.Space;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)Keys.Enter;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)Keys.Escape;
            //io.KeyMap[(int)ImGuiKey.KeyPadEnter] = (int)Keys.NumpadEnter;
            io.KeyMap[(int)ImGuiKey.A] = (int)Keys.A;
            io.KeyMap[(int)ImGuiKey.C] = (int)Keys.C;
            io.KeyMap[(int)ImGuiKey.V] = (int)Keys.V;
            io.KeyMap[(int)ImGuiKey.X] = (int)Keys.X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)Keys.Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)Keys.Z;

            // TODO: rph
            //--io.SetClipboardTextFn = ImGui_ImplGlfw_SetClipboardText;
            //--io.GetClipboardTextFn = ImGui_ImplGlfw_GetClipboardText;

            io.ClipboardUserData = g_Window;

            //#if defined(_WIN32)
            //    io.ImeWindowHandle = (void*)glfwGetWin32Window(g_Window);
            //#endif
            g_MouseCursors[(int)ImGuiMouseCursor.Arrow] = Glfw.CreateStandardCursor(CursorType.Arrow);
            g_MouseCursors[(int)ImGuiMouseCursor.TextInput] = Glfw.CreateStandardCursor(CursorType.Beam);
            g_MouseCursors[(int)ImGuiMouseCursor.ResizeAll] = Glfw.CreateStandardCursor(CursorType.Arrow);   // FIXME: GLFW doesn't have this.
            g_MouseCursors[(int)ImGuiMouseCursor.ResizeNS] = Glfw.CreateStandardCursor(CursorType.ResizeVertical);
            g_MouseCursors[(int)ImGuiMouseCursor.ResizeEW] = Glfw.CreateStandardCursor(CursorType.ResizeHorizontal);
            g_MouseCursors[(int)ImGuiMouseCursor.ResizeNESW] = Glfw.CreateStandardCursor(CursorType.Arrow);  // FIXME: GLFW doesn't have this.
            g_MouseCursors[(int)ImGuiMouseCursor.ResizeNWSE] = Glfw.CreateStandardCursor(CursorType.Arrow);  // FIXME: GLFW doesn't have this.
            g_MouseCursors[(int)ImGuiMouseCursor.Hand] = Glfw.CreateStandardCursor(CursorType.Hand);
            // Chain GLFW callbacks: our callbacks will call the user's previously installed callbacks, if any.
            g_PrevUserCallbackMousebutton = null;
            g_PrevUserCallbackScroll = null;
            g_PrevUserCallbackKey = null;
            g_PrevUserCallbackChar = null;
            if (install_callbacks)
            {
                g_PrevUserCallbackMousebutton = Glfw.SetMouseButtonCallback(window, _mouseButtonCallback);
                g_PrevUserCallbackScroll = Glfw.SetScrollCallback(window, _scrollCallback);
                g_PrevUserCallbackKey = Glfw.SetKeyCallback(window, _keyCallback);
                g_PrevUserCallbackChar = Glfw.SetCharCallback(window, _charCallback);
            }
            g_ClientApi = client_api;
            return true;
        }
        private static string ImGui_ImplGlfw_GetClipboardText(Window window)
        {
            return Glfw.GetClipboardString(window);
        }

        private static void ImGui_ImplGlfw_SetClipboardText(Window user_data, string text)
        {
            Glfw.SetClipboardString(user_data, text);
        }

        private static void ImGui_ImplGlfw_MouseButtonCallback(Window window, MouseButton button, InputState action, ModifierKeys mods)
        {
            if (g_PrevUserCallbackMousebutton != null)
                g_PrevUserCallbackMousebutton(window, button, action, mods);
            if (action == InputState.Press && button >= 0 && (int)button < g_MouseJustPressed.Length)
                g_MouseJustPressed[(int)button] = true;
        }

        private static void ImGui_ImplGlfw_ScrollCallback(Window window, double xoffset, double yoffset)
        {
            if (g_PrevUserCallbackScroll != null)
                g_PrevUserCallbackScroll(window, xoffset, yoffset);
            var io = IMGUI.GetIO();
            io.MouseWheelH += (float)xoffset;
            io.MouseWheel += (float)yoffset;
        }

        private static void ImGui_ImplGlfw_KeyCallback(Window window, Keys key, int scancode, InputState action, ModifierKeys mods)
        {
            if (g_PrevUserCallbackKey != null)
                g_PrevUserCallbackKey(window, key, scancode, action, mods);
            var io = IMGUI.GetIO();
            if (action == InputState.Press)
                io.KeysDown[(int)key] = true; // can throw an error.
            if (action == InputState.Release)
                io.KeysDown[(int)key] = false;
            // Modifiers are not reliable across systems
            io.KeyCtrl = io.KeysDown[(int)Keys.LeftControl] || io.KeysDown[(int)Keys.RightControl];
            io.KeyShift = io.KeysDown[(int)Keys.LeftShift] || io.KeysDown[(int)Keys.RightShift];
            io.KeyAlt = io.KeysDown[(int)Keys.LeftAlt] || io.KeysDown[(int)Keys.RightAlt];
            io.KeySuper = io.KeysDown[(int)Keys.LeftSuper] || io.KeysDown[(int)Keys.RightSuper];
        }
        private static void ImGui_ImplGlfw_CharCallback(Window window, uint c)
        {
            if (g_PrevUserCallbackChar != null)
                g_PrevUserCallbackChar(window, c);
            var io = IMGUI.GetIO();
            io.AddInputCharacter(c);
        }

        public static void ImGui_ImplGlfw_UpdateMouseCursor()
        {
            var io = IMGUI.GetIO();
            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0 || Glfw.GetInputMode(g_Window, InputMode.Cursor) == (int)CursorMode.Disabled)
                return;

            ImGuiMouseCursor imgui_cursor = IMGUI.GetMouseCursor();
            if (imgui_cursor == ImGuiMouseCursor.None || io.MouseDrawCursor)
            {
                // Hide OS mouse cursor if imgui is drawing it or if it wants no cursor
                Glfw.SetInputMode(g_Window, InputMode.Cursor, (int)CursorMode.Hidden);
            }
            else
            {
                // Show OS mouse cursor
                // FIXME-PLATFORM: Unfocused windows seems to fail changing the mouse cursor with GLFW 3.2, but 3.3 works here.
                Glfw.SetCursor(g_Window, g_MouseCursors[(int)imgui_cursor] != default ? g_MouseCursors[(int)imgui_cursor] : g_MouseCursors[(int)ImGuiMouseCursor.Arrow]);
                Glfw.SetInputMode(g_Window, InputMode.Cursor, (int)CursorMode.Normal);
            }
        }

        public static void ImGui_ImplGlfw_UpdateMousePosAndButtons()
        {
            // Update buttons
            var io = IMGUI.GetIO();
            for (int i = 0; i < io.MouseDown.Count; i++)
            {
                // If a mouse press event came, always pass it as "mouse held this frame", so we don't miss click-release events that are shorter than 1 frame.
                io.MouseDown[i] = g_MouseJustPressed[i] || Glfw.GetMouseButton(g_Window, (MouseButton)i) != 0;
                g_MouseJustPressed[i] = false;
            }
            // Update mouse position
            var mouse_pos_backup = io.MousePos;
            io.MousePos = new System.Numerics.Vector2(-FLT_MAX, -FLT_MAX);
            //# ifdef __EMSCRIPTEN__
            //const bool focused = true; // Emscripten
            //else
            bool focused = Glfw.GetWindowAttribute(g_Window, WindowAttribute.Focused);
            //endif
            if (focused)
            {
                if (io.WantSetMousePos)
                {
                    Glfw.SetCursorPosition(g_Window, (double)mouse_pos_backup.X, (double)mouse_pos_backup.Y);
                }
                else
                {
                    double mouse_x, mouse_y;
                    Glfw.GetCursorPosition(g_Window, out mouse_x, out mouse_y);
                    io.MousePos = new System.Numerics.Vector2((float)mouse_x, (float)mouse_y);
                }
            }
        }
    }
}
