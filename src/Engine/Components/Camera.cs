using GLFW;
using GlmNet;
using MinoGUI;
using System;

namespace MinoTool
{
    public enum CameraProjection
    {
        Perspective,
        Ortographic
    }

    public class Camera : Component
    {
        public mat4 Projection { get; private set; }

        private const float _degrees = 45;
        private const float _degToRad = MathF.PI / 180.0f;

        private float _fov = _degrees * _degToRad;
        private const float _farPlane = 300.0f;
        private const float _nearPlane = 1.0f;

        /// <summary>FOV in degrees</summary>
        public float FOV { get => _fov; set => _fov = value * _degToRad; }

        public Transform Transform_Test { get; private set; }
        private readonly Window _window;
        private float _moveSpeed = 30;
        private float _mouseMovementSpeed = 50;

        private bool _mouseDown;
        private bool _released = true;

        // private MouseCallback _callback;
        //private MouseCallback _scrollCallback;
        private Time _time;

        private int _width;
        private int _height;
        private float lastX;
        private float lastY;
        private bool firstMouse = true;
        private float yaw;
        private float pitch;
        private vec3 cameraFront;
        private bool _canFocusTarget;
        public vec3 NormalizedDirection { get; private set; }
        public vec3 Forward { get; private set; }
        public vec3 Right { get; private set; }
        public vec3 Up { get; private set; }

        private float _ortoh;
        private float _ortoV;

        public Camera(AppWindow window) : base()
        {
            _window = window.WindowInfo;
            Transform_Test = new Transform(gameObject);

            window.OnViewportSizeChanged += UpdateProjection;

            //_callback = MouseDelta;
            //_scrollCallback = ScrollCallback;

            UpdateProjection(window.WindowInfo, window.Width, window.Height);
        }

        private void UpdateProjection(Window window, int width, int height)
        {
            _width = width;
            _height = height;

            _ortoh = _width;
            _ortoV = _height;

            if (_projType == CameraProjection.Perspective)
            {
                Projection = glm.perspective(_fov, width / (float)height, _nearPlane, _farPlane);
            }
            else
            {
                Projection = glm.ortho(-_width, _width, -_height, _height, _nearPlane, _farPlane);
            }
        }

        private CameraProjection _projType;
        public CameraProjection ProjectionMode
        {
            get => _projType;
            set
            {
                _projType = value;

                if (_projType == CameraProjection.Perspective)
                {
                    Projection = glm.perspective(_fov, _width / (float)_height, _nearPlane, _farPlane);
                }
                else if (_projType == CameraProjection.Ortographic)
                {
                    Projection = glm.ortho(-_ortoh, _ortoh, -_ortoh, _ortoh, _nearPlane, _farPlane);
                }
            }
        }

        internal void Update(Time time)
        {
            _time = time;

            _mouseDown = false;

            if (IsKey(Keys.W))
            {
                //Logg.Warning("Up");
                Transform_Test.position += new vec3(0, 0, 1) * time.DeltaTime * _moveSpeed;
            }

            if (IsKey(Keys.S))
            {
                // Logg.Warning("down");
                Transform_Test.position += new vec3(0, 0, -1) * time.DeltaTime * _moveSpeed;
            }

            if (IsKey(Keys.D))
            {
                // Logg.Warning("right");
                Transform_Test.position += new vec3(-1, 0, 0) * time.DeltaTime * _moveSpeed;

            }

            if (IsKey(Keys.A))
            {
                Transform_Test.position += new vec3(1, 0, 0) * time.DeltaTime * _moveSpeed;

                // Logg.Warning("left");
            }

            //if (!IMGUI.IsWindowHovered(ImGuiHoveredFlags.AnyWindow))
            //{
            //    if (_projType == CameraProjection.Perspective)
            //    {
            //        Transform_Test.position += new vec3(0, 0, Input.MouseScrollDelta.y * _mouseMovementSpeed / 8);
            //    }
            //    else
            //    {
            //        _ortoh = Math.Clamp(_ortoh - Input.MouseScrollDelta.y , 0, Screen.Width);
            //        _ortoV = Math.Clamp(_ortoV - Input.MouseScrollDelta.y , 0, Screen.Heigh);

            //        Projection = glm.ortho(-_ortoh / 2f, _ortoh / 2f, -_ortoV / 2f, _ortoV / 2f, _nearPlane, _farPlane);
            //    }
            //}

            if (_projType == CameraProjection.Perspective)
            {
                if (IsMouseButton(MouseButton.Button1) && IsKey(Keys.LeftAlt))
                {
                    //Transform_Test.eulerAngles = new vec3(Transform_Test.eulerAngles.x + Input.MousePositionDelta.y * _mouseMovementSpeed * 3.4f, Transform_Test.eulerAngles.y + Input.MousePositionDelta.x * _mouseMovementSpeed * 3.4f, 0);

                    //glm.lookAt(new vec3());
                    // Transform_Test.eulerAngles = //new vec3(Transform_Test.eulerAngles.x + Input.MousePositionDelta.y * _mouseMovementSpeed * 3.4f, Transform_Test.eulerAngles.y + Input.MousePositionDelta.x * _mouseMovementSpeed * 3.4f, 0);
                    mouse_callback(Input.MousePosition.x, Input.MousePosition.y);

                    var target = new vec3(0, 0, 0);
                    NormalizedDirection = glm.normalize(Transform_Test.position - target);
                }
            }
            if (IsMouseButton(MouseButton.Button3))
            {
                Transform_Test.position = new vec3(Transform_Test.position.x + Input.MousePositionDelta.x * _mouseMovementSpeed, Transform_Test.position.y + -Input.MousePositionDelta.y * _mouseMovementSpeed, Transform_Test.position.z);


            }

            _released = !IsMouseButton(MouseButton.Button1) && !IsMouseButton(MouseButton.Button2) && !IsMouseButton(MouseButton.Button3);

            if (_released)
            {
                firstMouse = true;
            }


            if (Input.IsKey(Keys.F) && !_canFocusTarget)
            {
                _canFocusTarget = true;
            }

            if (_canFocusTarget)
            {
                FocusTarget(time.DeltaTime);
            }
        }

        private bool IsKey(Keys key)
        {
            var state = Glfw.GetKey(_window, key);
            return state == InputState.Press;
        }

        private bool IsMouseButton(MouseButton button)
        {
            var state = Glfw.GetMouseButton(_window, button);
            return state == InputState.Press;
        }

        void mouse_callback(float xpos, float ypos)
        {
            if (firstMouse)
            {
                lastX = xpos;
                lastY = ypos;
                firstMouse = false;
            }

            float xoffset = xpos - lastX;
            float yoffset = lastY - ypos;
            lastX = xpos;
            lastY = ypos;

            float sensitivity = 0.1f;
            xoffset *= sensitivity;
            yoffset *= sensitivity;

            yaw += xoffset;
            pitch += yoffset;

            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;

            vec3 direction;
            direction.x = glm.cos(glm.radians(yaw)) * glm.cos(glm.radians(pitch));
            direction.y = glm.sin(glm.radians(pitch));
            direction.z = glm.sin(glm.radians(yaw)) * glm.cos(glm.radians(pitch));
            cameraFront = glm.normalize(direction);


            var target = default(vec3);

            if (Selector.Current != null)
            {
                target = Selector.Current.transform.position;
            }

            var loomM = glm.lookAt(new vec3(0, 0, 0)/*target - cameraFront*/, cameraFront /*+ target*/, new vec3(0, 1, 0));
            //var loomM = glm.lookAt(target - cameraFront, cameraFront+ target, new vec3(0, 1, 0));
            Transform_Test._rotationM = loomM;

            Forward = direction;
            Right = glm.cross(direction, new vec3(0, 1, 0));
            Up = glm.cross(Right, Forward);

            //Console.WriteLine("F: " + cameraFront + ", T: " + Up + ", R: " + Right );
            //LOG.Log(Transform_Test.TransformMatrix);

            //Transform_Test.UpdateTransformMatrix();
        }
        private float _t;

        private void FocusTarget(float deltaTime)
        {
            //var targetPos = (Selector.Current.transform.position + _startPos) / 2;

            ////LOG.Log(targetPos );
            //_t += deltaTime;

            //Transform_Test.position = Utils.Lerp(_startPos, new vec3(targetPos.x, targetPos.y, targetPos.z), _t);
        }

        //private bool IsMouseButtonDown(MouseButton button)
        //{
        //    var state = Glfw.GetMouseButton(_window, button);
        //    var isDown = state == InputState.Press;

        //    if (isDown && _released)
        //    {
        //        _mouseDown = true;
        //    }

        //    return _mouseDown;
        //}

        //private void MouseDelta(Window window, double x, double y)
        //{
        //    var currentX = (float)x;
        //    var currentY = (float)y;

        //    currentX = currentX / _width;
        //    currentY = currentY / _height;

        //    if (IsMouseButtonDown(MouseButton.Button1) || IsMouseButtonDown(MouseButton.Button2) || IsMouseButtonDown(MouseButton.Button3))
        //    {
        //        _prevMousePos.x = currentX;
        //        _prevMousePos.y = currentY;
        //    }

        //    _delta = new Vector2(currentX - _prevMousePos.x, currentY - _prevMousePos.y);



        //    _prevMousePos.x = currentX;
        //    _prevMousePos.y = currentY;
        //}

    }
}