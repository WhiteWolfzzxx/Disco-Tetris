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
using Microsoft.Xna.Framework.Media;

namespace TetroXNA
{
    //This class is designed to display messages about how to play Tetro
    //The tutorial screen mostly reuses the play state code and the tutorial will pause to show the messages
    public class TutorialClass
    {
        private bool 
            tutorialPaused = true,
            showMessage = true,
            gotoMenu = false;
        private int messageNum = 1;
        private float 
            timer = 0.0f,
            minTimer = 1.0f;
        private Texture2D tutorialBackground;
        private SpriteFont font;
        private Song menuBGM;
        private Vector2[] messageLinePos = new Vector2[10];

        public bool getGotoMenu() { return gotoMenu; }
        public bool getIsTutorialPaused() { return tutorialPaused; }

        public TutorialClass(SpriteFont ft, Song mb)
        {
            font = ft;
            menuBGM = mb;
            for (int i = 0; i < messageLinePos.Length; i++)
                messageLinePos[i] = new Vector2(375, 285 + (i * 30));
        }

        public void resetTutorial()
        {
            gotoMenu = false;
            messageNum = 1;
            timer = 0.0f;
            showMessage = true;
            tutorialPaused = true;
        }

        public void LoadContent(ContentManager Content)
        {
            tutorialBackground = Content.Load<Texture2D>(@"Textures\tutorialBack");
        }

        //Tutorial Logic
        //Constructors at the menu will reset this class
        //Each state is the message number
        //Message number increments every time the timer reaches it's set limit allowing the player to play
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (messageNum == 1)
            {
                if (timer > minTimer)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        messageNum++;
                        timer = 0.0f;
                    }
                }
            }
            if (messageNum == 2)
            {
                if (timer > minTimer)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        messageNum++;
                        timer = 0.0f;
                        tutorialPaused = false;
                        showMessage = false;
                    }
                }
            }
            if (messageNum == 3)
            {
                if (timer > 3.0f)
                {
                    showMessage = true;
                    tutorialPaused = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        tutorialPaused = false;
                        showMessage = false;
                        timer = 0.0f;
                        messageNum++;
                    }
                }
            }
            if (messageNum == 4)
            {
                if (timer > 18.0f)
                {
                    tutorialPaused = true;
                    showMessage = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        tutorialPaused = false;
                        showMessage = false;
                        timer = 0.0f;
                        messageNum++;
                    }
                }
            }
            if (messageNum == 5)
            {
                if (timer > 3.0f)
                {
                    showMessage = true;
                    tutorialPaused = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        tutorialPaused = false;
                        timer = 0.0f;
                        showMessage = false;
                        messageNum++;
                    }
                }
            }
            if (messageNum == 6)
            {
                if (timer > 5.0f)
                {
                    tutorialPaused = true;
                    showMessage = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        MediaPlayer.Play(menuBGM);
                        gotoMenu = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tutorialBackground, new Vector2(348, 260), Color.Blue);
            #region Messages
            if (showMessage)
            {
                switch (messageNum)
                {
                    case 1:
                        spriteBatch.DrawString(font, "Welcome to Tetro", messageLinePos[0], Color.White);
                        spriteBatch.DrawString(font, "the objective of Tetro is to ", messageLinePos[1], Color.White);
                        spriteBatch.DrawString(font, "score the most points as", messageLinePos[2], Color.White);
                        spriteBatch.DrawString(font, "possible", messageLinePos[3], Color.White);
                        spriteBatch.DrawString(font, "press space to continue", messageLinePos[5], Color.White);
                        break;
                    case 2:
                        spriteBatch.DrawString(font, "To score points you will", messageLinePos[0], Color.White);
                        spriteBatch.DrawString(font, "land as many blocks and try", messageLinePos[1], Color.White);
                        spriteBatch.DrawString(font, "create as many lines as", messageLinePos[2], Color.White);
                        spriteBatch.DrawString(font, "possible", messageLinePos[3], Color.White);
                        spriteBatch.DrawString(font, "press space to continue", messageLinePos[5], Color.White);
                        break;
                    case 3:
                        spriteBatch.DrawString(font, "The line below has one", messageLinePos[0], Color.White);
                        spriteBatch.DrawString(font, "spot open. you need to", messageLinePos[1], Color.White);
                        spriteBatch.DrawString(font, "rotate the blocks", messageLinePos[2], Color.White);
                        spriteBatch.DrawString(font, "press the up arrow", messageLinePos[4], Color.White);
                        spriteBatch.DrawString(font, "to rotate", messageLinePos[5], Color.White);
                        break;
                    case 4:
                        spriteBatch.DrawString(font, "The blocks can also move", messageLinePos[0], Color.White);
                        spriteBatch.DrawString(font, "to the left and right", messageLinePos[1], Color.White);
                        spriteBatch.DrawString(font, "using the left and right", messageLinePos[2], Color.White);
                        spriteBatch.DrawString(font, "arrow keys", messageLinePos[3], Color.White);
                        spriteBatch.DrawString(font, "press the left arrow key", messageLinePos[5], Color.White);
                        break;
                    case 5:
                        spriteBatch.DrawString(font, "If the game is too slow", messageLinePos[0], Color.White);
                        spriteBatch.DrawString(font, "Hold the down arrow on ", messageLinePos[1], Color.White);
                        spriteBatch.DrawString(font, "your keyboard", messageLinePos[2], Color.White);
                        spriteBatch.DrawString(font, "Press the down arrow", messageLinePos[4], Color.White);
                        break;
                    case 6:
                        spriteBatch.DrawString(font, "Congratulations", messageLinePos[0], Color.White);
                        spriteBatch.DrawString(font, "you have completed the ", messageLinePos[1], Color.White);
                        spriteBatch.DrawString(font, "tutorial.  Now go to", messageLinePos[2], Color.White);
                        spriteBatch.DrawString(font, "menu and select play", messageLinePos[3], Color.White);
                        spriteBatch.DrawString(font, "press space to exit", messageLinePos[5], Color.White);
                        spriteBatch.DrawString(font, "the tutorial", messageLinePos[6], Color.White);
                        break;
                    default:

                        break;
                }
            }
            #endregion
        }
    }
}
