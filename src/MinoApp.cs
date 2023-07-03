using GLFW;
using System;

namespace MinoTool
{
    /// <summary>Entry point for the engine application.</summary>
    public class MinoApp
    {
        public Time Time { get; internal set; }
        public Camera MainCamera { get; internal set; }
        public Assets Assets { get; internal set; }
        //public InputManager Input { get; internal set; }
        internal event Action<CursorType> OnCursorChange;
        protected void ChangeCursor(CursorType cursor)
        {
            OnCursorChange(cursor);
        }

        public virtual void OnAppStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnGUI() { }
        public virtual void OnToolbarGUI() { }
        public virtual void OnQuit() { }
    }
}