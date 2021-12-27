using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRayTracing
{
    class Camera
    {
        private Vector3 lowerLeftCorner = new Vector3();
        private Vector3 horizontal = new Vector3();
        private Vector3 vertical = new Vector3();
        private Vector3 origin = new Vector3();

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 vUp, float vFov, float aspect)
        {
            Vector3 u, v, w;
            float theta = vFov * (float)Math.PI / 180;
            float halfHeight = (float)Math.Tan(theta/2);
            float halfWidth = aspect * halfHeight;
            origin = lookFrom;
            w = Vector3.unitVector(lookFrom - lookAt);
            u = Vector3.unitVector(Vector3.cross(vUp, w));
            v = Vector3.cross(w, u);
            lowerLeftCorner = origin - halfWidth * u - halfHeight * v - w;
            horizontal = 2 * halfWidth * u;
            vertical = 2 * halfHeight * v;
        }

        public Ray getRay(float u, float v) { return new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin); }
    }
}
