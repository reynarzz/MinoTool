## MinoTool
A custom C# - OpenGL standalone lib to create tools easier!

## Simple Dear ImGui implementation: 
If you have used Dear ImGui in the past there is not more to it apart from an easy to implement API. 
#### Build, import the libs to your app, and: 

### Main.cs
```c#
using MinoTool;

namespace Program
{
    public class Program 
    {
        public static void Main()
        {
            // Window name.
            // Screen width.
            // Screen height.
            
            Mino.Run<CustomUI>("Mino tool", 1324, 600);
        }
    }
}
```

### CustomUI.cs
```c#
using MinoTool;
using MinoGUI;

namespace MinoMain
{
    public class CustomUI : MinoApp
    {
        public override void OnAppStart() 
        {
            // initialization.
        }

        public override void OnGUI()
        {
            IMGUI.ShowDemoWindow();
        }

        public override void OnToolbarGUI()
        {
            // Toolbar Ui here!
        }

        public override void OnQuit() 
        {
            // Cleanup here.
        }
    }
}
```
### And this is the result:
The imgui demo window and a tool bar.
![](readmefiles/basic.png)

# But... is that all you can do with it? 

#### You can do some interesting tools when taking advantage of its (wip) 3D renderer.
## Here is a tool I made for my own game in Unity3D. 
Powered by the Unity's old GUI system.
![](readmefiles/unity3D_original_tool.gif)

## And this is the port to MinoTool:  OpenGl 3.3, dear Imgui, C#
Box stack modification.
![](readmefiles/standalone_tool_3d.gif)

#### just minor changes to the original Unity editor code were made to port the core of the tool!
Original, Unity3D:

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
                var areaRect = new Rect(x, y, width, Screen.height - offsetHeigh);

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
## Port to MinoTool:
The (WIP) architecture of MinoTool is almost identical to Unity3D, which helps to make the tools porting as smooth as possible.

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

                IMGUI.Begin(CategoryName, ImGuiWindowFlags.NoResize | 
                                           ImGuiWindowFlags.NoMove | 
                                           ImGuiWindowFlags.NoCollapse);

                IMGUI.SetWindowSize(areaSize);
                IMGUI.SetWindowPos(areaPos);

                var width = IMGUI.GetWindowWidth() - offset;

                if (IMGUI.CollapsingHeader(MainPropsTitle, ImGuiTreeNodeFlags.DefaultOpen))
                {
                    for (int i = 0; i < _categories.MainPropsNames.Length; i++)
                    {
                        if (IMGUI.Button(_categories.MainPropsNames[i], 
                                         new System.Numerics.Vector2(width, height)))
                        {
                            _builder.SetProp(column, _subMenuUIControl.Container, 
                                            _categories.GetPropEnum(_categories.MainPropsNames[i]));
                        }
                    }
                }

                IMGUI.Spacing();
                IMGUI.Spacing();

                if (IMGUI.CollapsingHeader(SecondaryPropsTitle, ImGuiTreeNodeFlags.DefaultOpen))
                ...
```

### To Do
- [ ] Refactor.
   - [ ] Fix some expensive calls.
   - [ ] Arquitecture.
   - [ ] Contain all third party dependencies in the main DLL.
- [ ] Directional light.
- [ ] Simple shadow.
- [ ] Port to Mac.
- [ ] Port to Linux.
- [ ] Port to Android.

### Dependencies
-Dear Imgui (C# wrapper).

StBImage.
GLfw.
GL lib.
Glm.
