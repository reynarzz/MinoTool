using GLFW;
using GlmNet;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public static class Input
    {
        public static vec2 MousePosition { get; internal set; }
        public static vec3 MousePositionDelta { get; internal set; }

        public static vec2 MouseScrollDelta { get; internal set; }
        public static bool LeftClick { get; internal set; }
        public static bool RightClick { get; internal set; }
        internal static Keys CurrentKey { get; set; }

        internal static AppWindow _window;
        public static Action<MouseButton> OnMouseButtonDown;

        public static bool IsKey(Keys key)
        {
            var state = Glfw.GetKey(_window.WindowInfo, key);
            return state == InputState.Press;
        }

        public static bool IsMouseButton(MouseButton button)
        {
            var state = Glfw.GetMouseButton(_window.WindowInfo, button);
            return state == InputState.Press;
        }
    }

    public class InputManager
    {
        private vec3 _delta;
        private vec2 _prevMousePos;
        private vec2 _mousePos;
        private vec2 _mouseScroll;
        private vec2 _clickDelta;

        private bool _mouseDown;
        private bool _released = true;

        private MouseCallback _callback;
        private MouseCallback _scrollCallback;

        private readonly AppWindow _window;

        internal InputManager(AppWindow window)
        {
            _window = window;

            _callback = MousePosition;
            _scrollCallback = ScrollCallback;

            Glfw.SetCursorPositionCallback(_window.WindowInfo, _callback);
            Glfw.SetScrollCallback(_window.WindowInfo, _scrollCallback);

            Input._window = _window;
        }

        internal void Update()
        {
            _mouseDown = false;
            Input.LeftClick = IsMouseButtonDown(MouseButton.Left);

            Input.LeftClick = IsMouseButton(MouseButton.Left);
            Input.RightClick = IsMouseButton(MouseButton.Right);

            Input.MousePosition = _mousePos;
            Input.MouseScrollDelta = _mouseScroll;
            Input.MousePositionDelta = _delta;

            _mouseScroll = default;
            _delta = default;

            _released = !IsMouseButton(MouseButton.Button1) && !IsMouseButton(MouseButton.Button2) && !IsMouseButton(MouseButton.Button3);
        }

        public bool IsKey(Keys key)
        {
            var state = Glfw.GetKey(_window.WindowInfo, key);
            return state == InputState.Press;
        }

        private bool IsMouseButton(MouseButton button)
        {
            var state = Glfw.GetMouseButton(_window.WindowInfo, button);
            return state == InputState.Press;
        }

        private bool IsMouseButtonDown(MouseButton button)
        {
            var state = Glfw.GetMouseButton(_window.WindowInfo, button);
            var isDown = state == InputState.Press;

            if (isDown && _released)
            {
                _mouseDown = true;

                if (!ImGui.IsAnyItemHovered() && !ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
                {
                    Input.OnMouseButtonDown(button);
                }
            }

            return _mouseDown;
        }

        private void MousePosition(Window window, double x, double y)
        {
            _mousePos.x = (float)x;
            _mousePos.y = (float)y;

            var currentX = (float)x;
            var currentY = (float)y;

            currentX = currentX / _window.Width;
            currentY = currentY / _window.Height;

            if (IsMouseButtonDown(MouseButton.Button1) || IsMouseButtonDown(MouseButton.Button2) || IsMouseButtonDown(MouseButton.Button3))
            {
                _prevMousePos.x = currentX;
                _prevMousePos.y = currentY;
            }

            _delta = new vec3(currentX - _prevMousePos.x, currentY - _prevMousePos.y, 0);
          
            
            _prevMousePos.x = currentX;
            _prevMousePos.y = currentY;
        }

        private void ScrollCallback(Window window, double x, double y)
        {
            _mouseScroll = new vec2((float)x, (float)y);
        }
    }
}
