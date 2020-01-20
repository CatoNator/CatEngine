using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace BankRobber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("b/u foldername/bankname");
            string input = Console.ReadLine();
            string[] param = input.Split(" ");

            if (param[0].Equals("b"))
                BuildBank(param[1]);
            else if (param[0].Equals("u"))
                UnBuildBank(param[1]);
            else
                Console.WriteLine("could not recognize command");
        }

        static void BuildBank(string folder)
        {
            string path = folder + "/";

            DirectoryInfo D = new DirectoryInfo(path);

            FileInfo[] files = D.GetFiles();

            int iHeaderSize = 44;

            using (FileStream stream = new FileStream(folder+".bnk", FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    int offset = 0;

                    List<Tuple<string, uint, uint>> dataList = new List<Tuple<string, uint, uint>>();

                    foreach (FileInfo f in files)
                    {
                        String soundName = f.Name.Substring(0, f.Name.Length - 4);

                        Console.WriteLine(f.FullName);

                        foreach (char c in soundName)
                        {
                            writer.Write((Byte)c);
                        }

                        //null terminator - end of filename, start of offset+lenght
                        writer.Write((Byte)0);

                        String soundSize = f.Length.ToString();

                        int l = ((int)f.Length-iHeaderSize); //we subtract the size of the wav header from the size of the file

                        writer.Write((UInt32)offset);
                        writer.Write((uint)l);

                        dataList.Add(new Tuple<string, uint, uint>(f.Name, (uint)offset, (uint)l));

                        offset += (int)l;

                        Console.WriteLine(soundName + " " + soundSize);
                    }

                    //end of files man!
                    writer.Write((Byte)'E');
                    writer.Write((Byte)'N');
                    writer.Write((Byte)'D');
                    writer.Write((Byte)0);


                    //we move on to rewriting the raw pcm to the bank
                    foreach (Tuple<string, uint, uint> t in dataList)
                    {
                        List<byte> byteArr = new List<byte>();

                        //the file
                        using (FileStream soundFile = new FileStream(path+t.Item1, FileMode.Open))
                        {
                            using (BinaryReader reader = new BinaryReader(soundFile))
                            {
                                Console.WriteLine("reading data from "+(path + t.Item1));

                                //now assuming we are dealing with a wav like we're supposed to, we just skip over the first 44 bytes because they're the header
                                for (int i = 0; i < iHeaderSize; i++)
                                {
                                    reader.ReadByte();
                                }

                                Console.WriteLine(t.Item3 + " v " + soundFile.Length);

                                //looping through the remaining bytes
                                for (int i = 0; i < t.Item3; i++)
                                {
                                    byte b = 0x00;

                                    try
                                    {
                                        b = reader.ReadByte();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("End of stream at " +i+"!, "+e.Message);
                                    }
                                    //Console.WriteLine(b.ToString());
                                    byteArr.Add(b);
                                }
                            }

                            for (int i = 0; i < byteArr.Count; i++)
                            {
                                writer.Write(byteArr[i]);
                            }
                        }
                    }
                }
            }
        }

        static void UnBuildBank(string bankname)
        {
            string bank = bankname + ".bnk";

            if (File.Exists(bank))
            {
                using (FileStream stream = new FileStream(bank, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        String s = "";

                        List<Tuple<string, UInt32, UInt32>> ItemList = new List<Tuple<string, uint, uint>>();

                        uint headerSize = 0;

                        while (true)
                        {
                            Byte next = reader.ReadByte();
                            headerSize++;

                            if (next != 0x00)
                            {
                                s += (char)next;
                            }
                            else
                            {
                                Console.WriteLine(s);
                                bool isEnd = s.Equals("END");
                                Console.WriteLine(isEnd.ToString());

                                if (isEnd)
                                {
                                    //we move on to read the files
                                    Console.WriteLine("got end of data at " + headerSize + " bytes");
                                    break;
                                }
                                else
                                {
                                    UInt32 offset = reader.ReadUInt32();
                                    headerSize += 4;
                                    UInt32 length = reader.ReadUInt32();
                                    headerSize += 4;

                                    ItemList.Add(new Tuple<string, uint, uint>(s, offset, length));

                                    Console.WriteLine(offset);
                                    Console.WriteLine(length);
                                }

                                s = "";
                            }
                        }

                        foreach (Tuple<string, uint, uint> t in ItemList)
                        {
                            Console.WriteLine(t.Item1);

                            List<Byte> byteArr = new List<byte>();

                            Console.WriteLine("reading item with size of " + (int)t.Item3);

                            for (int i = 0; i < (int)t.Item3; i++)
                            {
                                Byte b = reader.ReadByte();
                                //Console.WriteLine(b.ToString());
                                byteArr.Add(b);
                            }

                            using (FileStream soundFile = new FileStream(t.Item1, FileMode.Create))
                            {
                                using (BinaryWriter writer = new BinaryWriter(soundFile))
                                {
                                    for (int i = 0; i < byteArr.Count; i++)
                                    {
                                        writer.Write(byteArr[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
                Console.WriteLine("specified bank not found");
        }
    }
}
