﻿using MinoGUI;
using MinoTool;
using System;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mino.Run<ClassApp>();
        }

        private class ClassApp : MinoApp
        {
            public override void OnGUI()
            {
                IMGUI.ShowDemoWindow();
            }
        }
    }
}