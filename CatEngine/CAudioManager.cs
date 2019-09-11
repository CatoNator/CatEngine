using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
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
        private FMOD.Channel SoundChannel;

        private struct Sound
        {
            public String name;
            public FMOD.Sound sound;

            public Sound(String nm, FMOD.Sound snd)
            {
                name = nm;
                sound = snd;
            }
        }

        private List<Sound> Music = new List<Sound>();
        private List<Sound> SoundFX = new List<Sound>();
        
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

        /*public void LoadAudio()
        {
            LoadSong(GAME_SONG, "game.it");
            LoadSong(MENU_SONG, "menu.it");
            LoadSong(GAMEOVER_SONG, "gameover.it");

            LoadSound(SFX_PLAYERDEATH, "playerdeath");
            LoadSound(SFX_EXPLOSION1, "explosion1");
            LoadSound(SFX_EXPLOSION2, "explosion2");
            LoadSound(SFX_BLASTERSHOT, "normalshot");
            LoadSound(SFX_RAPIDSHOT, "rapidfireshot");
            LoadSound(SFX_POWERUP, "powerup");
            LoadSound(SFX_MULTISHOT, "multishot");
            LoadSound(SFX_RAPIDFIRE, "rapidfire");
        }*/

        public void Unload()
        {
            Stop();
            
            FMODSystem.release();
        }

        public void LoadSong(string name)
        {
            FMOD.Sound snd;
            FMOD.RESULT r = FMODSystem.createStream("AssetData/Music/" + name, FMOD.MODE.DEFAULT, out snd);
            Music.Add(new Sound(name, snd));
            Console.WriteLine("loaded track " + name + ", got result " + r);
        }

        public void LoadSound(string name)
        {
            FMOD.Sound snd;
            FMOD.RESULT r = FMODSystem.createStream("AssetData/Sounds/" + name, FMOD.MODE.DEFAULT, out snd);
            SoundFX.Add(new Sound(name, snd));
            Console.WriteLine("loaded sound " + name + ", got result " + r);
        }

        private int iCurrentSongID = -1;

        public bool IsPlaying()
        {
            bool isPlaying = false;

            if (MusicChannel != null)
                MusicChannel.isPlaying(out isPlaying);

            return isPlaying;
        }

        public void PlaySound(String name)
        {
            int soundId = 0;

            foreach (Sound i in SoundFX)
            {
                if (i.name.Equals(name))
                    soundId = SoundFX.IndexOf(i);
            }

            Console.WriteLine("soundId " + soundId);

            if (SoundFX[soundId].sound != null)
            {
                FMOD.RESULT r = FMODSystem.playSound(SoundFX[soundId].sound, null, false, out SoundChannel);
                //UpdateVolume(1.0f);
                SoundChannel.setMode(FMOD.MODE.LOOP_OFF);
                SoundChannel.setLoopCount(-1);

                Console.WriteLine("Playing sound " + soundId + ", got result " + r);

                iCurrentSongID = soundId;
            }
            else
                Console.WriteLine("sound was null");
        }

        public void PlaySong(String name)
        {
            int songId = 0;

            foreach (Sound i in Music)
            {
                if (i.name.Equals(name))
                    songId = Music.IndexOf(i);
            }

            Console.WriteLine("songId " + songId);

            if (iCurrentSongID != songId)
            {
                Stop();

                if (Music[songId].sound != null)
                {
                    FMOD.RESULT r = FMODSystem.playSound(Music[songId].sound, null, false, out MusicChannel);
                    UpdateVolume(1.0f);
                    MusicChannel.setMode(FMOD.MODE.LOOP_NORMAL);
                    MusicChannel.setLoopCount(-1);

                    Console.WriteLine("Playing track " + songId + ", got result" + r);

                    iCurrentSongID = songId;
                }
                else
                    Console.WriteLine("song was null");
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

            iCurrentSongID = -1;
        }
    }
}
