﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetroXNA
{
    //This class is designed to handle the pause menu and stop the game from updating
    public class PauseGameClass
    {
        private bool spaceDidSomething = false;
        private int menuOption = 1;
        private float 
            minMenuChangeTimer = 0.1f,
            menuChangeTimer;
        private SpriteFont 
            bigFont,
            smallFont;
        private KeyboardState keyState;
        
        public int getMenuOption() { return menuOption; }

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
                spaceDidSomething = true;
            else
                spaceDidSomething = false;
            //Navigation
            if (keyState.IsKeyDown(Keys.Up))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption--;
                    menuChangeTimer = 0.0f;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption++;
                    menuChangeTimer = 0.0f;
                }
            }
            if (menuOption > 4)
                menuOption = 1;
            if (menuOption < 1)
                menuOption = 4;
        }
        public void draw(SpriteBatch spriteBatch)
        {
            if (menuOption == 1)
            {
                spriteBatch.DrawString(smallFont, "exit without saving", new Vector2(80, 210), Color.White);
                spriteBatch.DrawString(bigFont, "Resume", new Vector2(60, 250), Color.White);
                spriteBatch.DrawString(smallFont, "save", new Vector2(110, 320), Color.White);
            }
            if (menuOption == 2)
            {
                spriteBatch.DrawString(smallFont, "resume", new Vector2(110, 210), Color.White);
                spriteBatch.DrawString(bigFont, "Save", new Vector2(80, 250), Color.White);
                spriteBatch.DrawString(smallFont, "save and exit", new Vector2 (90, 320), Color.White);
            }
            if (menuOption == 3)
            {
                spriteBatch.DrawString(smallFont, "save", new Vector2(110, 210), Color.White);
                spriteBatch.DrawString(bigFont, "save and exit", new Vector2(23, 250), Color.White, 0.0f, Vector2.Zero, 0.85f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(smallFont, "exit without saving", new Vector2(80, 320), Color.White);
            }
            if (menuOption == 4)
            {
                spriteBatch.DrawString(smallFont, "save and exit", new Vector2(90, 210), Color.White);
                spriteBatch.DrawString(bigFont, "exit without saving", new Vector2(23, 255), Color.White, 0.0f, Vector2.Zero, 0.55f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(smallFont, "resume", new Vector2(110, 320), Color.White);
            }
        }
    }
}
