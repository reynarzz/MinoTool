// UnityEngine.Quaternion
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MinoTool
{

    /// <summary>
    ///   <para>Quaternions are used to represent rotations.</para>
    /// </summary>
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <summary>
        ///   <para>X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
        /// </summary>
        public float x;

        /// <summary>
        ///   <para>Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
        /// </summary>
        public float y;

        /// <summary>
        ///   <para>Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
        /// </summary>
        public float z;

        /// <summary>
        ///   <para>W component of the Quaternion. Do not directly modify quaternions.</para>
        /// </summary>
        public float w;

        private static readonly Quaternion identityQuaternion = new Quaternion(0f, 0f, 0f, 1f);

        public const float kEpsilon = 1E-06f;

        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    3 => w,
                    _ => throw new IndexOutOfRangeException("Invalid Quaternion index!"),
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
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        /// <summary>
        ///   <para>The identity rotation (Read Only).</para>
        /// </summary>
        public static Quaternion identity => identityQuaternion;

        /// <summary>
        ///   <para>Returns or sets the euler angle representation of the rotation.</para>
        /// </summary>
        public Vector3 eulerAngles
        {
            get
            {
                return Internal_MakePositive(Internal_ToEulerRad(this) * 57.29578f);
            }
            set
            {
                this = Internal_FromEulerRad(value * ((float)Math.PI / 180f));
            }
        }

        /// <summary>
        ///   <para>Returns this quaternion with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public Quaternion normalized => Normalize(this);

        /// <summary>
        ///   <para>Constructs new Quaternion with given x,y,z,w components.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        ///   <para>Creates a rotation which rotates from fromDirection to toDirection.</para>
        /// </summary>
        /// <param name="fromDirection"></param>
        /// <param name="toDirection"></param>
        public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            FromToRotation_Injected(ref fromDirection, ref toDirection, out var ret);
            return ret;
        }

        /// <summary>
        ///   <para>Returns the Inverse of rotation.</para>
        /// </summary>
        /// <param name="rotation"></param>
        public static Quaternion Inverse(Quaternion rotation)
        {
            Inverse_Injected(ref rotation, out var ret);
            return ret;
        }

        /// <summary>
        ///   <para>Spherically interpolates between a and b by t. The parameter t is clamped to the range [0, 1].</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
        {
            Slerp_Injected(ref a, ref b, t, out var ret);
            return ret;
        }

        /// <summary>
        ///   <para>Spherically interpolates between a and b by t. The parameter t is not clamped.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            SlerpUnclamped_Injected(ref a, ref b, t, out var ret);
            return ret;
        }

        /// <summary>
        ///   <para>Interpolates between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1].</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
        {
            Lerp_Injected(ref a, ref b, t, out var ret);
            return ret;
        }

        /// <summary>
        ///   <para>Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            LerpUnclamped_Injected(ref a, ref b, t, out var ret);
            return ret;
        }

        private static Quaternion Internal_FromEulerRad(Vector3 euler)
        {
            Internal_FromEulerRad_Injected(ref euler, out var ret);
            return ret;
        }

        private static Vector3 Internal_ToEulerRad(Quaternion rotation)
        {
            Internal_ToEulerRad_Injected(ref rotation, out var ret);
            return ret;
        }

        private static void Internal_ToAxisAngleRad(Quaternion q, out Vector3 axis, out float angle)
        {
            Internal_ToAxisAngleRad_Injected(ref q, out axis, out angle);
        }

        /// <summary>
        ///   <para>Creates a rotation which rotates angle degrees around axis.</para>
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        public static Quaternion AngleAxis(float angle, Vector3 axis)
        {
            AngleAxis_Injected(angle, ref axis, out var ret);
            return ret;
        }

        /// <summary>
        ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
        /// </summary>
        /// <param name="forward">The direction to look in.</param>
        /// <param name="upwards">The vector that defines in which direction up is.</param>
        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
        {
            LookRotation_Injected(ref forward, ref upwards, out var ret);
            return ret;
        }

        /// <summary>
        ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
        /// </summary>
        /// <param name="forward">The direction to look in.</param>
        /// <param name="upwards">The vector that defines in which direction up is.</param>
        public static Quaternion LookRotation(Vector3 forward)
        {
            return LookRotation(forward, Vector3.up);
        }

        /// <summary>
        ///   <para>Set x, y, z and w components of an existing Quaternion.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        /// <param name="newW"></param>
        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            float num = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num4 = rotation.x * num;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vector3 result = default(Vector3);
            result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
            result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
            result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
            return result;
        }

        private static bool IsEqualUsingDot(float dot)
        {
            return dot > 0.999999f;
        }

        public static bool operator ==(Quaternion lhs, Quaternion rhs)
        {
            return IsEqualUsingDot(Dot(lhs, rhs));
        }

        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///   <para>The dot product between two rotations.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        /// <summary>
        ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
        /// </summary>
        /// <param name="view">The direction to look in.</param>
        /// <param name="up">The vector that defines in which direction up is.</param>
        public void SetLookRotation(Vector3 view)
        {
            Vector3 up = Vector3.up;
            SetLookRotation(view, up);
        }

        /// <summary>
        ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
        /// </summary>
        /// <param name="view">The direction to look in.</param>
        /// <param name="up">The vector that defines in which direction up is.</param>
        public void SetLookRotation(Vector3 view, Vector3 up)
        {
            this = LookRotation(view, up);
        }

        /// <summary>
        ///   <para>Returns the angle in degrees between two rotations a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Angle(Quaternion a, Quaternion b)
        {
            float num = Dot(a, b);
            return (!IsEqualUsingDot(num)) ? (Mathf.Acos(Mathf.Min(Mathf.Abs(num), 1f)) * 2f * 57.29578f) : 0f;
        }

        private static Vector3 Internal_MakePositive(Vector3 euler)
        {
            float num = -0.005729578f;
            float num2 = 360f + num;
            if (euler.x < num)
            {
                euler.x += 360f;
            }
            else if (euler.x > num2)
            {
                euler.x -= 360f;
            }
            if (euler.y < num)
            {
                euler.y += 360f;
            }
            else if (euler.y > num2)
            {
                euler.y -= 360f;
            }
            if (euler.z < num)
            {
                euler.z += 360f;
            }
            else if (euler.z > num2)
            {
                euler.z -= 360f;
            }
            return euler;
        }

        /// <summary>
        ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static Quaternion Euler(float x, float y, float z)
        {
            return Internal_FromEulerRad(new Vector3(x, y, z) * ((float)Math.PI / 180f));
        }

        /// <summary>
        ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis.</para>
        /// </summary>
        /// <param name="euler"></param>
        public static Quaternion Euler(Vector3 euler)
        {
            return Internal_FromEulerRad(euler * ((float)Math.PI / 180f));
        }

        public void ToAngleAxis(out float angle, out Vector3 axis)
        {
            Internal_ToAxisAngleRad(this, out axis, out angle);
            angle *= 57.29578f;
        }

        /// <summary>
        ///   <para>Creates a rotation which rotates from fromDirection to toDirection.</para>
        /// </summary>
        /// <param name="fromDirection"></param>
        /// <param name="toDirection"></param>
        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            this = FromToRotation(fromDirection, toDirection);
        }

        /// <summary>
        ///   <para>Rotates a rotation from towards to.</para>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="maxDegreesDelta"></param>
        public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            float num = Angle(from, to);
            if (num == 0f)
            {
                return to;
            }
            return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / num));
        }

        /// <summary>
        ///   <para>Converts this quaternion to one with the same orientation but with a magnitude of 1.</para>
        /// </summary>
        /// <param name="q"></param>
        public static Quaternion Normalize(Quaternion q)
        {
            float num = Mathf.Sqrt(Dot(q, q));
            if (num < Mathf.Epsilon)
            {
                return identity;
            }
            return new Quaternion(q.x / num, q.y / num, q.z / num, q.w / num);
        }

        public void Normalize()
        {
            this = Normalize(this);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }

        public override bool Equals(object other)
        {
            if (!(other is Quaternion))
            {
                return false;
            }
            return Equals((Quaternion)other);
        }

        public bool Equals(Quaternion other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string of the Quaternion.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", x, y, z, w);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string of the Quaternion.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2}, {3})", x.ToString(format, CultureInfo.InvariantCulture.NumberFormat), y.ToString(format, CultureInfo.InvariantCulture.NumberFormat), z.ToString(format, CultureInfo.InvariantCulture.NumberFormat), w.ToString(format, CultureInfo.InvariantCulture.NumberFormat));
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public static Quaternion EulerRotation(float x, float y, float z)
        {
            return Internal_FromEulerRad(new Vector3(x, y, z));
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public static Quaternion EulerRotation(Vector3 euler)
        {
            return Internal_FromEulerRad(euler);
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public void SetEulerRotation(float x, float y, float z)
        {
            this = Internal_FromEulerRad(new Vector3(x, y, z));
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public void SetEulerRotation(Vector3 euler)
        {
            this = Internal_FromEulerRad(euler);
        }

        [Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees.")]
        public Vector3 ToEuler()
        {
            return Internal_ToEulerRad(this);
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public static Quaternion EulerAngles(float x, float y, float z)
        {
            return Internal_FromEulerRad(new Vector3(x, y, z));
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public static Quaternion EulerAngles(Vector3 euler)
        {
            return Internal_FromEulerRad(euler);
        }

        [Obsolete("Use Quaternion.ToAngleAxis instead. This function was deprecated because it uses radians instead of degrees.")]
        public void ToAxisAngle(out Vector3 axis, out float angle)
        {
            Internal_ToAxisAngleRad(this, out axis, out angle);
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public void SetEulerAngles(float x, float y, float z)
        {
            SetEulerRotation(new Vector3(x, y, z));
        }

        [Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
        public void SetEulerAngles(Vector3 euler)
        {
            this = EulerRotation(euler);
        }

        [Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees.")]
        public static Vector3 ToEulerAngles(Quaternion rotation)
        {
            return Internal_ToEulerRad(rotation);
        }

        [Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees.")]
        public Vector3 ToEulerAngles()
        {
            return Internal_ToEulerRad(this);
        }

        [Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees.")]
        public void SetAxisAngle(Vector3 axis, float angle)
        {
            this = AxisAngle(axis, angle);
        }

        [Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
        public static Quaternion AxisAngle(Vector3 axis, float angle)
        {
            return AngleAxis(57.29578f * angle, axis);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void FromToRotation_Injected(ref Vector3 fromDirection, ref Vector3 toDirection, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Inverse_Injected(ref Quaternion rotation, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Slerp_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void SlerpUnclamped_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Lerp_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void LerpUnclamped_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_FromEulerRad_Injected(ref Vector3 euler, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_ToEulerRad_Injected(ref Quaternion rotation, out Vector3 ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_ToAxisAngleRad_Injected(ref Quaternion q, out Vector3 axis, out float angle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void AngleAxis_Injected(float angle, ref Vector3 axis, out Quaternion ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void LookRotation_Injected(ref Vector3 forward, ref Vector3 upwards, out Quaternion ret);
    }
}
