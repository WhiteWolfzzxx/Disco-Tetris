using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TetroXNA
{
    public class MainMenuClass
    {
        private int menuOption = 1;
        private int redIntensity, blueIntensity, greenIntensity;
        private float menuChangeTimer;
        private float minMenuChangeTimer = 0.1f;
        private Texture2D title;
        private Texture2D background;
        private SpriteFont bigFont, smallFont;
        private MenuProperties menuProperties = new MenuProperties();

        public MainMenuClass(SpriteFont small, SpriteFont big)
        {
            bigFont = big;
            smallFont = small;
        }

        public void Update(GameTime gameTime)
        {
            redIntensity = menuProperties.getRed();
            blueIntensity = menuProperties.getBlue();
            greenIntensity = menuProperties.getGreen();
            menuProperties.colorChanger();
            menuChangeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Navigate the menu
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption++;
                    menuChangeTimer = 0.0f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (menuChangeTimer > minMenuChangeTimer)
                {
                    menuOption--;
                    menuChangeTimer = 0.0f;
                }
            }

            //Resets the menu options
            if (menuOption > 6)
            {
                menuOption = 1;
            }
            if (menuOption < 1)
            {
                menuOption = 6;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            title = Content.Load<Texture2D>(@"Textures\TetroTitle");
            background = Content.Load<Texture2D>(@"Textures\Tetro Main Menu Background");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.Blue);

            spriteBatch.Draw(title, new Vector2(66, 5), new Color(redIntensity, greenIntensity, blueIntensity));

            spriteBatch.DrawString(smallFont, "Press space to select", new Vector2(10, 540), Color.White);
            spriteBatch.DrawString(smallFont, "Press arrows to navigate", new Vector2(10, 570), Color.White);

            if (menuOption == 1)
            {
                spriteBatch.DrawString(smallFont, "Load Game", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Play", new Vector2(290, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Tutorial", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 2)
            {
                spriteBatch.DrawString(smallFont, "Play", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Tutorial", new Vector2(200, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Settings", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 3)
            {
                spriteBatch.DrawString(smallFont, "Tutorial", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Settings", new Vector2(240, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Exit", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 4)
            {
                spriteBatch.DrawString(smallFont, "Settings", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Exit", new Vector2(290, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "High Scores", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 5)
            {
                spriteBatch.DrawString(smallFont, "Exit", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "High Scores", new Vector2(200, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Load Game", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 6)
            {
                spriteBatch.DrawString(smallFont, "High Scores", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Load Game", new Vector2(200, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Play", new Vector2(540, 325), Color.LightGray);
            }
        }

        public int detectGameState()
        {
            //playing state
            switch (menuOption)
            {
                case 1:
                    //Playing
                    return 1;

                case 2:
                    //Tutorial
                    return 2;

                case 3:
                    //Settings
                    return 3;

                case 4:
                    //Exit
                    return 4;

                case 5:
                    //High Scores
                    return 5;

                case 6:
                    //Load Game
                    return 6;

                default:
                    //Nothing is happening
                    return 0;
            }
        }
    }
}

