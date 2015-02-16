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
    //This class is designed to handle the settings screen
    //The player can customize the settings to their needs
    public class SettingsClass
    {
        private bool 
            spaceDidSomething = false,
            fullScreen = false,
            consoleShown = true;
        private int 
            menuOption = 1,
            redIntensity, 
            greenIntensity, 
            blueIntensity;
        private float 
            menuChangeTimer,
            minMenuChangeTimer = 0.1f;
        private SpriteFont bigFont, smallFont;
        private Texture2D settingsTitle, background;
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
            background = Content.Load<Texture2D>(@"Textures\Tetro Settings Background");
        }

        public void Update(GameTime gameTime)
        {
            fullScreen = Game1.fullscreen;
            consoleShown = Game1.consoleShown;

            redIntensity = menuProperties.getRed();
            blueIntensity = menuProperties.getBlue();
            greenIntensity = menuProperties.getGreen();
            menuProperties.colorChanger();

            menuChangeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //If key is still held down
            spaceDidSomething = keyState.IsKeyDown(Keys.Space) && spaceDidSomething;

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
                menuOption = 1;
            if (menuOption < 1)
                menuOption = 4;
        }

        public int changeSetting()
        {
            //Playing state
            switch (menuOption)
            {
                case 1:
                    //Full Screen
                    fullScreen = !fullScreen;
                    return 1;

                case 2:
                    //Shows or Hides the console
                    consoleShown = !consoleShown;
                    return 2;

                case 3:
                    //Mute music
                    return 3;

                case 4:
                    //Back to Main Menu
                    return 4;

                default:
                    //Nothing is happening
                    return 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.Blue);

            spriteBatch.Draw(settingsTitle, new Vector2(25, 20), new Color(redIntensity, greenIntensity, blueIntensity));
            spriteBatch.DrawString(smallFont, "Press space to select", new Vector2(10, 540), Color.White);
            spriteBatch.DrawString(smallFont, "Press arrows to navigate", new Vector2(10, 570), Color.White);

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
                if (!fullScreen) //If not full screen button displays full screen, while full screen button displays windowed.
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
    }
}
