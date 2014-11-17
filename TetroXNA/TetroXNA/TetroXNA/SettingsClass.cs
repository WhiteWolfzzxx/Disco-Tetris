using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetroXNA
{
    public class SettingsClass
    {
        private bool spaceDidSomething = false;
        private bool toggleFullScreen = false;
        private Texture2D settingsTitle;
        private int redIntensity;
        private int greenIntensity;
        private int blueIntensity;
        private MenuProperties menuProperties = new MenuProperties();
        private int menuOption = 1;
        private float menuChangeTimer;
        private float minMenuChangeTimer = 0.1f;
        private SpriteFont bigFont;
        private SpriteFont smallFont;
        private bool fullScreen = false;
       
        public bool getFull() { return toggleFullScreen; }

        public SettingsClass(SpriteFont small, SpriteFont big)
        {
            bigFont = big;
            smallFont = small;
        }

        public void Load(ContentManager Content)
        {
            settingsTitle = Content.Load<Texture2D>(@"Textures\SettingsTitle");
        }

        public void Update(GameTime gameTime)
        {
            redIntensity = menuProperties.getRed();
            blueIntensity = menuProperties.getBlue();
            greenIntensity = menuProperties.getGreen();
            menuProperties.colorChanger();

            menuChangeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Navigate the menu
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption++;
                    menuChangeTimer = 0.0f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption--;
                    menuChangeTimer = 0.0f;
                }
            }

            //Resets the menu options
            if (menuOption > 2)
            {
                menuOption = 1;
            }
            if (menuOption < 1)
            {
                menuOption = 2;
            }

            //Single Key press Space
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && spaceDidSomething)
            {
                spaceDidSomething = false;
            }
            else
            {
                spaceDidSomething = true;
            }

            if (menuOption == 1 && fullScreen == false && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                fullScreen = true;
            }
            else if (menuOption == 1 && fullScreen == true && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                fullScreen = false;
            }
        }

        public int changeSetting()
        {
            //playing state
            switch (menuOption)
            {
                case 1:
                    //Fullscreen
                    return 1;

                case 2:
                    //Back to Main
                    return 2;

                default:
                    //Nothing is happening
                    return 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(settingsTitle, new Vector2(25, 50), new Color(redIntensity, greenIntensity, blueIntensity));

            if (menuOption == 1)
            {
                spriteBatch.DrawString(smallFont, "Main Menu", new Vector2(300, 250), Color.LightGray);
                if (fullScreen == false)
                {
                    spriteBatch.DrawString(bigFont, "Fullscreen", new Vector2(220, 320), Color.LightGray);
                }
                else
                {
                    spriteBatch.DrawString(bigFont, "Windowed", new Vector2(220, 320), Color.LightGray);
                }
                spriteBatch.DrawString(smallFont, "Main Menu", new Vector2(300, 440), Color.LightGray);
            }
            if (menuOption == 2)
            {
                if (fullScreen == false)
                {
                    spriteBatch.DrawString(smallFont, "Fullscreen", new Vector2(300, 250), Color.LightGray);
                }
                else
                {
                    spriteBatch.DrawString(smallFont, "Windowed", new Vector2(300, 250), Color.LightGray);
                }
                spriteBatch.DrawString(bigFont, "Main Menu", new Vector2(220, 320), Color.LightGray);
                if (fullScreen == false)
                {
                    spriteBatch.DrawString(smallFont, "Fullscreen", new Vector2(300, 440), Color.LightGray);
                }
                else
                {
                    spriteBatch.DrawString(smallFont, "Windowed", new Vector2(300, 440), Color.LightGray);
                }
            }
        }
    }
}
