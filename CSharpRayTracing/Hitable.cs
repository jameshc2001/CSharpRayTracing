using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRayTracing
{
    class Hitable
    {
        public Material material;

        public virtual bool hit(Ray r, float tMin, float tMax, ref HitRecord rec) { return false; }
    }

    struct HitRecord
    {
        public float t;
        public Vector3 p;
        public Vector3 normal;
        public Material material;

        public override string ToString()
        {
            return "t: " + t + " p: " + p + " normal: " + normal;
        }
    }
}
