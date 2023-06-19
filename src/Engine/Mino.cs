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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="windowName"></param>
        /// <param name="scrWidth"></param>
        /// <param name="scrHeight"></param>
        /// <param name="backend">Only OpenGL is available at this point.</param>
        public static void Run<T>(string windowName = _defWTitle, int scrWidth = _defScrWidth, 
                                  int scrHeight = _defScrHigth, GraphicsBackend backend = GraphicsBackend.OpenGL) where T : MinoApp, new()
        {
            _engine.Run<T>(windowName, scrWidth, scrHeight, backend);
        }

        public static void Close()
        {
            _engine.Close();
        }
    }
}