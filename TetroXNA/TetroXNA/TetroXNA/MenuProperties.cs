using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TetroXNA
{
    public class MenuProperties
    {        
        private bool redIncrease = true;
        private bool greenIncrease = true;
        private bool blueIncrease = true;
        private int redIntensity = 0;
        private int greenIntensity = 50;
        private int blueIntensity = 100;
        private static int blockNum = 10000; //100x100
        private Vector2[] blockPos = new Vector2[blockNum];
        private Vector2[] startPos = new Vector2[blockNum];
        private Vector3[] blockColor = new Vector3[blockNum];
        private Rectangle[] blockRectangle = new Rectangle[blockNum];
        private float[] blockScale = new float[blockNum];
        private float[] blockDepth = new float[blockNum];
        private int shine = 0;
        private Vector2 centerPoint = new Vector2(200, 200);
        private Texture2D discoTexture;

        public int getRed() { return redIntensity; }
        public int getBlue() { return blueIntensity; }
        public int getGreen() { return greenIntensity; }

        public MenuProperties()
        {
            //sets a big square
            int y = 0;
            int x = 0;
            for (int i = 0; i < blockNum; i++)
            {
                startPos[i] = new Vector2(x * 5 + 10, y * 5 + 10); //density of the squares
                x++;
                if ((x >= Math.Sqrt(blockNum)) && (y <= Math.Sqrt(blockNum)))
                {
                    y++;
                    x = 0;
                }
                blockRectangle[i] = new Rectangle(0, 0, 5, 5);  //10,10
                blockColor[i] = new Vector3(255, 255, 255);
            }
        }

        public void colorChanger()
        {
            if (redIncrease)
            {
                redIntensity++;
                if (redIntensity >= 240)
                {
                    redIncrease = false;
                }
            }
            else
            {
                redIntensity--;
                if (redIntensity <= 20)
                {
                    redIncrease = true;
                }
            }

            //blue
            if (blueIncrease)
            {
                blueIntensity += 2;
                if (blueIntensity >= 240)
                {
                    blueIncrease = false;
                }
            }
            else
            {
                blueIntensity -= 2;
                if (blueIntensity <= 20)
                {
                    blueIncrease = true;
                }
            }

            //green
            if (greenIncrease)
            {
                greenIntensity += 3;
                if (greenIntensity >= 240)
                {
                    greenIncrease = false;
                }
            }
            else
            {
                greenIntensity -= 3;
                if (greenIntensity <= 20)
                {
                    greenIncrease = true;
                }
            }
        }

        public void LoadContentDiscoBall(ContentManager Content)
        {
            discoTexture = Content.Load<Texture2D>(@"Textures\DiscoBall");
        }

        public void UpdateDiscoBall()
        {
            centerPoint.X += 0.3f;
            if (centerPoint.X > 400)
            {
                centerPoint.X = 120;
            }


            Random random = new Random();
            for (int i = 0; i < blockNum; i++)
            {
                blockColor[i] = new Vector3(
                    .8f + (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * -0.00005f)),
                    .8f + (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * -0.00005f)),
                    .8f + (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * -0.00005f)));

                shine = random.Next(0, 900);
                if (shine == 0)
                    blockColor[i] = new Vector3(1f, 1f, 1f);

                blockScale[i] = 1f + (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * -0.00005f));// / (.01f * (float)Vector2.Distance(centerPoint, blockPos[i]));

                blockDepth[i] = (((Vector2.Distance(centerPoint, startPos[i]) * Vector2.Distance(centerPoint, startPos[i])) * 0.00005f));
            }
        }

        public void DrawDiscoBall(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, DepthStencilState.Default, RasterizerState.CullNone);
            for (int i = 0; i < blockNum; i++)
            {
                spriteBatch.Draw(
                    discoTexture,
                    (startPos[i] - centerPoint) * new Vector2(blockScale[i] + 1.3f, blockScale[i] + 1.3f) + new Vector2(350, 360),  //position the ball, and there is the roundness size
                    null,
                    new Color(blockColor[i].X, blockColor[i].Y, blockColor[i].Z),
                    0f,
                    new Vector2(blockRectangle[i].Width / 2, blockRectangle[i].Height / 2),
                    blockScale[i] + 0.3f, //2f
                    SpriteEffects.None,
                    blockDepth[i]);
            }
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
