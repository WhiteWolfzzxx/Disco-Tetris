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
    public class GameOverClass
    {
        private SpriteFont font;
        private StringInputClass nameClass = new StringInputClass();

        public GameOverClass(SpriteFont f)
        {
            font = f;
        }

        public void Update(GameTime gameTime)
        {
            nameClass.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Game Over", new Vector2(150, 50), Color.White);
            spriteBatch.DrawString(font, "Enter your name:", new Vector2(150,200), Color.White);
            spriteBatch.DrawString(font, nameClass.getName(), new Vector2(150, 250), Color.Blue);
        }
    }
}
