using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace AbsoluteMapMan
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadVertices("cube");
        }

        private static void ReadVertices(string fileName)
        {
            string xmlText = System.IO.File.ReadAllText(fileName+".dae");
            XDocument file = XDocument.Parse(xmlText);

            Console.WriteLine("open sesame");

            foreach (XElement floatArr in file.Descendants("float_array"))
            {
                String floatArrName = floatArr.Attribute("id").Value;
                Console.WriteLine("found float_array with id " + floatArrName);

                if (floatArrName.Contains("map-0"))
                {
                    String floatArrValues = floatArr.Value;
                    String[] floatArrSplit = floatArrValues.Split(" ");

                    int vl = (floatArrSplit.Length / 3);

                    Console.WriteLine("float_array size " + floatArrSplit.Length + " vector3 "+vl);

                    List<Vector3> vecList = new List<Vector3>();

                    for (int i = 0; i < vl; i++)
                    {
                        double val1 = Double.Parse(floatArrSplit[i]);
                        double val2 = Double.Parse(floatArrSplit[i+1]);
                        double val3 = Double.Parse(floatArrSplit[i+2]);
                        Vector3 vec = new Vector3(val1, val2, val3);
                        vecList.Add(vec);

                        Console.WriteLine(vec.ToString());
                    }

                    using (FileStream stream = new FileStream(fileName+".bin", FileMode.Create))
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            writer.Write(floatArrSplit.Length);

                            int l = floatArrSplit.Length;

                            for (int i = 0; i < l; i++)
                            {
                                double val = Double.Parse(floatArrSplit[i]);
                                writer.Write(val);
                                Console.WriteLine(val.ToString());
                            }
                        }
                    }
                }
            }

            //loading walls
            /*foreach (XElement geoLib in file.Descendants("library_geometries"))
            {
                Console.WriteLine("found library_geometries");
                foreach (XElement geo in file.Descendants("geometry"))
                {
                    String geoName = geo.Attribute("id").Value;
                    Console.WriteLine("found geometry with id "+geoName);

                    foreach (XElement mesh in file.Descendants("mesh"))
                    {
                        Console.WriteLine("found mesh");
                        foreach (XElement source in file.Descendants("source"))
                        {
                            String sourceName = source.Attribute("id").Value;
                            Console.WriteLine("found source with id " + sourceName);

                            foreach (XElement floatArr in file.Descendants("float_array"))
                            {
                                String floatArrName = floatArr.Attribute("id").Value;
                                Console.WriteLine("found float_array with id " + floatArrName);
                                String floatArrValues = floatArr.Value;
                                String[] floatArrSplit = floatArrValues.Split(" ");
                                Console.WriteLine("float_array size "+floatArrSplit.Length);
                            }
                        }
                    }
                }
            }*/
        }
    }
}
