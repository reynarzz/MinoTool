using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public abstract class Singleton<T> where T: new()
    {
        private static T _inst;
        public static T Instance => _inst ?? (_inst = new T());

        protected Singleton() { }
    }
}
