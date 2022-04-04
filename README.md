## MinoTool
A custom C# - OpenGL standalone lib to create tools easier!

### Simple UI Dear ImGui: 
# 
![](readmefiles/basic.png)

### But is that all you can do with it? 
## You can do some interesting tools taking advantage of its (wip) 3D renderer.
# Here is a tool I made some time ago for my own game in Unity3D. 
![](readmefiles/unity3D_original_tool.gif)

And this is the port to MinoTool:
![](readmefiles/standalone_tool_3d.gif)

### just minor changes to the original Unity editor code were made to port the entire tool!
Original:

```c#
using UnityEditor;

namespace LevelBuilder
{
    public class LevelBuilderPropsSidebarUI : ILevelBuilderUIMenu
    {
        public void OnGUI(B_Column column, EditorWindow window)
        {
            _column = column;

            if (column != null)
            {
                var areaRect = new Rect(x, y, width, Screen.height - ofssetHeigh);

                GUILayout.BeginArea(areaRect);

                window.BeginWindows();

                var color = Color.black;
                color.a = semiTransparent;

                var drawRect = areaRect;
                drawRect.x = 0;
                drawRect.y = 0;
                drawRect.width = drawRectWidth;

                EditorGUI.DrawRect(drawRect, color);
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                var c = GUI.color;
                GUI.color = Color.green;
                GUILayout.Label(localizedTitle);
                GUI.color = c;

                var enabled = EditorGUILayout.Toggle(column.IsHiddenPathEnabled);
                GUILayout.EndHorizontal();
                ...

```
# Port to MinoTool

```c#
using MinoTool;
using MinoGUI;

namespace LevelBuilder
{
    public class LevelBuilderPropsSidebarUI : ILevelBuilderUIMenu
    {
        public void OnGUI(B_Column column)
        {
            if (column != null)
            {
                var areaSize = _areaSize;
                var areaPos = _areaPos; 

                IMGUI.Begin(CateogoryName, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse);
                IMGUI.SetWindowSize(areaSize);
                IMGUI.SetWindowPos(areaPos);

                var width = IMGUI.GetWindowWidth() - offset;

                if (IMGUI.CollapsingHeader(MainPropsTitle, ImGuiTreeNodeFlags.DefaultOpen))
                {
                    for (int i = 0; i < _categories.MainPropsNames.Length; i++)
                    {
                        if (IMGUI.Button(_categories.MainPropsNames[i], new System.Numerics.Vector2(width, height)))
                        {
                            _builder.SetProp(column, _subMenuUIControl.Container, _categories.GetPropEnum(_categories.MainPropsNames[i]));
                        }
                    }
                }

                IMGUI.Spacing();
                IMGUI.Spacing();

                if (IMGUI.CollapsingHeader(SecondaryPropsTitle, ImGuiTreeNodeFlags.DefaultOpen))
                ...
```
# Gameplay showing the final level!
![](readmefiles/level_gameplay.gif)

### To Do
- [ ] Refactor.
   - [ ] Fix some expensive calls.
   - [ ] Arquitecture.
- [ ] Directional light.
- [ ] Simple shadow.
