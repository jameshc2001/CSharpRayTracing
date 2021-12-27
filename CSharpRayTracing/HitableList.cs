using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRayTracing
{
    class HitableList : Hitable
    {
        private List<Hitable> world = new List<Hitable>();

        public HitableList() { }
        public HitableList(List<Hitable> world) { this.world = world; }

        public override bool hit(Ray r, float tMin, float tMax, ref HitRecord rec) //find closest hit
        {
            HitRecord tempRec = new HitRecord();
            bool hitAnything = false;
            float closestSoFar = tMax;
            for (int i = 0; i < world.Count; i++)
            {
                if (world[i].hit(r, tMin, closestSoFar, ref tempRec))
                {
                    hitAnything = true;
                    closestSoFar = tempRec.t;
                    rec = tempRec;
                    rec.material = world[i].material;
                }
            }
            return hitAnything;
        }
    }
}
