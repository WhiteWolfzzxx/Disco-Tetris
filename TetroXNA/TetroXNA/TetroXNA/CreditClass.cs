using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetroXNA
{
    //This class displays the credit screen when Tetro starts
    public class CreditClass
    {
        private int
            redIntensity,
            blueIntensity,
            greenIntensity;
        private float
            changeScreenTimer = 0.0f,
            minChangeScreenTimer = 4.0f;
        private SpriteFont bigFont, smallFont;
        private Texture2D
            logo,
            background,
            tetroTitle;
        private SpecialEffects specialEffects = new SpecialEffects();

        public CreditClass(SpriteFont small, SpriteFont big)
        {
            bigFont = big;
            smallFont = small;
        }

        public bool ChangeScreen(GameTime gameTime)
        {
            changeScreenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            redIntensity = specialEffects.getRed();
            blueIntensity = specialEffects.getBlue();
            greenIntensity = specialEffects.getGreen();
            specialEffects.colorChanger();

            if (changeScreenTimer > minChangeScreenTimer)
                return true;
            return false;
        }

        public void LoadContent(ContentManager Content)
        {
            logo = Content.Load<Texture2D>(@"Textures\Flash-Block Studio Logo");
            background = Content.Load<Texture2D>(@"Textures\Tetro Credits Background");
            tetroTitle = Content.Load<Texture2D>(@"Textures\TetroTitle");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (changeScreenTimer <= 2)
            {
                spriteBatch.Draw(tetroTitle, new Vector2(43, 5), new Rectangle(0, 0, 570, 150), new Color(redIntensity, greenIntensity, blueIntensity), 0.0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(smallFont, "Published by", new Vector2(275, 250), Color.White);
                spriteBatch.DrawString(bigFont, "Cognitive Thought Media", new Vector2(80, 300), Color.White);
            }
            if (changeScreenTimer > 2)
            {
                spriteBatch.Draw(background, Vector2.Zero, Color.Blue);
                spriteBatch.Draw(logo, new Vector2(375, 175), Color.White);
                spriteBatch.DrawString(bigFont, "FLASH BLOCK STUDIO", new Vector2(116, 50), Color.White);
                spriteBatch.DrawString(smallFont, "Mitchell Loe", new Vector2(180, 200), Color.White);
                spriteBatch.DrawString(smallFont, "Victoria Jubb", new Vector2(180, 250), Color.White);
                spriteBatch.DrawString(smallFont, "Noah Kitson", new Vector2(180, 300), Color.White);
                spriteBatch.DrawString(smallFont, "Aaron Hosler", new Vector2(180, 350), Color.White);
            }
        }
    }
}

