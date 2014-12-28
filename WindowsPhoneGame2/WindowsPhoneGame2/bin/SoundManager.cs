using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


namespace WindowsPhoneGame2
{
    public enum ESound
    {
        Touch,
        Select,
        Explosion
    }

    public enum ESong
    {
        Menu
    }

    public static class SoundManager
    {
        #region Attributes
        
        // Check if song is repeated
        public static bool IsRepeating = true;

        // Songs
        private static Song menuSong;

        // Sound effects
        private static SoundEffect Touch;
        private static SoundEffect Select;
        private static SoundEffect Explosion;


        #endregion

        #region Initialize
        

        /// <summary>
        /// Load some songs and sound need for menu and other screens
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent1(ContentManager Content)
        {
            //if(!MediaPlayer.GameHasControl)
            //{
            //    Engine.ShowDialog("");
                
            //}
                // Songs
                //menuSong = Content.Load<Song>(@"Sounds\Menu");

                // Sound effects
                //Touch = Content.Load<SoundEffect>(@"Sounds\Touch");
                ///Select = Content.Load<SoundEffect>(@"Sounds\bounce");
               //Explosion = Content.Load<SoundEffect>(@"Sounds\getscore");

            
            
        }

        /// <summary>
        /// Load all songs and sound need for play game
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent2(ContentManager Content)
        {
            
        }


        #endregion

        #region Public methods
        

        public static void PlaySound(ESound id)
        {
            // Check if sound option is OFF
            //if (GlobalOption.SOUND)
            {
                switch (id)
                {
                    case ESound.Touch:
                        if(Touch != null)
                            Touch.Play();
                        break;
                    case ESound.Select:
                        if (Select != null)
                            Select.Play();
                        break;
                    case ESound.Explosion:
                        if (Explosion != null)
                            Explosion.Play();
                        break;
                }
            }
        }

        public static void PlaySong(ESong id)
        {
            MediaPlayer.IsRepeating = IsRepeating;

            // Check if sound option is OFF
            //if (GlobalOption.SOUND)
            {
                switch (id)
                {
                    case ESong.Menu:
                        if (menuSong != null)
                        {
                            MediaPlayer.Play(menuSong);
                            MediaPlayer.Volume = 0.2f;
                        }
                        break;
                }
            }
        }

        public static void MuteUnmuteSong()
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
        }

        public static void StopSongs()
        {
            MediaPlayer.Stop();
        }


        #endregion
    }
}
