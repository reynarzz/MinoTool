using System;
using System.Diagnostics;
using System.Threading;

namespace MinoTool
{
    public class Time
    {
        private Stopwatch _stopWatch;

        // Cap the fps's (currently set to 60 fps)
        public int TargetFps { get; set; } = 60;

        /// <summary>Time since the app started.</summary>
        private float AppTime { get; set; }

        /// <summary>Delta time.</summary>
        public float DeltaTime { get; private set; }

        private float _prevElapsed;

        private long _sleepTime;
        private long _nextTick = Environment.TickCount;

        private const int _secondAsMillisecond = 1000;
        private int _timeInSecElapsed;
        private int _currentFrame;
        private int _currentFPS;
        public int FPS => _currentFPS;
        public static float deltaTime { get; private set; }
        public static float time { get; private set; }
        internal Time()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        internal void Update()
        {
            var elapsed = _stopWatch.ElapsedMilliseconds / (float)_secondAsMillisecond;
            var deltaTime = elapsed - _prevElapsed;

            AppTime += deltaTime;
            DeltaTime = deltaTime;

            _prevElapsed = elapsed;

            if(TargetFps > 0)
            {
                _nextTick += _secondAsMillisecond / TargetFps;
                _sleepTime = _nextTick - Environment.TickCount;

                // Sleep the thread to adjust the time to the caped FPS's.
                if (_sleepTime >= 0)
                {
                    Thread.Sleep((int)_sleepTime);
                }
            }
         

            _currentFrame++;

            if (_timeInSecElapsed != (int)MathF.Round(AppTime))
            {
                _timeInSecElapsed = (int)MathF.Round(AppTime);

                _currentFPS = _currentFrame;

                _currentFrame = 0;
            }

            deltaTime = DeltaTime;
            time = AppTime;
        }
    }
}