using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace TetroXNA
{
    public class SettingsClass
    {
        private bool spaceDidSomething = false;
        private bool fullScreen = false;
        private bool consoleShown = true;
        private bool muted = false;
        private int menuOption = 1;
        private int redIntensity, greenIntensity, blueIntensity;
        private float menuChangeTimer;
        private float minMenuChangeTimer = 0.1f;
        private SpriteFont bigFont, smallFont;
        private Texture2D settingsTitle;
        private MenuProperties menuProperties = new MenuProperties();
        KeyboardState keyState;
       
        
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

            //Single Key press Space
            //If key is still held down
            if (keyState.IsKeyDown(Keys.Space) && spaceDidSomething)
            {
                spaceDidSomething = true;
            }
            else
            {
                spaceDidSomething = false;
            }

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
            if (menuOption > 4)
            {
                menuOption = 1;
            }
            if (menuOption < 1)
            {
                menuOption = 4;
            }
        }

        public int changeSetting()
        {
            //playing state
            switch (menuOption)
            {
                case 1:
                    //Fullscreen
                    fullScreen = !fullScreen;
                    return 1;

                case 2:
                    //Shows-Hides the console
                    consoleShown = !consoleShown;
                    return 2;

                case 3:
                    //Mute sound
                    return 3;

                case 4:
                    //Back to Main
                    return 4;

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
                if (consoleShown)
                {
                    spriteBatch.DrawString(smallFont, "Hide Console", new Vector2(295, 250), Color.LightGray);
                }
                if (!consoleShown)
                {
                    spriteBatch.DrawString(smallFont, "Show Console", new Vector2(285, 250), Color.LightGray);
                }
                if (!fullScreen) //If not fullscreen button displays fullscreen, while fullscreen button displays windowed.
                {
                    spriteBatch.DrawString(bigFont, "Fullscreen", new Vector2(220, 320), Color.LightGray);
                }
                if (fullScreen)
                {
                    spriteBatch.DrawString(bigFont, "Windowed", new Vector2(220, 320), Color.LightGray);
                }
                spriteBatch.DrawString(smallFont, "Main Menu", new Vector2(300, 440), Color.LightGray);
            }
            if (menuOption == 2)
            {
                spriteBatch.DrawString(smallFont, "Sound", new Vector2(325, 250), Color.LightGray);
                if (consoleShown)
                {
                    spriteBatch.DrawString(bigFont, "Hide Console", new Vector2(210, 320), Color.LightGray); 
                }
                if (!consoleShown)
                {
                    spriteBatch.DrawString(bigFont, "Show Console", new Vector2(190, 320), Color.LightGray); 
                }                
                if (!fullScreen)
                {
                    spriteBatch.DrawString(smallFont, "Fullscreen", new Vector2(300, 440), Color.LightGray);
                }
                if (fullScreen)
                {
                    spriteBatch.DrawString(smallFont, "Windowed", new Vector2(300, 440), Color.LightGray);
                }
            }
            if (menuOption == 3)
            {
                spriteBatch.DrawString(smallFont, "Main Menu", new Vector2(300, 250), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Sound", new Vector2(275, 320), Color.LightGray);
                if (consoleShown)
                {
                    spriteBatch.DrawString(smallFont, "Hide Console", new Vector2(295, 440), Color.LightGray);
                }
                if (!consoleShown)
                {
                    spriteBatch.DrawString(smallFont, "Show Console", new Vector2(285, 440), Color.LightGray);
                }
            }
            if (menuOption == 4)
            {
                if (!fullScreen)
                {
                    spriteBatch.DrawString(smallFont, "Fullscreen", new Vector2(290, 250), Color.LightGray);
                }
                if (fullScreen)
                {
                    spriteBatch.DrawString(smallFont, "Windowed", new Vector2(290, 250), Color.LightGray);
                }
                spriteBatch.DrawString(bigFont, "Main Menu", new Vector2(220, 320), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Sound", new Vector2(325, 440), Color.LightGray);
            }
        }

        // Noah wrote this
        public void settingStartup()
        {
            if (File.Exists("settings.txt"))
            {
                //have to assign variables from a read file
            }
            else
            { 
                
            }
        }

    }
}
