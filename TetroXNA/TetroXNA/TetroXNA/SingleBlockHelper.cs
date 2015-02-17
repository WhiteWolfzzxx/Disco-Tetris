using System;
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
        //This class is for single block properties that the player controls
        //This class acts as a child class to the block helper class because this class returns different flags about bloc properties
        //That the block helper interacts with
        private bool[,] store;
        private bool
            stopActiveBlocks, 
            canRotateBlocksFlag = true,
            cantRotateOtherBlockFlag = false,
            canGoLeftFlag = true,
            otherCantGoLeftFlag = false,
            canGoRightFlag = true,
            otherCantGoRightFlag = false,        
            blockCollideBottomFlag = false,
            canGoDownFlag = true;
        private int
            rotateState = 0,
            locationX, locationY, pattern, index, level,
            redIntensity, blueIntensity, greenIntensity,
            nextPattern,
            score = 0;
        private float 
            moveTimer = 0.0f,
            minMoveTimer = 0.1f,
            downTimer = 0.0f,
            minDownTimer = 0.0f,
            rotateTimer = 0.0f,
            minRotateTimer = 0.2f;
        private Random random = new Random();
        private SpecialEffects specialEffects = new SpecialEffects();
        private BlockConFigClass blockConFigClass;
        private Texture2D block;
        private Vector2[,] lines;

        public bool getCanGoDownFlag() { return canGoDownFlag;}
        public bool getBlockCollideBottomFlag() { return blockCollideBottomFlag; }
        public bool getStopActiveBlocks() { return stopActiveBlocks; }
        public bool getCanRotateBlocksFlag() { return canRotateBlocksFlag; }
        public bool getCanGoLeftFlag() { return canGoLeftFlag; }
        public bool getCanGoRightFlag() { return canGoRightFlag; }
        public int getPattern() { return pattern; }
        public int getNextPattern() { return nextPattern; }
        public int getScore() { return score; }
        public float getMinTimer() { return minDownTimer; }

        public void setCanGoDownFlag(bool cd) { canGoDownFlag = cd; }
        public void setBlockCollideBottomFlag(bool bcb) { blockCollideBottomFlag = bcb; } 
        public void setStopActiveBlocks(bool stop) { stopActiveBlocks = stop; }
        public void setCantRotateOtherBlocksFlag(bool rotate) { cantRotateOtherBlockFlag = rotate; }
        public void setOtherCantGoLeftFlag(bool left) { otherCantGoLeftFlag = left; }
        public void setOtherCantGoRightFlag(bool right) { otherCantGoRightFlag = right; }
        public void setStore(bool[,] st) { store = st; }
        public void setLevel(int lv) { level = lv; }
        public void setNextPattern(int patt) { nextPattern = patt; }
        public void setPattern(int pat) { pattern = pat; }

        public SingleBlockHelper(int locationX1, int locationY1, Texture2D block1, Vector2[,] lns, int patt, int inx, bool[,] st, int np)
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
            nextPattern = np;
        }

        //Update order for the 4 player controled blocks
        public bool[,] SingleBlockHelperUpdate(GameTime gameTime)
        {
            //Set timers
            HandleKeyboardInput(Keyboard.GetState(), store);
            moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            downTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Set colors
            redIntensity = specialEffects.getRed();
            blueIntensity = specialEffects.getBlue();
            greenIntensity = specialEffects.getGreen();
            specialEffects.colorChanger();

            canRotateBlockFlag();
            checkLeftFlag();
            checkRightFlag();

            //Flag if the bottom of the blocks are hitting something
            //Boolean if statement compressed
            blockCollideBottomFlag = ((locationY == 19) || (store[locationX, (locationY + 1)] == true) || (stopActiveBlocks == true));

            //PlayerBlock will move downward forcefully
            if ((downTimer >= minDownTimer) && canGoDownFlag)
            {
                locationY += 1;
                downTimer = 0.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && (downTimer >= minDownTimer - .02f)) //slows the score incriment down while holding down
                score = level;
            else
                score = 0;
            return store;
        }

        //Records the location of the player controlled blocks
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

        //Checks to detect other blocks or boundaries to rotate
        private void canRotateBlockFlag()
        {
            canRotateBlocksFlag = !(
                (locationY == 0) ||
                (locationX == 0) ||
                (locationX == 9) ||
                (store[(locationX - 1), locationY] == true) ||
                (store[(locationX + 1), locationY] == true));
        }

        //Player input and level manageing is here
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
            //Level increment speed
            if (keyState.IsKeyDown(Keys.Down))
            {
                minDownTimer = 0.05f;
                if (locationY >= 19)
                    locationY = 19;
            }
            else
            {
                if (level < 18)
                    minDownTimer = 1.0f - ((level - 1) * 0.05f);
                else
                    minDownTimer = 0.2f;
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

        //Constructor for player controlled blocks
        public void resetBlocks()
        {
            blockConFigClass = new BlockConFigClass(pattern, index, block);
            resetPlayerBlockPos();
            rotateState = 0;
        }

        //Constructor reference to relocate player controlled blocks
        public void resetPlayerBlockPos()
        {
            locationX = blockConFigClass.resetPattern("X");
            locationY = blockConFigClass.resetPattern("Y");
        }

        //Collision detections for player blocks
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
