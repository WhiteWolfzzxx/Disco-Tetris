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
            pattern,
            score,
            level = 1,
            totalClearedLines = 0,
            clearedLines = 0,
            nextPattern = 2;
        private float 
            randTimer,
            minRandTimer = 0.01f,
            stopBlocksTimer;
        private SingleBlockHelper[] activeBlocks;
        private Color[] color = new Color[10];
        private Color[,] blockColor = new Color[10, 20];
        private Random random = new Random();
        private Vector2[,] lines;
        private SoundEffect 
            blockGroundSoundEffect,
            lineClearedSoundEffect;

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

            //Sets all colors
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                    blockColor[x, y] = color[3];
            }
        }

        //Constructor
        public BlockHelper(SingleBlockHelper[] atb, Vector2[,] ln, bool[,] st, SoundEffect bl, SoundEffect lc)
        {
            activeBlocks = atb;
            lines = ln;
            store = st;
            blockGroundSoundEffect = bl;
            lineClearedSoundEffect = lc;
            patternRandomizer();
        }

        //Tasks in update order
        public void BlockHelperUpdate(GameTime gameTime)
        {
            randTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            stopBlocksTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            stopBlocks();
            syncActiveBlocks();
            canRotateBlocks();
            canGoLeft();
            canGoRight();
            UpdatePlayerClass(gameTime);
            resetPlayerBlocks();
            lineDetection();
            levelDetection();
            randomColors();

            //Corrects the blocks so they are on the same level
            for (int i = 0; i < activeBlocks.Length; i++)
                activeBlocks[i].setLevel(level);

            //Adds hold down score
            score += activeBlocks[0].getScore();
        }

        //Block hits bottom algorithm
        //Controls the steps to eventually save the positions
        private void stopBlocks()
        {
            //If one player block has something under it
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
                    blockGroundSoundEffect.Play();
                    for (int i = 0; i < activeBlocks.Length; i++)
                    {
                        //Score for blocks landed
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

        //Level manager
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
            //Logic:
            //Checks top to down for a full line
            //From found line it shifts the blocks above down 1
            for (int i = 1; i < 20; i++)
            {
                if ((store[0, i] && store[1, i] && store[2, i] && store[3, i] && store[4, i] &&
                    store[5, i] && store[6, i] && store[7, i] && store[8, i] && store[9, i]) == true)
                {
                    for (int x = i; x > 0; x--)
                    {
                        for (int z = 0; z < 10; z++ )
                            store[z, x] = store[z, x - 1];
                    }
                    score += (1500 + (100 * (level - 1)));
                    clearedLines++;
                    totalClearedLines++;
                    lineClearedSoundEffect.Play();
                }
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
                patternRandomizer();
                for (int i = 0; i < activeBlocks.Length; i++)
                {
                    activeBlocks[i].setStopActiveBlocks(false);
                    activeBlocks[i].setNextPattern(nextPattern);
                    activeBlocks[i].setPattern(pattern);
                    activeBlocks[i].resetBlocks();
                }
            }
        }

        private void patternRandomizer()
        {
            pattern = nextPattern;
            nextPattern = random.Next(1, 8);
        }

        //Update the game board and player blocks
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

        //If one player controlled block stops all stop
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
        //If one player block can't rotate then none can
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
                    store,
                    nextPattern);
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

