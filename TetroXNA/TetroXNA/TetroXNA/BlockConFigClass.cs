using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetroXNA
{
    //This class is used for the arrangement property for the 4 blocks the player controls
    //This also has how the positioning location for the blocks when they need to rotate a pattern
    public class BlockConFigClass
    {
        private int pattern, index, locationX, locationY;
        private float rTimer;
        private Texture2D block;

        public float getRotateTimer() { return rTimer; }

        public BlockConFigClass(int patt, int inx, Texture2D bl)
        {
            pattern = patt;
            index = inx;
            block = bl;
        }

        //Sets the position of the blocks when the next pattern is generated
        public int resetPattern(string xY)
        {
            #region pattern 1 square
            if (pattern == 1)
            {
                if (index == 0)
                {
                    locationX = 4;
                    locationY = 0;
                }
                if (index == 1)
                {
                    locationX = 5;
                    locationY = 0;
                }
                if (index == 2)
                {
                    locationX = 4;
                    locationY = 1;
                }
                if (index == 3)
                {
                    locationX = 5;
                    locationY = 1;
                }
            }
            #endregion

            #region pattern 2 line
            if (pattern == 2)
            {
                if (index == 0)
                {
                    locationX = 3;
                    locationY = 0;
                }
                if (index == 1)
                {
                    locationX = 4;
                    locationY = 0;
                }
                if (index == 2)
                {
                    locationX = 5;
                    locationY = 0;
                }
                if (index == 3)
                {
                    locationX = 6;
                    locationY = 0;
                }
            }
            #endregion

            #region pattern 3 Z
            if (pattern == 3)
            {
                if (index == 0)
                {
                    locationX = 4;
                    locationY = 0;
                }
                if (index == 1)
                {
                    locationX = 3;
                    locationY = 0;
                }
                if (index == 2)
                {
                    locationX = 4;
                    locationY = 1;
                }
                if (index == 3)
                {
                    locationX = 5;
                    locationY = 1;
                }
            }
            #endregion

            #region pattern 4 S
            if (pattern == 4)
            {
                if (index == 0)
                {
                    locationX = 4;
                    locationY = 0;
                }
                if (index == 1)
                {
                    locationX = 5;
                    locationY = 0;
                }
                if (index == 2)
                {
                    locationX = 4;
                    locationY = 1;
                }
                if (index == 3)
                {
                    locationX = 3;
                    locationY = 1;
                }
            }
            #endregion

            #region pattern 5 T
            if (pattern == 5)
            {
                if (index == 0)
                {
                    locationX = 4;
                    locationY = 0;
                }
                if (index == 1)
                {
                    locationX = 3;
                    locationY = 0;
                }
                if (index == 2)
                {
                    locationX = 5;
                    locationY = 0;
                }
                if (index == 3)
                {
                    locationX = 4;
                    locationY = 1;
                }
            }
            #endregion

            #region pattern 6 J
            if (pattern == 6)
            {
                if (index == 0)
                {
                    locationX = 5;
                    locationY = 1;
                }
                if (index == 1)
                {
                    locationX = 5;
                    locationY = 0;
                }
                if (index == 2)
                {
                    locationX = 5;
                    locationY = 2;
                }
                if (index == 3)
                {
                    locationX = 4;
                    locationY = 2;
                }
            }
            #endregion

            #region pattern 7 L
            if (pattern == 7)
            {
                if (index == 0)
                {
                    locationX = 4;
                    locationY = 1;
                }
                if (index == 1)
                {
                    locationX = 4;
                    locationY = 0;
                }
                if (index == 2)
                {
                    locationX = 4;
                    locationY = 2;
                }
                if (index == 3)
                {
                    locationX = 5;
                    locationY = 2;
                }
            }
            #endregion


            if (xY.Equals("X"))
            {
                return locationX;
            }
            else
            {
                return locationY;
            }
        }

        //Rotates the current pattern
        public int rotatePattern(int locationX, int locationY, float rotateTimer, float minRotateTimer, int rotateState, int xYR)
        {
            rTimer = rotateTimer;
            if (rotateTimer >= minRotateTimer)
            {
                rotateState++;

                #region pattern 2
                if (pattern == 2)
                {
                    if (rotateState == 1)
                    {
                        if (locationY == 18)
                        {
                            locationY = 17;
                        }

                        if (index == 0)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 1)
                        {

                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationX -= 2;
                            locationY += 2;
                        }
                    }
                    if (rotateState == 2)
                    {

                        //Hard code fix for unique out of bounds array error
                        if (locationX == 8)
                        {
                            locationX = 7;
                        }

                        if (index == 0)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 1)
                        {

                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationX += 2;
                            locationY -= 2;
                        }
                        rotateState = 0;
                    }
                }
                #endregion

                #region pattern 3
                if (pattern == 3)
                {
                    if (rotateState == 1)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationX -= 2;
                        }
                    }
                    if (rotateState == 2)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationX += 2;
                        }
                        rotateState = 0;
                    }
                }
                #endregion

                #region pattern 4
                if (pattern == 4)
                {
                    if (rotateState == 1)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationY -= 2;
                        }
                    }
                    if (rotateState == 2)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationY += 2;
                        }
                        rotateState = 0;
                    }
                }
                #endregion

                #region pattern 5
                if (pattern == 5)
                {
                    if (rotateState == 1)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationX--;
                            locationY--;
                        }
                    }
                    if (rotateState == 2)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationX++;
                            locationY--;
                        }
                    }
                    if (rotateState == 3)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationX++;
                            locationY++;
                        }
                    }
                    if (rotateState == 4)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationX--;
                            locationY++;
                        }
                        rotateState = 0;
                    }
                }
                #endregion

                #region pattern 6
                if (pattern == 6)
                {
                    if (rotateState == 1)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationY -= 2;
                        }
                    }
                    if (rotateState == 2)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationX += 2;
                        }
                    }
                    if (rotateState == 3)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationY += 2;
                        }
                    }
                    if (rotateState == 4)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationX -= 2;
                        }
                        rotateState = 0;
                    }
                }
                #endregion

                #region pattern 7
                if (pattern == 7)
                {
                    if (rotateState == 1)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationX -= 2;
                        }
                    }
                    if (rotateState == 2)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 3)
                        {
                            locationY -= 2;
                        }
                    }
                    if (rotateState == 3)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX--;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX++;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationX += 2;
                        }
                    }
                    if (rotateState == 4)
                    {
                        if (index == 0)
                        {

                        }
                        if (index == 1)
                        {
                            locationX++;
                            locationY--;
                        }
                        if (index == 2)
                        {
                            locationX--;
                            locationY++;
                        }
                        if (index == 3)
                        {
                            locationY += 2;
                        }
                        rotateState = 0;
                    }
                }
                #endregion

                rTimer = 0.0f;
            }
            if (xYR == 1)
                return locationX;
            if (xYR == 2)
                return locationY;
            if (xYR == 3)
                return rotateState;
            else
                return 0;
        }

        //Draws the next pattern
        public void DrawNextPattern(SpriteBatch spriteBatch, int nextPattern)
        {
            switch (nextPattern)
            {
                case 1:
                    spriteBatch.Draw(block, new Vector2(400, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(430, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 130), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(430, 130), Color.LightGray);
                    break;

                case 2:
                    spriteBatch.Draw(block, new Vector2(400, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 130), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 160), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 190), Color.LightGray);
                    break;

                case 3:
                    spriteBatch.Draw(block, new Vector2(400, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(370, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 130), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(430, 130), Color.LightGray);
                    break;

                case 4:
                    spriteBatch.Draw(block, new Vector2(400, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(430, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 130), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(370, 130), Color.LightGray);
                    break;

                case 5:
                    spriteBatch.Draw(block, new Vector2(400, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(430, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(370, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 130), Color.LightGray);
                    break;

                case 6:
                    spriteBatch.Draw(block, new Vector2(400, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 130), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 160), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(370, 160), Color.LightGray);
                    break;

                case 7:
                    spriteBatch.Draw(block, new Vector2(400, 100), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 130), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(400, 160), Color.LightGray);
                    spriteBatch.Draw(block, new Vector2(430, 160), Color.LightGray);
                    break;

                default:

                    break;
            }
        }
    }
}

