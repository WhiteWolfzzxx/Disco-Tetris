using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TetroXNA
{
    //This class contains code that serves special tasks like the color phases for the menu and the blocks
    public class MenuProperties
    {        
        private bool 
            redIncrease = true,
            greenIncrease = true,
            blueIncrease = true;
        private int 
            redIntensity = 0,
            greenIntensity = 50,
            blueIntensity = 100,
            shine = 0;
        private static int blockNum = 10000;
        private Vector2[] blockPos = new Vector2[blockNum];
        private Vector2[] startPos = new Vector2[blockNum];
        private float[] blockColor = new float[blockNum];
        private float[] blockScale = new float[blockNum];
        private float[] blockDepth = new float[blockNum];
        private Rectangle[] blockRectangle = new Rectangle[blockNum];
        private Vector2 centerPoint = new Vector2(200, 200);
        private Texture2D discoTexture;

        public int getRed() { return redIntensity; }
        public int getBlue() { return blueIntensity; }
        public int getGreen() { return greenIntensity; }

        public MenuProperties()
        {
            //Creates a grid of squares
            int x = 0, y = 0;
            for (int i = 0; i < blockNum; i++)
            {
                startPos[i] = new Vector2(x * 5 + 10, y * 5 + 10);
                x++;
                if ((x >= Math.Sqrt(blockNum)) && (y <= Math.Sqrt(blockNum)))
                {
                    y++;
                    x = 0;
                }
                blockRectangle[i] = new Rectangle(0, 0, 5, 5);
                //blockColor[i] = new Vector3(255, 255, 255);
                blockColor[i] = 255;
            }
        }

        public void colorChanger()
        {
            //Red
            if (redIncrease)
            {
                redIntensity++;
                if (redIntensity >= 240)
                    redIncrease = false;
            }
            else
            {
                redIntensity--;
                if (redIntensity <= 20)
                    redIncrease = true;
            }

            //Blue
            if (blueIncrease)
            {
                blueIntensity += 2;
                if (blueIntensity >= 240)
                    blueIncrease = false;
            }
            else
            {
                blueIntensity -= 2;
                if (blueIntensity <= 20)
                    blueIncrease = true;
            }

            //Green
            if (greenIncrease)
            {
                greenIntensity += 3;
                if (greenIntensity >= 240)
                    greenIncrease = false;
            }
            else
            {
                greenIntensity -= 3;
                if (greenIntensity <= 20)
                    greenIncrease = true;
            }
        }

        public void LoadContentDiscoBall(ContentManager Content)
        {
            discoTexture = Content.Load<Texture2D>(@"Textures\DiscoBall");
        }

        public void UpdateDiscoBall()
        {
            //Spins the discoball
            centerPoint.X += 0.3f;
            if (centerPoint.X > 350)
                centerPoint.X = 140;

            Random random = new Random();
            for (int i = 0; i < blockNum; i++)
            {
                blockColor[i] = .8f + (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * -0.00005f));

                shine = random.Next(0, 900);
                if (shine == 0)
                    blockColor[i] = 1;

                blockScale[i] = 1f + (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * -0.00005f));
                blockDepth[i] = (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * 0.00005f));
            }
        }

        public void DrawDiscoBall(SpriteBatch spriteBatch)
        {
            //The spriteBatch.Begin and end are call multiple times because this requires special depth requirement to display correctly
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, DepthStencilState.Default, RasterizerState.CullNone);
            for (int i = 0; i < blockNum; i++)
            {
                //This has the equation for the discoball's depth, color and shape
                spriteBatch.Draw(
                    discoTexture,
                    (startPos[i] - centerPoint) * new Vector2(blockScale[i] + 1.3f, blockScale[i] + 1.3f) + new Vector2(350, 360),
                    null,
                    new Color(blockColor[i], blockColor[i], blockColor[i]),
                    0f,
                    new Vector2(blockRectangle[i].Width / 2, blockRectangle[i].Height / 2),
                    blockScale[i] + 0.3f,
                    SpriteEffects.None,
                    blockDepth[i]);
            }
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
