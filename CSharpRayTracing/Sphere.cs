using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRayTracing
{
    class Sphere : Hitable
    {
        public Vector3 centre;
        public float radius;

        public Sphere() { }
        public Sphere(Vector3 centre, float radius, Material material)
        {
            this.centre = centre;
            this.radius = radius;
            this.material = material;
        }

        public override bool hit(Ray r, float tMin, float tMax, ref HitRecord rec)
        {
            Vector3 oc = r.origin() - centre;
            float a = Vector3.dot(r.direction(), r.direction());
            float b = Vector3.dot(oc, r.direction());
            float c = Vector3.dot(oc, oc) - radius * radius;
            float discriminant = b * b - a * c; //find out if it has any real roots
            if (discriminant > 0)
            {
                //if there are roots solve for both values of t
                float temp = (-b - (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.t = temp;
                    rec.p = r.pointAtParameter(rec.t);
                    rec.normal = (rec.p - centre) / radius;
                    return true;
                }
                temp = (-b + (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.t = temp;
                    rec.p = r.pointAtParameter(rec.t);
                    rec.normal = (rec.p - centre) / radius;
                    return true;
                }
            }
            return false;
        }
    }
}
