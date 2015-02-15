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
    //Mainly for the interaction of the four blocks controlled by the player and how it interacts with the stored blocks
    public class BlockHelper
    {
        private bool[,] store;
        private int 
            pattern = 2,
            score,
            level = 1,
            totalClearedLines = 0,
            clearedLines = 0;
        private float 
            lineCheckTimer = 0.2f,
            minLineCheckTimer = 1.0f,
            randTimer,
            minRandTimer = 0.01f,
            stopBlocksTimer;
        private SingleBlockHelper[] activeBlocks;
        private Color[] color = new Color[10];
        private Color[,] blockColor = new Color[10, 20];
        private Random random = new Random();
        private Vector2[,] lines;

        public int getLevel() { return level; }
        public int getTotalClearedLines() { return totalClearedLines; }
        public int getScore() { return score; }
        public SingleBlockHelper[] getActiveBlocks() { return activeBlocks; }

        public void setStore(bool[,] st) { store = st; }
        public void setScore(int sc) { score = sc; }
        public void setLevel(int lv) { level = lv; }
        public void setTotalClearedLine(int ln) { totalClearedLines = ln; }
        public void setActiveBlocks(SingleBlockHelper[] atb) { activeBlocks = atb; }
        public void setColors()
        {
            color[0] = Color.Red;
            color[1] = Color.Orange;
            color[2] = Color.Yellow;
            color[3] = Color.Green;
            color[4] = Color.Blue;
            color[5] = Color.Indigo;
            color[6] = Color.Violet;
            color[7] = Color.Pink;
            color[8] = Color.Brown;
            color[9] = Color.MintCream;

            //sets all colors
            //TODO set to random color
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                    blockColor[x, y] = color[3];
            }
        }

        //Constructor
        public BlockHelper(SingleBlockHelper[] atb, Vector2[,] ln, bool[,] st)
        {
            activeBlocks = atb;
            lines = ln;
            store = st;
        }

        //tasks in update order
        public void BlockHelperUpdate(GameTime gameTime) 			////////////////UPADATE!!!!!
        {
            lineCheckTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            randTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            stopBlocksTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            stopBlocks();
            syncActiveBlocks();
            canRotateBlocks();
            canGoLeft();
            canGoRight();
            UpdatePlayerClass(gameTime);
            resetPlayerBlocks();			//MUST BE LAST TO UPDATE
            lineDetection();
            levelDetection();
            randomColors();

            //corrects the blocks so they are on the same level
            for (int i = 0; i < activeBlocks.Length; i++)
                activeBlocks[i].setLevel(level);

            //Adds hold down score
            score += activeBlocks[0].getScore();
        }

        //block hits bottom algorithm
        //controls the steps to eventually save the positions
        private void stopBlocks()
        {
            //If one player_block has something under it
            //Stops other player blocks going down if one can't and timer counts down then save positions
            if (activeBlocks[0].getBlockCollideBottomFlag() ||
                activeBlocks[1].getBlockCollideBottomFlag() ||
                activeBlocks[2].getBlockCollideBottomFlag() ||
                activeBlocks[3].getBlockCollideBottomFlag())
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setCanGoDownFlag(false);
                if (stopBlocksTimer >= activeBlocks[0].getMinTimer())
                {
                    for (int i = 0; i < activeBlocks.Length; i++)
                    {
                        //score for blocks landed
                        score += 25 + ((level - 1) * 2);
                        activeBlocks[i].savePositions();
                        activeBlocks[i].setBlockCollideBottomFlag(false);
                    }
                    stopBlocksTimer = 0.0f;
                }
            }
            else
            {
                stopBlocksTimer = 0.0f;
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setCanGoDownFlag(true);
            }
        }

        //LEVEL ALGORITHEM
        private void levelDetection()
        {
            if (clearedLines >= 10)
            {
                clearedLines = 0;
                level++;
            }
        }

        //Save blocks color randomizer
        private void randomColors()
        {
            if (randTimer > minRandTimer)
            {
                blockColor[random.Next(0, 10), random.Next(0, 20)] = color[random.Next(0, 10)];
                randTimer = 0.0f;
            }
        }

        //Controls how lines are cleared and found
        //Controls scoring when a line is cleared
        private void lineDetection()
        {
            if (lineCheckTimer >= minLineCheckTimer)
            {
                //Logic:
                //checks top to down for a full line
                //from found line it shifts the block above down 1
                //Timer is present to reduce bug errors
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
                        score += (1500 + (100 * (level - 1)));
                        clearedLines++;
                        totalClearedLines++;
                    }
                }
                lineCheckTimer = 0.0f;
            }
        }

        //Resets player blocks for the next pattern
        private void resetPlayerBlocks()
        {
            if ((activeBlocks[0].getStopActiveBlocks()) &&
                (activeBlocks[1].getStopActiveBlocks()) &&
                (activeBlocks[2].getStopActiveBlocks()) &&
                (activeBlocks[3].getStopActiveBlocks()))
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
                store = activeBlocks[i].SingleBlockHelperUpdate(gameTime);
        }

        //Detects other blocks to see if they can't go Right
        private void canGoRight()
        {
            if ((activeBlocks[0].getCanGoRightFlag()) &&
                (activeBlocks[1].getCanGoRightFlag()) &&
                (activeBlocks[2].getCanGoRightFlag()) &&
                (activeBlocks[3].getCanGoRightFlag()))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setOtherCantGoRightFlag(false);
            }
            else
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setOtherCantGoRightFlag(true);
            }
        }

        //Detects other blocks to see if they can't go left
        private void canGoLeft()
        {
            if ((activeBlocks[0].getCanGoLeftFlag()) &&
                (activeBlocks[1].getCanGoLeftFlag()) &&
                (activeBlocks[2].getCanGoLeftFlag()) &&
                (activeBlocks[3].getCanGoLeftFlag()))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setOtherCantGoLeftFlag(false);
            }
            else
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setOtherCantGoLeftFlag(true);
            }
        }

        //Block syic detect and reassign so all blocks stop
        //AKA if one player controlled block stops all stop
        private void syncActiveBlocks()
        {
            if ((activeBlocks[0].getStopActiveBlocks()) ||
                (activeBlocks[1].getStopActiveBlocks()) ||
                (activeBlocks[2].getStopActiveBlocks()) ||
                (activeBlocks[3].getStopActiveBlocks()))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setStopActiveBlocks(true);
            }
        }

        //Detects other blocks to see if they can't rotate and assigns other blocks to stop rotation
        //If one player block can't rotate, none can
        private void canRotateBlocks()
        {
            if ((activeBlocks[0].getCanRotateBlocksFlag() == true) &&
                (activeBlocks[1].getCanRotateBlocksFlag() == true) &&
                (activeBlocks[2].getCanRotateBlocksFlag() == true) &&
                (activeBlocks[3].getCanRotateBlocksFlag() == true))
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setCantRotateOtherBlocksFlag(false);
            }
            else
            {
                for (int i = 0; i < activeBlocks.Length; i++)
                    activeBlocks[i].setCantRotateOtherBlocksFlag(true);
            }
        }

        //Load textures for player blocks
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

        //Draw all store[] blocks
        //If store[z,i] == true then show that block on screen
        public void Draw(SpriteBatch spriteBatch, Texture2D[,] blocks)
        {
            for (int z = 0; z < 10; z++)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (store[z, i] == true)
                        spriteBatch.Draw(blocks[z, i], lines[z, i], blockColor[z, i]);
                }
            }
        }
    }
}

