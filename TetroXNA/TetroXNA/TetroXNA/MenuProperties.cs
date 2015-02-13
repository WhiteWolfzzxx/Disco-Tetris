using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public int getRed() { return redIntensity; }
        public int getBlue() { return blueIntensity; }
        public int getGreen() { return greenIntensity; }

        public MenuProperties()
        {
           
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
    }
}
