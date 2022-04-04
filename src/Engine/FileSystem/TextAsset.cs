using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public sealed class TextAsset : Asset
    {
        public string Text { get; private set; }
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }


        internal TextAsset(string contents)
        {
            Text = contents;
        }
    }
}
