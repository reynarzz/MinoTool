using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MinoTool
{
    internal class FilesRepository
    {
        //private Dictionary<string, >
        public const string _root = "Assets/";

        public FilesRepository()
        {

        }

        private void ResolvePaths()
        {

        }

        internal string GetTextFile(string path) 
        {
            return File.ReadAllText(_root + path);
        }
    }
}