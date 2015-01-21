using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TetroXNA
{
    public class PauseGameClass
    {
        private SpriteFont font;
        public PauseGameClass(SpriteFont f)
        {
            font = f;
        }
        public void update(GameTime gameTime)
        {

        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "swag", new Vector2(250, 250), Color.Firebrick);
        }
    }
}
