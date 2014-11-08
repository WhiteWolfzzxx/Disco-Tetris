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
    public class CreditClass
    {
        private float changeScreenTimer = 0.0f;
        private float minChangeScreenTimer = 4.0f;
        private SpriteFont bigFont;
        private SpriteFont smallFont;

        public CreditClass(SpriteFont small, SpriteFont big)
        {
            bigFont = big;
            smallFont = small;
        }

        public bool ChangeScreen(GameTime gameTime)
        {
            changeScreenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (changeScreenTimer > minChangeScreenTimer)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(bigFont, "FLASH BLOCK STUDIO", new Vector2(100, 50), Color.White);
            spriteBatch.DrawString(smallFont, "Mitchell Loe", new Vector2(250, 200), Color.White);
            spriteBatch.DrawString(smallFont, "Victoria Jubb", new Vector2(250, 250), Color.White);
            spriteBatch.DrawString(smallFont, "Noah Kitson", new Vector2(250, 300), Color.White);
            spriteBatch.DrawString(smallFont, "Aaron Hosler", new Vector2(250, 350), Color.White);
        }
    }
}

