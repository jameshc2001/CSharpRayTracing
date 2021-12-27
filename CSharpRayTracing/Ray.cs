using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRayTracing
{
    //rays are described by the equation p(t) = A + t*B
    //A is the origin
    //B is the direction
    //t moves the point p along the ray

    class Ray
    {
        private Vector3 A;
        private Vector3 B;

        public Ray() { }
        public Ray(Vector3 A, Vector3 B)
        {
            this.A = A;
            this.B = B;
        }

        public Vector3 origin() { return A; }
        public Vector3 direction() { return B; }
        public Vector3 pointAtParameter(float t) { return A + t * B; }
    }
}
