using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public class MinoInfo
    {
        public string Version { get; internal set; }
        public IEnumerable<string> Contributors { get; internal set; }
    }
}
