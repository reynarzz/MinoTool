using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MinoTool
{
    internal class ColorID
    {
        private Dictionary<ColorInt, int> _colorsID;
        public int ColorsCount => _colorsID.Count;

        public ColorID()
        {
            _colorsID = new Dictionary<ColorInt, int>();
            Generate();
        }

        private void Generate()
        {
            var index = 0;
            for (int i = 1; i < 255; i++)
            {
                for (int j = 1; j < 255; j++)
                {
                    _colorsID.Add(new ColorInt(j, i, i / 2, 255), index);
                    index++;
                }
            }
        }

        public ColorInt GetColor(int index)
        {
            return _colorsID.ElementAt(index).Key;
        }

        public bool GetIndex(ColorInt color, out int index)
        {
            return _colorsID.TryGetValue(color, out index);
        }
    }
}
