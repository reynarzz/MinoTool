// UnityEngine.Vector2
using System;
using System.Globalization;

namespace MinoTool
{

    /// <summary>
    ///   <para>Representation of 2D vectors and points.</para>
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        ///   <para>X component of the vector.</para>
        /// </summary>
        public float x;

        /// <summary>
        ///   <para>Y component of the vector.</para>
        /// </summary>
        public float y;

        private static readonly Vector2 zeroVector = new Vector2(0f, 0f);

        private static readonly Vector2 oneVector = new Vector2(1f, 1f);

        private static readonly Vector2 upVector = new Vector2(0f, 1f);

        private static readonly Vector2 downVector = new Vector2(0f, -1f);

        private static readonly Vector2 leftVector = new Vector2(-1f, 0f);

        private static readonly Vector2 rightVector = new Vector2(1f, 0f);

        private static readonly Vector2 positiveInfinityVector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

        private static readonly Vector2 negativeInfinityVector = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

        public const float kEpsilon = 1E-05f;

        public const float kEpsilonNormalSqrt = 1E-15f;

        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException("Invalid Vector2 index!"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        /// <summary>
        ///   <para>Returns this vector with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public Vector2 normalized
        {
            get
            {
                Vector2 result = new Vector2(x, y);
                result.Normalize();
                return result;
            }
        }

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float magnitude => (float)Math.Sqrt(x * x + y * y);

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public float sqrMagnitude => x * x + y * y;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 0).</para>
        /// </summary>
        public static Vector2 zero => zeroVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 1).</para>
        /// </summary>
        public static Vector2 one => oneVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 1).</para>
        /// </summary>
        public static Vector2 up => upVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, -1).</para>
        /// </summary>
        public static Vector2 down => downVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(-1, 0).</para>
        /// </summary>
        public static Vector2 left => leftVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 0).</para>
        /// </summary>
        public static Vector2 right => rightVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).</para>
        /// </summary>
        public static Vector2 positiveInfinity => positiveInfinityVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).</para>
        /// </summary>
        public static Vector2 negativeInfinity => negativeInfinityVector;

        /// <summary>
        ///   <para>Constructs a new vector with given x, y components.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///   <para>Set x and y components of an existing Vector2.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void Set(float newX, float newY)
        {
            x = newX;
            y = newY;
        }

        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        /// <summary>
        ///   <para>Moves a point current towards target.</para>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = num * num + num2 * num2;
            if (num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta))
            {
                return target;
            }
            float num4 = (float)Math.Sqrt(num3);
            return new Vector2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
        }

        /// <summary>
        ///   <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static Vector2 Scale(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        /// <summary>
        ///   <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector2 scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        /// <summary>
        ///   <para>Makes this vector have a magnitude of 1.</para>
        /// </summary>
        public void Normalize()
        {
            float magnitude = this.magnitude;
            if (magnitude > 1E-05f)
            {
                this /= magnitude;
            }
            else
            {
                this = zero;
            }
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        //public override string ToString()
        //{
        //	return UnityString.Format("({0:F1}, {1:F1})", x, y);
        //}

        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        //public string ToString(string format)
        //{
        //	return UnityString.Format("({0}, {1})", x.ToString(format, CultureInfo.InvariantCulture.NumberFormat), y.ToString(format, CultureInfo.InvariantCulture.NumberFormat));
        //}

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        /// <summary>
        ///   <para>Returns true if the given vector is exactly equal to this vector.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            if (!(other is Vector2))
            {
                return false;
            }
            return Equals((Vector2)other);
        }

        public bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        /// <summary>
        ///   <para>Reflects a vector off the vector defined by a normal.</para>
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
        {
            float num = -2f * Dot(inNormal, inDirection);
            return new Vector2(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
        }

        /// <summary>
        ///   <para>Returns the 2D vector perpendicular to this 2D vector. The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.</para>
        /// </summary>
        /// <param name="inDirection">The input direction.</param>
        /// <returns>
        ///   <para>The perpendicular direction.</para>
        /// </returns>
        public static Vector2 Perpendicular(Vector2 inDirection)
        {
            return new Vector2(0f - inDirection.y, inDirection.x);
        }

        /// <summary>
        ///   <para>Dot Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static float Dot(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }

        /// <summary>
        ///   <para>Returns the unsigned angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        public static float Angle(Vector2 from, Vector2 to)
        {
            float num = (float)Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            if (num < 1E-15f)
            {
                return 0f;
            }
            float num2 = Mathf.Clamp(Dot(from, to) / num, -1f, 1f);
            return (float)Math.Acos(num2) * 57.29578f;
        }

        /// <summary>
        ///   <para>Returns the signed angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            float num = Angle(from, to);
            float num2 = Mathf.Sign(from.x * to.y - from.y * to.x);
            return num * num2;
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Distance(Vector2 a, Vector2 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            return (float)Math.Sqrt(num * num + num2 * num2);
        }

        /// <summary>
        ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            float sqrMagnitude = vector.sqrMagnitude;
            if (sqrMagnitude > maxLength * maxLength)
            {
                float num = (float)Math.Sqrt(sqrMagnitude);
                float num2 = vector.x / num;
                float num3 = vector.y / num;
                return new Vector2(num2 * maxLength, num3 * maxLength);
            }
            return vector;
        }

        public static float SqrMagnitude(Vector2 a)
        {
            return a.x * a.x + a.y * a.y;
        }

        public float SqrMagnitude()
        {
            return x * x + y * y;
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector2 Min(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector2 Max(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
        }

        public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
        {
            smoothTime = Mathf.Max(0.0001f, smoothTime);
            float num = 2f / smoothTime;
            float num2 = num * deltaTime;
            float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
            float num4 = current.x - target.x;
            float num5 = current.y - target.y;
            Vector2 vector = target;
            float num6 = maxSpeed * smoothTime;
            float num7 = num6 * num6;
            float num8 = num4 * num4 + num5 * num5;
            if (num8 > num7)
            {
                float num9 = (float)Math.Sqrt(num8);
                num4 = num4 / num9 * num6;
                num5 = num5 / num9 * num6;
            }
            target.x = current.x - num4;
            target.y = current.y - num5;
            float num10 = (currentVelocity.x + num * num4) * deltaTime;
            float num11 = (currentVelocity.y + num * num5) * deltaTime;
            currentVelocity.x = (currentVelocity.x - num * num10) * num3;
            currentVelocity.y = (currentVelocity.y - num * num11) * num3;
            float num12 = target.x + (num4 + num10) * num3;
            float num13 = target.y + (num5 + num11) * num3;
            float num14 = vector.x - current.x;
            float num15 = vector.y - current.y;
            float num16 = num12 - vector.x;
            float num17 = num13 - vector.y;
            if (num14 * num16 + num15 * num17 > 0f)
            {
                num12 = vector.x;
                num13 = vector.y;
                currentVelocity.x = (num12 - vector.x) / deltaTime;
                currentVelocity.y = (num13 - vector.y) / deltaTime;
            }
            return new Vector2(num12, num13);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(0f - a.x, 0f - a.y);
        }

        public static Vector2 operator *(Vector2 a, float d)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator *(float d, Vector2 a)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator /(Vector2 a, float d)
        {
            return new Vector2(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            float num = lhs.x - rhs.x;
            float num2 = lhs.y - rhs.y;
            return num * num + num2 * num2 < 9.99999944E-11f;
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator Vector3(Vector2 v)
        {
            return new Vector3(v.x, v.y, 0f);
        }
    }
}
