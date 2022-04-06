using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinoTool;
using MinoGUI;

namespace ConsoleAppMinoTool
{
    public class CustomUI : MinoApp 
    {
        public override void OnAppStart() { }

        public override void OnGUI()
        {
            IMGUI.ShowDemoWindow();
        }

        public override void OnToolbarGUI() { }

        public override void OnQuit() { }
    }
}