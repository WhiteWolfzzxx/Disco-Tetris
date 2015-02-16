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
    //This class is designed for the game over screen and uses the string input class to get the name of the player
    public class GameOverClass
    {
        private string playerName;
        private bool canSubmitName = false;
        private SpriteFont font, smallFont;
        private StringInputClass nameClass = new StringInputClass();
        private Texture2D background;

        public bool getCanSubmitName() { return canSubmitName; }

        public GameOverClass(SpriteFont f, SpriteFont sf)
        {
            font = f;
            smallFont = sf;
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>(@"Textures\Tetro GameOver Background");
        }

        public void Update(GameTime gameTime)
        {
            nameClass.Update(gameTime);
            System.Console.WriteLine("Update Game Over");
            canSubmitName = (nameClass.getName().Length == 3);
        }

        public string getName()
        {
            playerName = nameClass.getName();
            return playerName;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            System.Console.WriteLine("Draw Game Over");
            spriteBatch.Draw(background, Vector2.Zero, Color.Blue);
            spriteBatch.DrawString(smallFont, "Press space to enter the name", new Vector2(10, 570), Color.White);
            spriteBatch.DrawString(font, "Game Over", new Vector2(200, 50), Color.White);
            spriteBatch.DrawString(font, "Enter your name:", new Vector2(125, 200), Color.White);
            spriteBatch.DrawString(font, nameClass.getName(), new Vector2(250, 250), Color.Blue);
            if (!canSubmitName)
                spriteBatch.DrawString(smallFont, "invalid name please enter three letters", new Vector2(125, 300), Color.Red);
        }
    }
}
