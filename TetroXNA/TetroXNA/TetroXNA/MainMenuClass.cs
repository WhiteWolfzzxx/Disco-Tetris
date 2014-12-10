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
        private int redIntensity;
        private int greenIntensity;
        private int blueIntensity;
        private float menuChangeTimer;
        private float minMenuChangeTimer = 0.1f;
        private Texture2D title;
        private SpriteFont bigFont;
        private SpriteFont smallFont;
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
            if (menuOption > 5)
            {
                menuOption = 1;
            }
            if (menuOption < 1)
            {
                menuOption = 5;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            title = Content.Load<Texture2D>(@"Textures\TetroTitle");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(title, new Vector2(66, 50), new Color(redIntensity, greenIntensity, blueIntensity));

            if (menuOption == 1)
            {
                spriteBatch.DrawString(smallFont, "Load Game", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Play", new Vector2(290, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "High Scores", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 2)
            {
                spriteBatch.DrawString(smallFont, "Play", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "High Scores", new Vector2(200, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Settings", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 3)
            {
                spriteBatch.DrawString(smallFont, "High Scores", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Settings", new Vector2(240, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Exit", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 4)
            {
                spriteBatch.DrawString(smallFont, "Settings", new Vector2(75, 325), Color.LightGray);
                spriteBatch.DrawString(bigFont, "Exit", new Vector2(290, 300), Color.LightGray);
                spriteBatch.DrawString(smallFont, "Load Game", new Vector2(540, 325), Color.LightGray);
            }
            if (menuOption == 5)
            {
                spriteBatch.DrawString(smallFont, "Exit", new Vector2(75, 325), Color.LightGray);
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
                    //High Scores
                    return 2;

                case 3:
                    //Controls
                    return 3;

                case 4:
                    //Exit
                    return 4;

                case 5:
                    //Load Game
                    return 5;

                default:
                    //Nothing is happening
                    return 0;
            }
        }
    }
}

