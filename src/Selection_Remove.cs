using System;

namespace MinoTool
{
    public class Selection_Remove
    {
        private static Entity _selection;
        public static Entity activeObject 
        {
            get 
            {
                return _selection;
            }
            set
            {
                Console.WriteLine("Selected");
                _selection = value;
            } 
        }
    }
}