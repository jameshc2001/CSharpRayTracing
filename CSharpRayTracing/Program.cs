using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace CSharpRayTracing
{
    class Program
    {
        static Random random = new Random();
        static DateTime start = DateTime.Now;

        static void Main(string[] args)
        {
            int counter = 0; //this will be used to calculating percentage done

            //first setup the file that the render will be stored in
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "render.ppm");
            writer.WriteLine("P3");
            int x = 1280;
            int y = 720;
            writer.WriteLine(x + " " + y);
            writer.WriteLine(255);

            //setup the world
            //List<Hitable> hitables = new List<Hitable>();
            //hitables.Add(new Sphere(new Vector3(0f, 0f, -1f), 0.5f, new Lambertian(new Vector3(0.1f, 0.2f, 0.5f))));
            //hitables.Add(new Sphere(new Vector3(0f, -100.5f, -1f), 100f, new Lambertian(new Vector3(0.8f, 0.8f, 0f))));
            //hitables.Add(new Sphere(new Vector3(1f, 0f, -1f), 0.5f, new Metal(new Vector3(1f, 1f, 1f), 1f)));
            //hitables.Add(new Sphere(new Vector3(-1f, 0f, -1f), 0.5f, new Dielectric(1.5f)));
            //hitables.Add(new Sphere(new Vector3(-1f, 0f, -1f), -0.45f, new Dielectric(1.5f)));
            //HitableList world = new HitableList(hitables);
            HitableList world = new HitableList(randomWorld());

            //setup the camera
            Vector3 lookFrom = new Vector3(13, 2, 3);
            Vector3 lookAt = new Vector3(0, 0, 0);
            Camera cam = new Camera(lookFrom, lookAt, new Vector3(0,1,0), 20, (float)x / y);

            //now loop through each pixel, calculating each ones colour
            int ns = 10; //this is the number of samples per pixel (basically rays per pixel)
            for (int j = y - 1; j >= 0; j--)
            {
                for (int i = 0; i < x; i++)
                {
                    Vector3 col = new Vector3(0, 0, 0);
                    for (int s = 0; s < ns; s++)
                    {
                        float u = (i + (float)random.NextDouble()) / x;
                        float v = (j + (float)random.NextDouble()) / y;
                        Ray r = cam.getRay(u, v);
                        col = col + colour(r, world, 0);
                    }
                    col = col / ns; //take an average of all the rays that were just calculated in the s loop
                    col = new Vector3((float)Math.Sqrt(col.r()), (float)Math.Sqrt(col.g()), (float)Math.Sqrt(col.b())); //gamma correct the image
                    int ir = (int)(255.99 * col.r());
                    int ig = (int)(255.99 * col.g());
                    int ib = (int)(255.99 * col.b());
                    writer.WriteLine(ir + " " + ig + " " + ib);

                    //calculate percentage
                    counter++;
                    Console.Write("\r{0}%       ", (float)counter / (x * y) * 100);
                }
            }

            writer.Close();
            TimeSpan elapsedTime = DateTime.Now - start;
            Console.WriteLine("Render completed");
            Console.WriteLine("Elapsed time: " + elapsedTime.Hours + " Hours, " + elapsedTime.Minutes + " Minutes and " + elapsedTime.Seconds + " Seconds");
            Console.ReadLine();
        }

        static Vector3 colour(Ray r, HitableList world, int depth)
        {
            HitRecord rec = new HitRecord();
            if (world.hit(r, 0.001f, float.MaxValue, ref rec))
            {
                Ray scattered = new Ray();
                Vector3 attenuation = new Vector3();
                if (depth < 50 && rec.material.scatter(ref r, ref rec, ref attenuation, ref scattered))
                {
                    return attenuation * colour(scattered, world, depth + 1);
                }
                else
                {
                    return new Vector3(0,0,0);
                }
            }
            else
            {
                Vector3 unitDirection = Vector3.unitVector(r.direction());
                float t = 0.5f * (unitDirection.y + 1.0f);
                return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
            }
        }

        public static List<Hitable> randomWorld()
        {
            List<Hitable> world = new List<Hitable>();
            world.Add(new Sphere(new Vector3(0f, -1000f, 0f), 1000f, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f))));
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    float chooseMat = (float)random.NextDouble();
                    Vector3 centre = new Vector3(a + 0.9f * (float)random.NextDouble(), 0.2f, b + 0.9f * (float)random.NextDouble());
                    if ((centre - new Vector3(4f, 0.2f, 0f)).length() > 0.9f)
                    {
                        if (chooseMat < 0.8f)
                        {
                            world.Add(new Sphere(centre, 0.2f, new Lambertian(new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()))));
                        }
                        else if (chooseMat < 0.95)
                        {
                            world.Add(new Sphere(centre, 0.2f, new Metal(new Vector3(0.5f * (1f + (float)random.NextDouble()), 0.5f * (1f + (float)random.NextDouble()), 0.5f * (1f + (float)random.NextDouble())), 0.5f * (float)random.NextDouble())));
                        }
                        else
                        {
                            world.Add(new Sphere(centre, 0.2f, new Dielectric(1.5f)));
                        }
                    }
                }
            }
            world.Add(new Sphere(new Vector3(0f, 1f, 0f), 1.0f, new Dielectric(1.5f)));
            world.Add(new Sphere(new Vector3(-4f, 1f, 0f), 1.0f, new Lambertian(new Vector3(0.4f, 0.2f, 0.1f))));
            world.Add(new Sphere(new Vector3(4f, 1f, 0f), 1.0f, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f)));
            return world;
        }
    }
}
