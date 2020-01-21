using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using FMOD;
using Microsoft.Xna.Framework.Audio;

namespace CatEngine.Content
{
    class CAudioManager : CContentManager
    {
        public const int NUM_SONGS = 0;

        public const int GAME_SONG = 0;
        public const int MENU_SONG = 1;
        public const int GAMEOVER_SONG = 2;

        public const int NUM_SFX = 0;

        public const int SFX_PLAYERDEATH = 0;
        public const int SFX_EXPLOSION1 = 1;
        public const int SFX_EXPLOSION2 = 2;
        public const int SFX_BLASTERSHOT = 3;
        public const int SFX_RAPIDSHOT = 4;
        public const int SFX_POWERUP = 5;
        public const int SFX_MULTISHOT = 6;
        public const int SFX_RAPIDFIRE = 7;

        private FMOD.System FMODSystem;
        private FMOD.Channel MusicChannel;

        private Dictionary<string, SoundEffect> dSoundFXDict = new Dictionary<string, SoundEffect>();
        private Dictionary<string, Sound> dMusicDict = new Dictionary<string, Sound>();
        
        private CAudioManager()
        {
            FMOD.Factory.System_Create(out FMODSystem);

            FMODSystem.setDSPBufferSize(1024, 10);
            FMODSystem.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)0);
        }
        
        public static CAudioManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CAudioManager instance = new CAudioManager();
        }

        public void Unload()
        {
            Stop();
            
            FMODSystem.release();

            foreach (KeyValuePair<string, SoundEffect> s in dSoundFXDict.ToList())
            {
                s.Value.Dispose();
            }
        }

        public void LoadSong(string path, string name, string filetype)
        {
            FMOD.Sound snd;
            FMOD.RESULT r = FMODSystem.createStream(path + "/" + name + "." + filetype, FMOD.MODE.DEFAULT, out snd);
            dMusicDict.Add(name, snd);
            CConsole.Instance.Print("loaded track " + name + ", got result " + r);
        }

        public void LoadBank(string path, string name)
        {
            string file = path + "/" + name + ".bnk";

            if (File.Exists(file))
            {
                using (FileStream stream = new FileStream(file, FileMode.Open))
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
                                    //Console.WriteLine("got end of data at " + headerSize + " bytes");
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

                            for (int i = 0; i < (int)t.Item3; i++)
                            {
                                Byte b = reader.ReadByte();
                                //Console.WriteLine(b.ToString());
                                byteArr.Add(b);
                            }

                            byte[] buffer = byteArr.ToArray();

                            int freq = 44100;

                            SoundEffect snd = new SoundEffect(buffer, freq, AudioChannels.Stereo);
                            dSoundFXDict.Add(t.Item1, snd);
                            CConsole.Instance.Print("loaded sound " + t.Item1 + " with size of " + t.Item3 +" from " + name +".bnk");
                        }
                    }
                }
            }
            else
                CConsole.Instance.Print("Tried loading bank " + path +"/"+name + ".bnk but it didn't exist");
        }

        public void LoadSound(string path, string name, string filetype)
        {
            /*FMOD.Sound snd;
            FMOD.RESULT r = FMODSystem.createStream(path + "/" + name + "." + filetype, FMOD.MODE.DEFAULT, out snd);
            dSoundFXDict.Add(name, snd);
            CConsole.Instance.Print("loaded sound " + name + ", got result " + r);*/
        }

        //private int iCurrentSongID = -1;
        private string sCurrentTrack = "";

        public bool IsPlaying()
        {
            bool isPlaying = false;

            if (MusicChannel != null)
                MusicChannel.isPlaying(out isPlaying);

            return isPlaying;
        }

        /*public void PlaySoundFMOD(String name)
        {
            try
            {
                if (dSoundFXDict[name] != null)
                {
                    FMOD.RESULT r = FMODSystem.playSound(dSoundFXDict[name], null, false, out SoundChannel);
                    //UpdateVolume(1.0f);
                    SoundChannel.setMode(FMOD.MODE.LOOP_OFF);
                    SoundChannel.setLoopCount(-1);

                    //iCurrentSongID = soundId;
                }
                else
                    CConsole.Instance.Print("sound " + name + " was null");
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("Sound "+name+" wasn't found in dSoundFXDict, "+e.Message);
            }
        }*/

        public void PlaySound(string name)
        {
            dSoundFXDict[name].Play();
        }

        public void PlaySong(String name)
        {
            if (!sCurrentTrack.Equals(name))
            {
                Stop();
                try
                {
                    if (dMusicDict[name] != null)
                    {
                        FMOD.RESULT r = FMODSystem.playSound(dMusicDict[name], null, false, out MusicChannel);
                        UpdateVolume(1.0f);
                        MusicChannel.setMode(FMOD.MODE.LOOP_NORMAL);
                        MusicChannel.setLoopCount(-1);

                        sCurrentTrack = name;
                    }
                    else
                        CConsole.Instance.Print("song " + name + " was null");
                }
                catch (KeyNotFoundException e)
                {
                    CConsole.Instance.Print("Track " + name + " wasn't found in dMusicDict, " + e.Message);
                }
            }
        }

        public void UpdateVolume(float volume)
        {
            if (MusicChannel != null)
                MusicChannel.setVolume(volume);
        }

        public void Stop()
        {
            if (IsPlaying())
                MusicChannel.stop();

            sCurrentTrack = "";
        }
    }
}
