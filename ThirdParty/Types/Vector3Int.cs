using GlmNet;

namespace MinoTool
{
    public struct Vector3Int
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public int sqrMagnitude => x * x + y * y + z * z;

        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(Vector3Int v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator -(vec3 a, Vector3Int b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static bool operator ==(Vector3Int lhs, Vector3Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(Vector3Int lhs, Vector3Int rhs)
        {
            return !(lhs == rhs);
        }
    }
}