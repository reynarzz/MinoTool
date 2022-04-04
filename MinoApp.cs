using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    /// <summary>Entry point for the engine application.</summary>
    public class MinoApp
    {
        internal Time _time;
        internal Camera _mainCamera;
        internal Assets _assets;
        internal InputManager _input;

        public Time Time => _time;
        public Camera MainCamera => _mainCamera;
        public Assets Assets => _assets;
        public InputManager Input => _input;

        public virtual void OnAppStart() { }
        public virtual void OnGUI() { }
        public virtual void OnQuit() { }
        public virtual void OnToolbarGUI() { }
        public virtual void OnUpdate() {  }
    }
}