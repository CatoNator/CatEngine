using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

/*
 * this converter only works with blender!!!!
 * it's designed around blender's mesh naming conventions (as of v2.8)
 * I don't really care about compatibility that much right now, as the engine is only designed for one specific game project using a specific set of tools
*/

/*TO DO:
 * precalculate whether the poly is a wall or floor or ceiling based on the polygon's normal's y-component
 * split the polygons to several cells by their location
*/

namespace AbsoluteMapMan
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Name the model to load:");
            string name = Console.ReadLine();
            ReadVertices(name);
        }

        private static void ReadVertices(string fileName)
        {
            string realName = fileName+".dae";
            if (File.Exists(realName))
            {
                XDocument file;

                string xmlText = File.ReadAllText(realName);

                //fixing the stupid collada shit
                //xml.linq doesn't know how to parse a root element with attributes, so we just replace the element with attributes with one that doesn't have any
                xmlText = xmlText.Replace("<COLLADA xmlns=\"http://www.collada.org/2005/11/COLLADASchema\" version=\"1.4.1\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">", "<COLLADA>");

                //parsing the string...
                file = XDocument.Parse(xmlText);

                Console.WriteLine("open sesame");

                //looping through the geometry files
                foreach (XElement mesh in file.Descendants("geometry"))
                {
                    string meshId = mesh.Attribute("id").Value;
                    Console.WriteLine("mesh with id " + meshId);

                    List<int> DrawOrder = new List<int>();

                    //the "triangles" element has the index drawing order. we collect this data
                    foreach (XElement tri in mesh.Descendants("triangles"))
                    {
                        //let's split the value into an array
                        string[] triValues = tri.Element("p").Value.Split(" ");

                        Console.WriteLine("mesh with id " + meshId + " triangledata " + triValues.Length);

                        //every third value is the index which to draw, we only need that
                        for (int i = 0; i < triValues.Length; i += 3)
                        {
                            int val = Int32.Parse(triValues[i]);
                            DrawOrder.Add(val);

                            Console.WriteLine(val.ToString());
                        }
                    }

                    //armed with the drawing order, we look for a float_array
                    foreach (XElement floatArr in mesh.Descendants("float_array"))
                    {
                        String floatArrName = floatArr.Attribute("id").Value;

                        //it has to be a positions array
                        if (floatArrName.Contains("positions"))
                        {
                            //splitting the string into bits
                            String[] floatArrSplit = floatArr.Value.Split(" ");

                            //lol
                            Console.WriteLine(floatArrName + " size " + floatArrSplit.Length);

                            List<Vector3> vertices = new List<Vector3>();

                            //generating vectors
                            for (int i = 0; i < floatArrSplit.Length; i += 3)
                            {
                                double val1 = Double.Parse(floatArrSplit[i]);
                                double val2 = Double.Parse(floatArrSplit[i + 1]);
                                double val3 = Double.Parse(floatArrSplit[i + 2]);
                                Vector3 vec = new Vector3(val1, val2, val3);
                                vertices.Add(vec);

                                Console.WriteLine(vec.ToString());
                            }

                            //creating a list of the vectors in drawing order
                            List<Vector3> verticesInOrder = new List<Vector3>();
                            Console.WriteLine("sorting the vector3 list");

                            //we sort the vectors using the drawing order list
                            foreach (int i in DrawOrder)
                            {
                                Vector3 curr;
                                if (i < vertices.Count)
                                {
                                    curr = vertices[i];
                                }
                                else //error! the vertex we want to draw now doesn't exist
                                {
                                    curr = vertices[vertices.Count - 1];
                                }

                                verticesInOrder.Add(curr);

                                Console.WriteLine(curr.ToString());
                            }

                            //saving the vertexdata
                            using (FileStream stream = new FileStream(fileName+"_"+meshId+".bin", FileMode.Create))
                            {
                                using (BinaryWriter writer = new BinaryWriter(stream))
                                {
                                    int l = verticesInOrder.Count;
                                    Console.WriteLine(l.ToString());

                                    writer.Write(l);

                                    foreach (Vector3 vec in verticesInOrder)
                                    {
                                        writer.Write(vec.X);
                                        writer.Write(vec.Y);
                                        writer.Write(vec.Z);
                                        //Console.WriteLine(val.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}