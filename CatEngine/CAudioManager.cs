using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
{
    class CAudioManager
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
        private FMOD.Sound[] Music;
        private FMOD.Sound[] SoundFX;
        
        private CAudioManager()
        {
            FMOD.Factory.System_Create(out FMODSystem);

            FMODSystem.setDSPBufferSize(1024, 10);
            FMODSystem.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)0);
            
            Music = new FMOD.Sound[NUM_SONGS];

            SoundFX = new FMOD.Sound[NUM_SFX];
        }
        
        public static CAudioManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CAudioManager instance = new CAudioManager();
        }

        public void LoadAudio()
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
        }

        public void Unload()
        {
            Stop();
            
            FMODSystem.release();
        }

        public void PlaySound(int soundId)
        {
            if (soundId >= 0 && soundId < NUM_SFX && SoundFX[soundId] != null)
            {
                FMOD.RESULT r = FMODSystem.playSound(SoundFX[soundId], null, false, out SoundChannel);
                //UpdateVolume(1.0f);
                SoundChannel.setMode(FMOD.MODE.LOOP_OFF);
                SoundChannel.setLoopCount(-1);

                Console.WriteLine("Playing sound " + soundId + ", got result " + r);

                m_iCurrentSongID = soundId;
            }
        }

        private void LoadSong(int songId, string name)
        {
            FMOD.RESULT r = FMODSystem.createStream("Music/" + name, FMOD.MODE.DEFAULT, out Music[songId]);
            //Console.WriteLine("loading " + songId + ", got result " + r);
        }

        private void LoadSound(int soundId, string name)
        {
            FMOD.RESULT r = FMODSystem.createStream("Sounds/" + name + ".wav", FMOD.MODE.DEFAULT, out SoundFX[soundId]);
            Console.WriteLine("loading " + name + ", got result " + r);
        }

        private int m_iCurrentSongID = -1;

        public bool IsPlaying()
        {
            bool isPlaying = false;

            if (MusicChannel != null)
                MusicChannel.isPlaying(out isPlaying);

            return isPlaying;
        }

        public void Play(int songId)
        {
            if (m_iCurrentSongID != songId)
            {
                Stop();

                if (songId >= 0 && songId < NUM_SONGS && Music[songId] != null)
                {
                    FMOD.RESULT r = FMODSystem.playSound(Music[songId], null, false, out MusicChannel);
                    UpdateVolume(1.0f);
                    MusicChannel.setMode(FMOD.MODE.LOOP_NORMAL);
                    MusicChannel.setLoopCount(-1);

                    //Console.WriteLine("Playing track " + songId + ", got result" + r);

                    m_iCurrentSongID = songId;
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

            m_iCurrentSongID = -1;
        }
    }
}
