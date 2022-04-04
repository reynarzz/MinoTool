using System;
using System.Collections.Generic;
using System.Text;

namespace MinoTool
{
    internal class ObjectSelectorInternal
    {
        private static ObjectSelectorInternal _objectSelectorInternal;
        public static ObjectSelectorInternal Inst => _objectSelectorInternal ?? (_objectSelectorInternal = new ObjectSelectorInternal());

        internal EntityObject Current { get; private set; }
        internal int CurrentGizmo { get; private set; }

        public List<EntityObject> _selectedObjs;
        public List<int> _selectedGizmos;

        internal event Action<EntityObject> OnObjectSelected;
        internal event Action<EntityObject> OnObjectUnSelected;
        internal event Action<EntityObject[]> OnObjectSelectionArrayChanged;

        internal event Action<GizmoEntity> OnGizmoSelected;
        internal event Action<GizmoEntity> OnGizmoUnSelected;
        internal event Action<int[]> OnGizmoSelectionArrayChanged;

        private ObjectSelectorInternal()
        {
            _selectedObjs = new List<EntityObject>();
            _selectedGizmos = new List<int>();
        }

        internal void SelectObj_Internal(EntityObject obj)
        {
            if (!_selectedObjs.Contains(obj))
            {
                Current = obj;

                _selectedObjs.Add(obj);
                OnObjectSelected?.Invoke(obj);

                OnObjectSelectionArrayChanged?.Invoke(_selectedObjs.ToArray());
            }
            else
            {
                Unselect_Internal(obj);
            }

        }

        internal void SelectGizmo_Internal(GizmoEntity obj)
        {
            if (!_selectedGizmos.Contains(obj.GizmoIndex))
            {
                CurrentGizmo = obj.GizmoIndex;

                _selectedGizmos.Add(obj.GizmoIndex);
                OnGizmoSelected?.Invoke(obj);

                OnGizmoSelectionArrayChanged?.Invoke(_selectedGizmos.ToArray());
            }
            
        }

        internal EntityObject[] GetSelectedObjectsArray_Internal()
        {
            return _selectedObjs.ToArray();
        }

        internal void Unselect_Internal(EntityObject obj)
        {
            if (_selectedObjs.Contains(obj))
            {
                if (Current == obj)
                {
                    Current = null;
                }

                _selectedObjs.Remove(obj);

                OnObjectUnSelected?.Invoke(obj);
                OnObjectSelectionArrayChanged?.Invoke(_selectedObjs.ToArray());
            }
            else
            {
                LOG.Log("Trying to remove a non select object: " + obj.name);
            }
        }

        internal void UnselectGizmo_Internal()
        {
            _selectedGizmos.Clear();
            CurrentGizmo = 0;
        }
        //internal void UnselectGizmo_Internal(GizmoEntity obj)
        //{
        //    if (_selectedGizmos.Count > 0 && _selectedGizmos.Exists(x => x == obj.GizmoIndex))
        //    {
        //        if (CurrentGizmo == obj)
        //        {
        //            CurrentGizmo = null;
        //        }

        //        _selectedGizmos.Remove(obj.GizmoIndex);

        //        OnGizmoUnSelected?.Invoke(obj);
        //        OnGizmoSelectionArrayChanged?.Invoke(_selectedGizmos.ToArray());
        //    }
        //    else
        //    {
        //        LOG.Log("Trying to remove a non select object: " + obj.name);
        //    }
        //}

        internal void SelectByID(string entityID, bool isGizmo = false)
        {
            if (isGizmo)
            {
                for (int i = 0; i < Gizmos.Renderers.Count; i++)
                {
                    var gizmoRenderer = Gizmos.Renderers[i];

                    if (gizmoRenderer.EntityID.Equals(entityID, StringComparison.OrdinalIgnoreCase))
                    {
                        LOG.Log("Gizmo");
                        SelectGizmo_Internal(Gizmos.Entities[i]);
                    }
                }
            }
            else
            {
                var onSceneRenderers = EntitiesManager.Instance.Renderables;
                for (int i = 0; i < onSceneRenderers.Count; i++)
                {
                    var renderer = onSceneRenderers.GetRenderer(i);
                    if (renderer.EntityID.Equals(entityID, StringComparison.OrdinalIgnoreCase))
                    {
                        LOG.Log(renderer.gameObject.name);

                        UnselectAll_Internal();
                        SelectObj_Internal(renderer.gameObject);
                    }
                }
            }
        }

        internal void UnselectAll_Internal()
        {
            if (Current != null)
            {
                OnObjectUnSelected?.Invoke(Current);
            }
            
            Current = null;
            _selectedObjs.Clear();
            OnObjectSelectionArrayChanged?.Invoke(_selectedObjs.ToArray());

            CurrentGizmo = 0;
            _selectedGizmos.Clear();
            OnGizmoSelectionArrayChanged?.Invoke(_selectedGizmos.ToArray());
        }
    }
}
