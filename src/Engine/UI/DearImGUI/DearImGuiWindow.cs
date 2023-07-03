using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinoGUI;
using static OpenGL.GL;
using ImVec4 = System.Numerics.Vector4;

namespace MinoTool
{
    public class DearImGuiWindow
    {
        ImGuiController _controller;
        private int _width;
        private int _height;

        public DearImGuiWindow(int width, int height)
        {
            _width = width;
            _height = height;

            _controller = new ImGuiController(width, height);
            MinoTheme();
           // Style2();
        }

        public void OnResize(int width, int height)
        {
            //// Update the opengl viewport
            glViewport(0, 0, width, height);

            // Tell ImGui of the new size
            _controller.WindowResized(width, height);
        }

        public void OnRenderFrame(Time time)
        {
            _controller.Update(time.DeltaTime);

            //glClearColor(0, 32/255f, 48 / 255f, 255 / 255f);
            //glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);

            //ImGui.ShowDemoWindow();

            _controller.Render();
            //Util.CheckGLError("End of frame");
        }

        private void MinoTheme()
        {
            var style = IMGUI.GetStyle();
            style.WindowRounding = 0;
            style.ChildRounding = 15f;
            style.FrameRounding = 2f;
            style.GrabRounding = 5f;
            style.ScrollbarSize = 11;
            style.WindowTitleAlign = new System.Numerics.Vector2(0.1f, 0.5f);
            // style.ItemSpacing = new System.Numerics.Vector2(5, 5);
            var colors = IMGUI.GetStyle().Colors;

            colors[(int)ImGuiCol.Text] = new ImVec4(0.90f, 0.90f, 0.90f, 1f);
            colors[(int)ImGuiCol.TextDisabled] = new ImVec4(0.60f, 0.60f, 0.60f, 1.00f);
            colors[(int)ImGuiCol.WindowBg] = new ImVec4(0.2f, 0.2f, 0.2f, 1f);
            colors[(int)ImGuiCol.ChildBg] = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.PopupBg] = new ImVec4(0.05f, 0.05f, 0.10f, 1f);
            colors[(int)ImGuiCol.Border] = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.BorderShadow] = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.FrameBg] = new ImVec4(0.1f, 0.1f, 0.1f, 1f); 
            colors[(int)ImGuiCol.FrameBgHovered] = new ImVec4(0.4f, 0.4f, 0.4f, 1.00f);
            colors[(int)ImGuiCol.FrameBgActive] = new ImVec4(0.68f, 0.68f, 0.68f, 0.45f);
            colors[(int)ImGuiCol.TitleBg] = new ImVec4(0.20f, 0.20f, 0.20f, 0.83f);
            colors[(int)ImGuiCol.TitleBgActive] = new ImVec4(0.13f, 0.13f, 0.13f, 1f);
            colors[(int)ImGuiCol.TitleBgCollapsed] = new ImVec4(0.40f, 0.40f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.MenuBarBg] = new ImVec4(0.00f, 0.00f, 0.00f, 0.80f);
            colors[(int)ImGuiCol.ScrollbarBg] = new ImVec4(0.13f, 0.13f, 0.13f, 0.60f);
            colors[(int)ImGuiCol.ScrollbarGrab] = new ImVec4(0.55f, 0.53f, 0.55f, 0.51f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new ImVec4(0.56f, 0.56f, 0.56f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new ImVec4(0.56f, 0.56f, 0.56f, 0.91f);
            colors[(int)ImGuiCol.CheckMark] = new ImVec4(0.90f, 0.90f, 0.90f, 0.83f);
            colors[(int)ImGuiCol.SliderGrab] = new ImVec4(1f, 1f, 1f, 0.62f);
            colors[(int)ImGuiCol.SliderGrabActive] = new ImVec4(1f, 1f, 1f, 0.84f);
            colors[(int)ImGuiCol.Button] = new ImVec4(0.55f, 0.55f, 0.55f, 0.49f);
            colors[(int)ImGuiCol.ButtonHovered] = new ImVec4(0.85f, 0.21f, 0.17f, 0.68f);
            colors[(int)ImGuiCol.ButtonActive] = new ImVec4(0.76f, 0.55f, 0.55f, 1.00f);
            colors[(int)ImGuiCol.Header] = new ImVec4(0.37f, 0.37f, 0.37f, 1.00f);
            colors[(int)ImGuiCol.HeaderHovered] = new ImVec4(0.61f, 0.19f, 0.19f, 1.00f);
            colors[(int)ImGuiCol.HeaderActive] = new ImVec4(0.48f, 0.13f, 0.13f, 1.00f);
            colors[(int)ImGuiCol.Separator] = new ImVec4(0.0f, 0.0f, 0.0f, 1f);
            colors[(int)ImGuiCol.SeparatorHovered] = new ImVec4(0.10f, 0.40f, 0.75f, 0.78f);
            colors[(int)ImGuiCol.SeparatorActive] = new ImVec4(0.10f, 0.40f, 0.75f, 1.00f);
            colors[(int)ImGuiCol.ResizeGrip] = new ImVec4(1.00f, 1.00f, 1.00f, 0.85f);
            colors[(int)ImGuiCol.ResizeGripHovered] = new ImVec4(1.00f, 1.00f, 1.00f, 0.60f);
            colors[(int)ImGuiCol.ResizeGripActive] = new ImVec4(1.00f, 1.00f, 1.00f, 0.90f);
            colors[(int)ImGuiCol.Tab] = new ImVec4(0.64f, 0.06f, 0.15f, 0.86f);
            colors[(int)ImGuiCol.TabHovered] = new ImVec4(0.57f, 0.04f, 0.04f, 0.80f);
            colors[(int)ImGuiCol.TabActive] = new ImVec4(0.85f, 0.17f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocused] = new ImVec4(0.22f, 0.22f, 0.22f, 0.97f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = new ImVec4(0.14f, 0.26f, 0.42f, 1.00f);
            colors[(int)ImGuiCol.DockingPreview] = new ImVec4(0.26f, 0.59f, 0.98f, 0.70f);
            colors[(int)ImGuiCol.DockingEmptyBg] = new ImVec4(0.20f, 0.20f, 0.20f, 1.00f);
            colors[(int)ImGuiCol.PlotLines] = new ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered] = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram] = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = new ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg] = new ImVec4(0.00f, 0.00f, 1.00f, 0.35f);
            colors[(int)ImGuiCol.DragDropTarget] = new ImVec4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight] = new ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight] = new ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg] = new ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = new ImVec4(0.20f, 0.20f, 0.20f, 0.35f);
        }

        private void Style2()
        {
            var io = IMGUI.GetIO();
            IMGUI.GetStyle().FrameRounding = 4.0f;
            IMGUI.GetStyle().GrabRounding = 4.0f;

            var colors = IMGUI.GetStyle().Colors;
            colors[(int)ImGuiCol.Text] = new ImVec4(0.95f, 0.96f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled] = new ImVec4(0.36f, 0.42f, 0.47f, 1.00f);
            colors[(int)ImGuiCol.WindowBg] = new ImVec4(0.11f, 0.15f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.ChildBg] = new ImVec4(0.15f, 0.18f, 0.22f, 1.00f);
            colors[(int)ImGuiCol.PopupBg] = new ImVec4(0.08f, 0.08f, 0.08f, 0.94f);
            colors[(int)ImGuiCol.Border] = new ImVec4(0.08f, 0.10f, 0.12f, 1.00f);
            colors[(int)ImGuiCol.BorderShadow] = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.FrameBg] = new ImVec4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.FrameBgHovered] = new ImVec4(0.12f, 0.20f, 0.28f, 1.00f);
            colors[(int)ImGuiCol.FrameBgActive] = new ImVec4(0.09f, 0.12f, 0.14f, 1.00f);
            colors[(int)ImGuiCol.TitleBg] = new ImVec4(0.09f, 0.12f, 0.14f, 0.65f);
            colors[(int)ImGuiCol.TitleBgActive] = new ImVec4(0.08f, 0.10f, 0.12f, 1.00f);
            colors[(int)ImGuiCol.TitleBgCollapsed] = new ImVec4(0.00f, 0.00f, 0.00f, 0.51f);
            colors[(int)ImGuiCol.MenuBarBg] = new ImVec4(0.15f, 0.18f, 0.22f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarBg] = new ImVec4(0.02f, 0.02f, 0.02f, 0.39f);
            colors[(int)ImGuiCol.ScrollbarGrab] = new ImVec4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new ImVec4(0.18f, 0.22f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new ImVec4(0.09f, 0.21f, 0.31f, 1.00f);
            colors[(int)ImGuiCol.CheckMark] = new ImVec4(0.28f, 0.56f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab] = new ImVec4(0.28f, 0.56f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.SliderGrabActive] = new ImVec4(0.37f, 0.61f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.Button] = new ImVec4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.ButtonHovered] = new ImVec4(0.28f, 0.56f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.ButtonActive] = new ImVec4(0.06f, 0.53f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.Header] = new ImVec4(0.20f, 0.25f, 0.29f, 0.55f);
            colors[(int)ImGuiCol.HeaderHovered] = new ImVec4(0.26f, 0.59f, 0.98f, 0.80f);
            colors[(int)ImGuiCol.HeaderActive] = new ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.Separator] = new ImVec4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.SeparatorHovered] = new ImVec4(0.10f, 0.40f, 0.75f, 0.78f);
            colors[(int)ImGuiCol.SeparatorActive] = new ImVec4(0.10f, 0.40f, 0.75f, 1.00f);
            colors[(int)ImGuiCol.ResizeGrip] = new ImVec4(0.26f, 0.59f, 0.98f, 0.25f);
            colors[(int)ImGuiCol.ResizeGripHovered] = new ImVec4(0.26f, 0.59f, 0.98f, 0.67f);
            colors[(int)ImGuiCol.ResizeGripActive] = new ImVec4(0.26f, 0.59f, 0.98f, 0.95f);
            colors[(int)ImGuiCol.Tab] = new ImVec4(0.11f, 0.15f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.TabHovered] = new ImVec4(0.26f, 0.59f, 0.98f, 0.80f);
            colors[(int)ImGuiCol.TabActive] = new ImVec4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocused] = new ImVec4(0.11f, 0.15f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = new ImVec4(0.11f, 0.15f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.PlotLines] = new ImVec4(0.61f, 0.61f, 0.61f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered] = new ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram] = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = new ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg] = new ImVec4(0.26f, 0.59f, 0.98f, 0.35f);
            colors[(int)ImGuiCol.DragDropTarget] = new ImVec4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight] = new ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight] = new ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg] = new ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = new ImVec4(0.80f, 0.80f, 0.80f, 0.35f);
        }
    }
}
