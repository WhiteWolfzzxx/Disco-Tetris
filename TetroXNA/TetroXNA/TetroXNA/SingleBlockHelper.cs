﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetroXNA
{
    public class SingleBlockHelper
    {
        //this class is for single block properties
        private bool[,] store;
        private bool
            stopActiveBlocks, 		//This is used to syic the blocks together
            canRotateBlocksFlag = true,
            cantRotateOtherBlockFlag = false,
            canGoLeftFlag = true,
            otherCantGoLeftFlag = false,
            canGoRightFlag = true,
            otherCantGoRightFlag = false,        
            blockCollideBottomFlag = false,
            canGoDown = true;
        private int 
            rotateState = 0,
            locationX, locationY, pattern, index, level,
            redIntensity, blueIntensity, greenIntensity,
            nextPattern = 4;
        private float 
            moveTimer = 0.0f,
            minMoveTimer = 0.1f,
            downTimer = 0.0f,
            minDownTimer = 0.0f,
            rotateTimer = 0.0f,
            minRotateTimer = 0.2f;
        private Random random = new Random();
        private MenuProperties menuProperties = new MenuProperties();
        private BlockConFigClass blockConFigClass;
        private Texture2D block;
        private Vector2[,] lines;

        public bool getCanGoDown() { return canGoDown;}
        public bool getBlockCollideBottom() { return blockCollideBottomFlag; }
        public bool getStopActiveBlocks() { return stopActiveBlocks; }
        public bool getCanRotateBlocks() { return canRotateBlocksFlag; }
        public bool getCanGoLeft() { return canGoLeftFlag; }
        public bool getCanGoRight() { return canGoRightFlag; }
        public int getPattern() { return pattern; }
        public int getNextPattern() { return nextPattern; }
        public float getMinTimer() { return minDownTimer; }

        public void setCanGoDown(bool cd) { canGoDown = cd; }
        public void setBlockCollideBottom(bool bcb) { blockCollideBottomFlag = bcb; } 
        public void setStopActiveBlocks(bool stop) { stopActiveBlocks = stop; }
        public void setCantRotateOtherBlocks(bool rotate) { cantRotateOtherBlockFlag = rotate; }
        public void setOtherCantGoLeft(bool left) { otherCantGoLeftFlag = left; }
        public void setOtherCantGoRight(bool right) { otherCantGoRightFlag = right; }
        public void setStore(bool[,] st) { store = st; }
        public void setLevel(int lv) { level = lv; }
        public void setNextPattern(int patt) { nextPattern = patt; }

        public SingleBlockHelper(int locationX1, int locationY1, Texture2D block1, Vector2[,] lns, int patt, int inx, bool[,] st)
        {
            locationX = locationX1;
            locationY = locationY1;
            block = block1;
            lines = lns;
            pattern = patt;
            index = inx;
            blockConFigClass = new BlockConFigClass(pattern, index, block1);
            resetPlayerBlockPos();
            store = st;
        }

        public bool[,] SingleBlockHelperUpdate(GameTime gameTime)
        {
            //Set timers
            HandleKeyboardInput(Keyboard.GetState(), store);
            moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            downTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //set colors
            redIntensity = menuProperties.getRed();
            blueIntensity = menuProperties.getBlue();
            greenIntensity = menuProperties.getGreen();
            menuProperties.colorChanger();

            canRotateBlockFlag();
            checkLeftFlag();
            checkRightFlag();

            //Flag if the bottom of the blocks are hitting something
            //boolean if statment compressed
            blockCollideBottomFlag = ((locationY == 19) || (store[locationX, (locationY + 1)] == true) || (stopActiveBlocks == true));

            //PlayerBlock will move downward forcefully HAHAHA!!!!
            if ((downTimer >= minDownTimer) && canGoDown)
            {
                locationY += 1;
                downTimer = 0.0f;
            }
            return store;
        }

        //records the location of the player controlled blocks
        public void savePositions()
        {
            stopActiveBlocks = true;
            store[locationX, locationY] = true;
        }

        //Checks to see if blocks can go right
        private void checkRightFlag()
        {
            canGoRightFlag = !((locationX == 9) || store[(locationX + 1), locationY]);
        }

        //Checks to see if blocks can go left
        private void checkLeftFlag()
        {
            canGoLeftFlag = !((locationX == 0) || store[(locationX - 1), locationY]);
        }

        //Checks to detect other blocks or bounderies to rotate
        private void canRotateBlockFlag()
        {
            canRotateBlocksFlag = !(
                (locationY == 0) ||
                (locationX == 0) ||
                (locationX == 9) ||
                (store[(locationX - 1), locationY] == true) ||
                (store[(locationX + 1), locationY] == true));
        }

        //CONTROL DETECTION AND FUNTION HERE
        private void HandleKeyboardInput(KeyboardState keyState, bool[,] store)
        {
            //Checks flags to rotate
            if ((keyState.IsKeyDown(Keys.Up)) && canRotateBlocksFlag && !cantRotateOtherBlockFlag)
            {
                locationX = blockConFigClass.rotatePattern(locationX, locationY, rotateTimer, minRotateTimer, rotateState, 1);
                locationY = blockConFigClass.rotatePattern(locationX, locationY, rotateTimer, minRotateTimer, rotateState, 2);
                rotateState = blockConFigClass.rotatePattern(locationX, locationY, rotateTimer, minRotateTimer, rotateState, 3);

                if (blockConFigClass.getRotateTimer() == 0.0f)
                    rotateTimer = 0.0f;
            }
            //Shorten the minDown Timer to make the blocks fall faster
            //LEVEL INCRIMENT SPEED AGORITHEM IS HERE
            if (keyState.IsKeyDown(Keys.Down))
            {
                minDownTimer = 0.05f;
                if (locationY >= 19)
                    locationY = 19;
            }
            else
            {
                if (level < 6)
                    minDownTimer = 1.0f - ((level - 1) * 0.2f);
                else
                    minDownTimer = 0.1f;
            }
            //Checks flags to go left
            if (keyState.IsKeyDown(Keys.Left) && canGoLeftFlag && !otherCantGoLeftFlag)
            {
                if (moveTimer >= minMoveTimer)
                {
                    locationX -= 1;
                    moveTimer = 0.0f;
                }
                checkBounds(store);
            }
            //Checks flags to go right
            if (keyState.IsKeyDown(Keys.Right) && canGoRightFlag && !otherCantGoRightFlag)
            {
                if (moveTimer >= minMoveTimer)
                {
                    locationX += 1;
                    moveTimer = 0.0f;
                }
                checkBounds(store);
            }
        }

        //Constructor for player controled blocks
        //PATTERN ALGORITHEM HERE
        public void resetBlocks()
        {
            pattern = nextPattern;
            nextPattern = random.Next(1, 8);		////////////THIS IS THE PATTERN RANDOMIZER: CHANGE WHEN TESTING!!!!!
            blockConFigClass = new BlockConFigClass(pattern, index, block);
            resetPlayerBlockPos();
            rotateState = 0;
        }

        //Constructor refrance to relocate player controled blocks
        public void resetPlayerBlockPos()
        {
            locationX = blockConFigClass.resetPattern("X");
            locationY = blockConFigClass.resetPattern("Y");
        }

        //Colision detections for player blocks
        private void checkBounds(bool[,] store)
        {
            if (locationX > 9)
                locationX = 9;
            else if (locationX < 0)
                locationX = 0;
            else if (store[locationX, locationY] == true)
                locationX -= 1;
            else if (store[locationX, locationY] == true)
                locationX += 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(block, lines[locationX, locationY], new Color(redIntensity, blueIntensity, greenIntensity));
            blockConFigClass.DrawNextPattern(spriteBatch, nextPattern);
        }
    }
}
