using System.Collections.Generic;
using MinoTool;

namespace MinoTool
{
    /// <summary>The entry point for MinoTool.</summary>
    public static class Mino
    {
        private readonly static MinoEngineEntry _engine;

        private const string _defWTitle = "Mino App";
        private const int _defScrWidth = 960;
        private const int _defScrHigth = 480;

        static Mino()
        {
            _engine = new MinoEngineEntry();
        }

        public static void Run<T>(string windowName = _defWTitle, 
                                  GraphicsBackend backend = GraphicsBackend.OpenGL, 
                                  int scrWidth = _defScrWidth, int scrHeight = _defScrHigth) where T : MinoApp, new()
        {
            _engine.Run<T>(windowName, backend, scrWidth, scrHeight);
        }

        public static void Close()
        {
            _engine.Close();
        }
    }
}