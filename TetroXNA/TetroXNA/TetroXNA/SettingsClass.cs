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
        private bool fDidSomething = false;
        private bool toggleFullScreen = false;
        private Texture2D settingsTitle;
        private int redIntensity;
        private int greenIntensity;
        private int blueIntensity;
        private MenuProperties menuProperties = new MenuProperties();
       
        public bool getFull() { return toggleFullScreen; }

        public void SettingClass()
        {
           
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

            if (Keyboard.GetState().IsKeyDown(Keys.F) && !fDidSomething)
            {
                toggleFullScreen = true;
                fDidSomething = true;
            }
            else
            {
                toggleFullScreen = false;
            }

            //Single Key press Space
            if (Keyboard.GetState().IsKeyDown(Keys.F) && fDidSomething)
            {
                fDidSomething = true;
            }
            else
            {
                fDidSomething = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(settingsTitle, new Vector2(25, 50), new Color(redIntensity, greenIntensity, blueIntensity));
            spriteBatch.DrawString(font, "Press F to fullscreen", new Vector2(67,200), Color.White);
            spriteBatch.DrawString(font, "Press Space to go back", new Vector2(58, 500), Color.White);
        }
    }
}
