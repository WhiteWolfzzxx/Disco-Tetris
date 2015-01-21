using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TetroXNA
{
    public class PauseGameClass
    {
        private SpriteFont bigFont;
        private SpriteFont smallFont;
        private float menuChangeTimer;
        private KeyboardState keyState;
        private bool spaceDidSomething = false;
        private int menuOption = 1;
        private float minMenuChangeTimer = 0.1f;
        public PauseGameClass(SpriteFont f, SpriteFont s)
        {
            bigFont = f;
            smallFont = s;
        }
        public void update(GameTime gameTime)
        {
            menuChangeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyState = Keyboard.GetState();
            //For single key presses
            if (keyState.IsKeyDown(Keys.Space) && spaceDidSomething)
            {
                spaceDidSomething = true;
            }
            else
            {
                spaceDidSomething = false;
            }
            //Navigation
            if (keyState.IsKeyDown(Keys.Up))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption++;
                    menuChangeTimer = 0.0f;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption--;
                    menuChangeTimer = 0.0f;
                }
            }
            if (menuOption > 3)
            {
                menuOption = 1;
            }
            if (menuOption < 1)
            {
                menuOption = 3;
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(bigFont, "Victoria is Cute", new Vector2(250, 250), Color.Firebrick);
          //  spriteBatch.DrawString(bigFont, menuOption.ToString(), new Vector2(150, 250), Color.Firebrick);
            if (menuOption == 1)
            {
                spriteBatch.DrawString(bigFont, "Resume", new Vector2(60, 250), Color.White);
            }
            if (menuOption == 2)
            {
                spriteBatch.DrawString(bigFont, "Save", new Vector2(60, 250), Color.White);
            }
            if (menuOption == 3)
            {
                spriteBatch.DrawString(bigFont, "Exit", new Vector2(60, 250), Color.White);
            }

        }
    }
}
