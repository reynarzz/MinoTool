using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using ui = ImGuiNET.ImGui;

namespace MinoTool
{
    internal class MinoTitleToolBar : IEngineUI
    {
        private readonly Action _onClientMenu;
        public MinoTitleToolBar(Action OnClientMenu)
        {
            _onClientMenu = OnClientMenu;
        }

        public void MGUI(Time time)
        {
            ui.BeginMainMenuBar();

            _onClientMenu?.Invoke();

            if (ui.BeginMenu("About"))
            {
                ui.Text("By 'reynarz'");
                ui.EndMenu();
            }

            ui.EndMainMenuBar();
        }
    }
}
