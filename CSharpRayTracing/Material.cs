using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRayTracing
{
    class Material
    {
        public Random random = new Random();
        public virtual bool scatter(ref Ray rIn, ref HitRecord rec, ref Vector3 attenuation, ref Ray scattered) { return false; }

        public Vector3 randomInUnitSphere()
        {
            Vector3 p = new Vector3();
            do
            {
                p = 2.0f * new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()) - new Vector3(1f, 1f, 1f);
            } while (p.squaredLength() >= 1.0f);
            return p;
        }

        public Vector3 reflect(Vector3 vec, Vector3 normal)
        {
            return vec - 2 * Vector3.dot(vec, normal) * normal;
        }

        public bool refract(Vector3 v, Vector3 n, float niOverNt, ref Vector3 refracted) //snells law
        {
            Vector3 uv = Vector3.unitVector(v);
            float dt = Vector3.dot(uv, n);
            float discriminant = 1.0f - niOverNt * niOverNt * (1f - dt * dt);
            if (discriminant > 0)
            {
                refracted = niOverNt * (uv - n * dt) - n * (float)Math.Sqrt(discriminant);
                return true;
            }
            else return false;
        }

        public float schlick(float cosine, float ri) //causes dielectrics to be more reflective when viewed from a steep angle
        {
            float r0 = (1f - ri) / (1f + ri);
            r0 = r0 * r0;
            return r0 + (1f - r0) * (float)Math.Pow((1f - cosine), 5f);
        }
    }

    class Lambertian : Material
    {
        private Vector3 albedo;
        public Lambertian(Vector3 albedo) { this.albedo = albedo; }

        public override bool scatter(ref Ray rIn, ref HitRecord rec, ref Vector3 attenuation, ref Ray scattered)
        {
            Vector3 target = rec.p + rec.normal + randomInUnitSphere(); //this generates a random reflection vector
            scattered = new Ray(rec.p, target - rec.p);
            attenuation = albedo;
            return true;
        }
    }

    class Metal : Material
    {
        private Vector3 albedo;
        private float fuzz;
        public Metal(Vector3 albedo, float fuzz)
        {
            this.albedo = albedo;
            if (fuzz < 1) this.fuzz = fuzz;
            else this.fuzz = 1;
        }

        public override bool scatter(ref Ray rIn, ref HitRecord rec, ref Vector3 attenuation, ref Ray scattered)
        {
            Vector3 reflected = reflect(Vector3.unitVector(rIn.direction()), rec.normal);
            scattered = new Ray(rec.p, reflected + fuzz * randomInUnitSphere());
            attenuation = albedo;
            return (Vector3.dot(scattered.direction(), rec.normal) > 0);
        }
    }

    class Dielectric : Material
    {
        private float ri; //refractive index
        public Dielectric(float refractiveIndex) { ri = refractiveIndex; }

        public override bool scatter(ref Ray rIn, ref HitRecord rec, ref Vector3 attenuation, ref Ray scattered)
        {
            Vector3 outwardNormal = new Vector3();
            Vector3 reflected = reflect(rIn.direction(), rec.normal);
            float niOverNt;
            attenuation = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 refracted = new Vector3();
            float reflectProb;
            float cosine;

            if (Vector3.dot(rIn.direction(), rec.normal) > 0)
            {
                outwardNormal = rec.normal.Negative();
                niOverNt = ri;
                cosine = ri * Vector3.dot(rIn.direction(), rec.normal) / rIn.direction().length();
            }
            else
            {
                outwardNormal = rec.normal;
                niOverNt = 1.0f / ri;
                cosine = -Vector3.dot(rIn.direction(), rec.normal) / rIn.direction().length();
            }

            if (refract(rIn.direction(), outwardNormal, niOverNt, ref refracted))
            {
                reflectProb = schlick(cosine, ri);
            }
            else
            {
                reflectProb = 1.0f;
            }

            if (random.NextDouble() < reflectProb)
            {
                scattered = new Ray(rec.p, reflected);
            }
            else
            {
                scattered = new Ray(rec.p, refracted);
            }

            return true;
        }
    }
}
