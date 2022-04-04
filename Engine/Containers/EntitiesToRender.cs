using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MinoTool
{
    internal class EntitiesToRender
    {
        internal Dictionary<string, Renderer> _objectsToRender;
        internal int Count => _objectsToRender.Count;

        internal EntitiesToRender()
        {
            _objectsToRender = new Dictionary<string, Renderer>();
        }

        internal Renderer GetRenderer(int index)
        {
            return _objectsToRender.Values.ElementAt(index);
        }

        internal void RemoveRenderer(Renderer renderer)
        {
            if (_objectsToRender.ContainsKey(renderer.EntityID))
            {
                _objectsToRender.Remove(renderer.EntityID);
            }
        }

        internal void AddRenderer(Renderer renderer)
        {
            if (renderer != null)
            {
                //LOG.Log("Renderer: " + renderer.EntityID + ", to: " + renderer.gameObject.name);

                if (!_objectsToRender.ContainsKey(renderer.EntityID))
                {
                    _objectsToRender.Add(renderer.EntityID, renderer);
                }
            }
            else
            {
                LOG.Log("Null renderer");
            }
        }
    }
}