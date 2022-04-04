using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    public static class Selector
    {
        public static EntityObject[] SelectedObjects => ObjectSelectorInternal.Inst.GetSelectedObjectsArray_Internal();
        public static EntityObject Current => ObjectSelectorInternal.Inst.Current;

        internal static ObjectSelectorInternal Internal => ObjectSelectorInternal.Inst;

        public static void SelectObj(EntityObject obj)
        {
            UnselectAll();
            ObjectSelectorInternal.Inst.SelectObj_Internal(obj);
        }

        public static void AddToSelection(EntityObject obj)
        {
            ObjectSelectorInternal.Inst.SelectObj_Internal(obj);
        }

        public static void UnselectAll()
        {
            ObjectSelectorInternal.Inst.UnselectAll_Internal();
        }

        public static void Unselect(EntityObject obj)
        {
            ObjectSelectorInternal.Inst.Unselect_Internal(obj);
        }
    }
}
