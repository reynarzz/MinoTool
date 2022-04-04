## MinoTool
A custom C# - OpenGL standalone lib to create tools easier!

### Simple UI Dear ImGui: 
# 
![](readmefiles/basic.png)

### But is that all you can do with it? 
## You can do some interesting tools taking advantage of its (wip) 3D renderer.
# Here is a tool I made some time ago for my own game in Unity3D. 
![](readmefiles/unity3D_original_tool.gif)

# And this is the port to MinoTool:
![](readmefiles/standalone_tool_3d.gif)

### just minor changes to the original Unity editor code were made to port the entire tool!
# Original

```c#
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace LevelBuilder
{
    public class ColumnEditor
    {
        private GUIContent _content;
        [SerializeField] private Texture2D _icon;

        private B_Column _column;

        public ColumnEditor(LevelGrid grid)
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;

            _grid = grid;
            _content = new GUIContent("Column editor")
            {
                text = "Editor",
                image = _icon
            };

            _columnsToAdd = new List<ColumnData>();
        }

        public void OnGUI(B_Column column)
        {
            var zTest = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;


            if (column)
            {
                if (column && column.gameObject.activeInHierarchy)
                {
                    if (_column != column)
                    {
                        if (_column)
                        {
                            OnColumnUnselected(_column);
                        }

                        _column = column;
```
# Port to MinoTool

```c#
using System;
using MinoTool;

namespace LevelBuilder
{
    public class ColumnEditor : EntityBehaviour
    {
        private B_Column _column;

        private enum CurrentSideArrow
        {
            None, Front, Back, Right, Left
        }

        private float _increaseHeightEvery = 3.5f;

        private CurrentSideArrow _currentSideArrow;

        private readonly LevelGrid _grid;

        public ColumnEditor(LevelGrid grid)
        {
            Handles.zTest = CompareFunction.Less;

            _grid = grid;
         
            _columnsToAdd = new List<ColumnData>();
        }

        public void OnGUI(B_Column column)
        {
            var zTest = Handles.zTest;
            Handles.zTest = CompareFunction.Less;


            if (column != null)
            {
                if (column != null && column.gameObject.activeInHierarchy)
                {
                    if (_column != column)
                    {
                        if (_column != null)
                        {
                            OnColumnUnselected(_column);
                        }

                        _column = column;
```
# Gameplay showing the created level!
![](readmefiles/level_gameplay.gif)

### To Do
- [ ] Refactor.
   - [ ] Fix some expensive calls.
   - [ ] Arquitecture.
- [ ] Directional light.
- [ ] Simple shadow.
