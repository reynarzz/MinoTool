using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public class InspectorUI : IEngineUI
    {
        public void MGUI(Time time)
        {
            ImGui.Begin("Window", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar 
                | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoDecoration);
            
            ImGui.SetWindowSize(new System.Numerics.Vector2(200, Screen.Heigh-20));
            ImGui.SetWindowPos(new System.Numerics.Vector2(0, 19));
            ImGui.BeginChild("Entities");

            for (int i = 0; i < 6; i++)
            {
                ImGui.Text("Entity");
            }
            ImGui.EndChild();
            ImGui.End();
        }
    }
}