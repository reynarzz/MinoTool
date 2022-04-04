using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    internal class EngineEditorUI
    {
        private List<IEngineUI> _ui;

        public EngineEditorUI(Action clientMenuGuiCallback)
        {
            _ui = new List<IEngineUI>()
            {
                new MinoTitleToolBar(clientMenuGuiCallback),
               // new InspectorUI()
            };
        }

        public void Update(Time time)
        {
            for (int i = 0; i < _ui.Count; i++)
            {
                _ui[i].MGUI(time);
            }
        }
    }
}