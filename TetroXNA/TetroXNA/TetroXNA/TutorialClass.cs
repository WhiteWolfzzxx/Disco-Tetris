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

namespace TetroXNA
{
    public class TutorialClass
    {
        private bool tutorialPaused = true;
        private bool showMessage = true;
        private float timer = 0.0f;
        private float minTimer = 1.0f;
        private Texture2D tutorialBackground;
        private SpriteFont font;
        private int messageNum = 1;
        private Vector2[] messageLinePos = new Vector2[10];

        public bool getIsTutorialPaused() { return tutorialPaused; }

        public TutorialClass(SpriteFont ft)
        {
            font = ft;
            for (int i = 0; i < messageLinePos.Length; i++)
            {
                messageLinePos[i] = new Vector2(375, 285 + (i * 30));
            }
        }

        public void LoadContent(ContentManager Content)
        {
            tutorialBackground = Content.Load<Texture2D>(@"Textures\tutorialBack");
        }

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

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tutorialBackground, new Vector2(360, 275), Color.Blue);
            #region Messages
            if (showMessage)
            {
                switch (messageNum)
                {
                    case 1:
                        spriteBatch.DrawString(font, "welcome to tetro", messageLinePos[0], Color.White);
                        spriteBatch.DrawString(font, "the objective of tetro is to ", messageLinePos[1], Color.White);
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
                        spriteBatch.DrawString(font, "the line below has one", messageLinePos[0], Color.White);
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
                    default:

                        break;
                }
            }
            #endregion
        }
    }
}
