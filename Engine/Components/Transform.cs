using GlmNet;
using System;
using System.Collections.Generic;

namespace MinoTool
{
    /// <summary>Position, Rotation, and scale manipulation.</summary>
    public class Transform : Component
    {
        internal mat4 _traslationM;
        internal mat4 _rotationM;
        internal mat4 _scaleM;

        private vec3 _position;
        private vec3 _localPosition;
        private vec3 _eulerAngles;
        private vec3 _scale;

        private mat4 _transformMatrix;
        public mat4 TransformMatrix
        {
            get { return _transformMatrix; }
            internal set
            {
                _transformMatrix = value;
            }
        }

        private List<Transform> _children;

        private Transform _parent;
        public Transform parent
        {
            get
            {
                return _parent;
            }
            set
            {
                //TODO
                if (value == null)
                {
                    if (_parent != null && _parent != this)
                    {
                        _parent.RemoveChild_Internal(this);
                    }
                    _parent = this;
                }
                else
                {
                    _parent = value;

                    _parent.SetChild_Internal(this);
                }
            }
        }

        private void RemoveChild_Internal(Transform transform)
        {
            _children.Remove(transform);
            //childCount = _children.Count;
        }

        private void SetChild_Internal(Transform child)
        {
            if (!_children.Contains(child))
            {
                _children.Add(child);
                LOG.Log("Child " + child.name);
            }

            //childCount = _children.Count;
        }

        /// <summary>Entity position.</summary>
        public vec3 position
        {
            get => _position;
            set
            {
                _position = value;
                _traslationM = glm.translate(mat4.identity(), _position);

                if (parent != null)
                {
                    _localPosition = parent.position + value;
                }

                UpdateTransformMatrix();
            }
        }

        public vec3 eulerAngles
        {
            get => _eulerAngles;
            set
            {
                _eulerAngles = value;

                var xRot = glm.rotate(glm.radians(_eulerAngles.x), new vec3(1, 0, 0));
                var yRot = glm.rotate(glm.radians(_eulerAngles.y), new vec3(0, 1, 0));
                var zRot = glm.rotate(glm.radians(_eulerAngles.z), new vec3(0, 0, 1));

                _rotationM = zRot * yRot * xRot;

                UpdateTransformMatrix();
            }
        }

        public vec3 localScale
        {
            get => _scale;
            set
            {
                _scale = value;

                _scaleM = glm.scale(mat4.identity(), _scale);

                UpdateTransformMatrix();
            }
        }

        public vec3 localPosition
        {
            get
            {
                return _localPosition;
            }
            set
            {
                var parentPos = default(vec3);

                if (parent != null)
                {
                    parentPos = parent.position;
                    _position = parentPos - value;
                }

                _localPosition = value;
                //TODO: take rotation an scale in consideration.
                _traslationM = glm.translate(mat4.identity(), _localPosition + parentPos);

                UpdateTransformMatrix();
            }
        }

        public int childCount => _children.Count;

        public vec3 localEulerAngles { get; set; }

        public Vector3 InverseTransformPoint(Vector3 startPos)
        {
            throw new NotImplementedException();
        }

        internal Transform(EntityObject obj) : this()
        {
            _owner_internal = obj;
        }

        // was private
        internal Transform()
        {
            var identity = mat4.identity();

            _transformMatrix = identity;

            _traslationM = identity;
            _rotationM = identity;
            _scaleM = identity;

            _children = new List<Transform>();

            _parent = this;
        }

        internal void UpdateTransformMatrix()
        {
            _transformMatrix = _traslationM * _rotationM * _scaleM;

            for (int i = 0; i < childCount; i++)
            {
                _children[i].onParentChanged();
            }
        }

        private void onParentChanged()
        {
            localPosition = _localPosition;

            for (int i = 0; i < childCount; i++)
            {
                _children[i].onParentChanged();
            }
        }

        public Transform GetChild(int i)
        {
            return _children[i];
        }

        public vec3 TransformVector(vec3 localScale)
        {
            throw new NotImplementedException();
        }

        public void SetSiblingIndex(int atIndex)
        {
            throw new NotImplementedException();
        }

        internal override void _OnDestroyInternal()
        {
            base._OnDestroyInternal();
        }

        public void DestroyChild(int v)
        {
            DestroyImmediate(_children[v].gameObject);
            _children.RemoveAt(v);
        }
    }
}