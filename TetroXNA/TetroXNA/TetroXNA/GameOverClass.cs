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
        private string playerName;
        private SpriteFont font;
        private StringInputClass nameClass = new StringInputClass();  

        public GameOverClass(SpriteFont f)
        {
            font = f;
        }

        public void Update(GameTime gameTime)
        {
            nameClass.Update(gameTime);
            System.Console.WriteLine("Update Game Over");
        }

        public string getName()
        {
            playerName = nameClass.getName();
            return playerName;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            System.Console.WriteLine("Draw Game Over");
            spriteBatch.DrawString(font, "Game Over", new Vector2(200, 50), Color.White);
            spriteBatch.DrawString(font, "Enter your name:", new Vector2(125,200), Color.White);
            spriteBatch.DrawString(font, nameClass.getName(), new Vector2(250, 250), Color.Blue);
        }
    }
}
