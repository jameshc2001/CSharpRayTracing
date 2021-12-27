using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRayTracing
{
    class Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        //Vector3 will also be used for storing rgb values
        public float r() { return x; }
        public float g() { return y; }
        public float b() { return z; }

        public Vector3 Negative() { return new Vector3(-x, -y, -z); }

        public float length() { return (float)Math.Sqrt(x * x + y * y + z * z); }
        public float squaredLength() { return x * x + y * y + z * z; }

        public void makeUnitVector()
        {
            float k = 1.0f / (float)Math.Sqrt(x * x + y * y + z * z);
            x *= k;
            y *= k;
            z *= k;
        }

        public static Vector3 operator+(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator-(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator*(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 operator/(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static Vector3 operator*(Vector3 v, float t)
        {
            return new Vector3(v.x * t, v.y * t, v.z * t);
        }

        public static Vector3 operator*(float t, Vector3 v)
        {
            return new Vector3(v.x * t, v.y * t, v.z * t);
        }

        public static Vector3 operator/(Vector3 v, float t)
        {
            return new Vector3(v.x / t, v.y / t, v.z / t);
        }

        public static float dot(Vector3 v1, Vector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Vector3 cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.y * v2.z - v1.z * v2.y, -(v1.x * v2.z - v1.z * v2.x), v1.x * v2.y - v1.y * v2.x);
        }

        public static Vector3 unitVector(Vector3 v)
        {
            return v / v.length();
        }

        public override string ToString()
        {
            return "x: " + x + " y: " + y + " z: " + z;
        }
    }
}
