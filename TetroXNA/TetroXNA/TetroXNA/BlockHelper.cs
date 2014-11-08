using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TetroXNA
{
    //Class is for general properties of multiple block interactions
    public class BlockHelper
    {
        private SingleBlockHelper[] activeBlocks;
        private Color[] color = new Color[10];
        private Color[,] blockColor = new Color[10, 20];
        private Random random = new Random();
        private Vector2[,] lines;
        private bool[,] store;
        private int pattern = 2;
        private int score;
        private int level = 1;
        private int totalClearedLines = 0;
        private int clearedLines = 0;
        private float lineCheckTimer = 0.2f;
        private float minLineCheckTimer = 1.0f;
        private float randTimer;
        private float minRandTimer = 0.01f;
        private Song playBGM;

        public void setStore(bool[,] st) { store = st; }
        public void setScore(int sc) { score = sc; }
        public void setLevel(int lv) { level = lv; }
        public void setTotalClearedLine(int ln) { totalClearedLines = ln; }

        public BlockHelper(SingleBlockHelper[] atb, Vector2[,] ln, bool[,] st, Song pl)
        {
            activeBlocks = atb;
            lines = ln;
            store = st;
            playBGM = pl;
        }

        public void BlockHelperUpdate(GameTime gameTime) 			////////////////UPADATE!!!!!
        {
            lineCheckTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            randTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            syncActiveBlocks();
            canRotateBlocks();
            canGoLeft();
            canGoRight();
            UpdatePlayerClass(gameTime);
            resetPlayerBlocks();			//MUST BE LAST TO UPDATE
            lineDetection();
            levelDetection();
            randomColors();
            for (int i = 0; i < activeBlocks.Length; i++)
            {
                activeBlocks[i].setLevel(level);
            }
        }

        public int getLevel()
        {
            return level;
        }

        private void levelDetection()
        {
            if (clearedLines >= 10)
            {
                clearedLines = 0;
                level++;
                MediaPlayer.Play(playBGM);
            }
        }

        private void randomColors()
        {
            if (randTimer > minRandTimer)
            {
                blockColor[random.Next(0, 10), random.Next(0, 20)] = color[random.Next(0, 10)];
                randTimer = 0.0f;
            }
        }

        //Change to add colors
        public void setColors()
        {
            color[0] = Color.LightGray;
            color[1] = Color.Blue;
            color[2] = Color.Yellow;
            color[3] = Color.Green;
            color[4] = Color.Orange;
            color[5] = Color.Purple;
            color[6] = Color.Red;
            color[7] = Color.Silver;
            color[8] = Color.Brown;
            color[9] = Color.MintCream;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    blockColor[x, y] = Color.LightGray;
                }
            }
        }

        private void lineDetection()
        {
            if (lineCheckTimer >= minLineCheckTimer)
            {
                for (int i = 1; i < 20; i++)
                {
                    if ((store[0, i] && store[1, i] && store[2, i] && store[3, i] && store[4, i] &&
                        store[5, i] && store[6, i] && store[7, i] && store[8, i] && store[9, i]) == true)
                    {
                        for (int x = i; x > 0; x--)
                        {
                            store[0, x] = store[0, x - 1];
                            store[1, x] = store[1, x - 1];
                            store[2, x] = store[2, x - 1];
                            store[3, x] = store[3, x - 1];
                            store[4, x] = store[4, x - 1];
                            store[5, x] = store[5, x - 1];
                            store[6, x] = store[6, x - 1];
                            store[7, x] = store[7, x - 1];
                            store[8, x] = store[8, x - 1];
                            store[9, x] = store[9, x - 1];
                        }
                        score += (40 * level);
                        clearedLines++;
                        totalClearedLines++;
                    }
                }
                lineCheckTimer = 0.0f;
            }
            else
            {
                //lineCheckTimer = 0.0f;
            }
        }

        public int getTotalClearedLines()
        {
            return totalClearedLines;
        }

        //return score
        public int getScore()
        {
            return score;
        }

        //Resets player blocks for the next pattern
        private void resetPlayerBlocks()
        {
            if ((activeBlocks[0].getStopActiveBlocks() == true) &&
                (activeBlocks[1].getStopActiveBlocks() == true) &&
                (activeBlocks[2].getStopActiveBlocks() == true) &&
                (activeBlocks[3].getStopActiveBlocks() == true))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setStopActiveBlocks(false);
                    activeBlocks[i].resetBlocks();
                }
            }
        }

        //Update the game grid and player blocks
        private void UpdatePlayerClass(GameTime gameTime)
        {
            for (int i = 0; i < activeBlocks.Length; i++)
            {
                store = activeBlocks[i].SingleBlockHelperUpdate(gameTime);
            }
        }

        //Detects other blocks to see if they can't go Right
        private void canGoRight()
        {
            if ((activeBlocks[0].getCanGoRight() == true) &&
                (activeBlocks[1].getCanGoRight() == true) &&
                (activeBlocks[2].getCanGoRight() == true) &&
                (activeBlocks[3].getCanGoRight() == true))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setOtherCantGoRight(false);
                }
            }
            else
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setOtherCantGoRight(true);
                }
            }
        }

        //Detects other blocks to see if they can't go left
        private void canGoLeft()
        {
            if ((activeBlocks[0].getCanGoLeft() == true) &&
                (activeBlocks[1].getCanGoLeft() == true) &&
                (activeBlocks[2].getCanGoLeft() == true) &&
                (activeBlocks[3].getCanGoLeft() == true))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setOtherCantGoLeft(false);
                }
            }
            else
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setOtherCantGoLeft(true);
                }
            }
        }

        //Block syic detect and reasign so all blocks stop
        private void syncActiveBlocks()
        {
            if ((activeBlocks[0].getStopActiveBlocks() == true) ||
                (activeBlocks[1].getStopActiveBlocks() == true) ||
                (activeBlocks[2].getStopActiveBlocks() == true) ||
                (activeBlocks[3].getStopActiveBlocks() == true))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setStopActiveBlocks(true);
                }
            }
        }

        //Detects other blocks to see if they can't rotate and assigns other blocks to stop rotation
        private void canRotateBlocks()
        {
            if ((activeBlocks[0].getCanRotateBlocks() == true) &&
                (activeBlocks[1].getCanRotateBlocks() == true) &&
                (activeBlocks[2].getCanRotateBlocks() == true) &&
                (activeBlocks[3].getCanRotateBlocks() == true))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setCantRotateOtherBlocks(false);
                }
            }
            else
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setCantRotateOtherBlocks(true);
                }
            }
        }

        public void setActiveBlocks(SingleBlockHelper[] atb)
        {
            activeBlocks = atb;
        }

        public SingleBlockHelper[] getActiveBlocks()
        {
            return activeBlocks;
        }

        public SingleBlockHelper[] loadPlayerBlocks(ContentManager Content)
        {
            for (int i = 0; i < activeBlocks.Length; i++)
            {
                activeBlocks[i] = new SingleBlockHelper(
                    4,
                    0,
                    Content.Load<Texture2D>(@"Textures\TetrusBlock1"),
                    lines,
                    pattern,
                    i,
                    store);
            }
            return activeBlocks;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D[,] blocks)
        {
            for (int z = 0; z < 10; z++)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (store[z, i] == true)
                    {
                        spriteBatch.Draw(blocks[z, i], lines[z, i], blockColor[z, i]);
                    }
                }
            }

        }

    }
}

