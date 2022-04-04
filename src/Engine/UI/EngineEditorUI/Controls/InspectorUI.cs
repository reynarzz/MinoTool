using MinoGUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public class InspectorUI : IEngineUI
    {
        public void MGUI(Time time)
        {
            IMGUI.Begin("Window", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar 
                | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoDecoration);
            
            IMGUI.SetWindowSize(new System.Numerics.Vector2(200, Screen.Heigh-20));
            IMGUI.SetWindowPos(new System.Numerics.Vector2(0, 19));
            IMGUI.BeginChild("Entities");

            for (int i = 0; i < 6; i++)
            {
                IMGUI.Text("Entity");
            }
            IMGUI.EndChild();
            IMGUI.End();
        }
    }
}