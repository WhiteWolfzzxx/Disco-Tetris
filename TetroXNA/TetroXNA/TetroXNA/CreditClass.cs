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
        private float 
            changeScreenTimer = 0.0f,
            minChangeScreenTimer = 4.0f;
        private SpriteFont bigFont, smallFont;
        private Texture2D logo, background;

        public CreditClass(SpriteFont small, SpriteFont big)
        {
            bigFont = big;
            smallFont = small;
        }

        public bool ChangeScreen(GameTime gameTime)
        {
            changeScreenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (changeScreenTimer > minChangeScreenTimer)
                return true;
            return false;
        }

        public void LoadContent(ContentManager Content)
        {
            logo = Content.Load<Texture2D>(@"Textures\Flash-Block Studio Logo");
            background = Content.Load<Texture2D>(@"Textures\Tetro Credits Background");
        }

        public void Draw(SpriteBatch spriteBatch)
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

