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
    //This class for creating the game board so the math is easier for the programmer to understand for block placement
    public class BoardClass
    {
        private bool[,] store;
        private Texture2D[,] blocks;
        private Vector2[,] lines;

        public BoardClass(Texture2D[,] bl, bool[,] st, Vector2[,] ln)
        {
            blocks = bl;
            store = st;
            lines = ln;
        }

        //Constructor for the grid placement x,y
        //This will set up the board so the programmers can use simple numbers for placement instead of pixels
        public Vector2[,] resetLinesGrid()
        {
            for (int z = 0; z < 10; z++)
            {
                for (int i = 0; i < 20; i++)
                    lines[z, i] = new Vector2(16 + (30 * z), (30 * i));
            }
            return lines;
        }

        //Constructor for store array
        //Used for keeping track of blocks on the board that the player doesn't control
        public bool[,] resetStore()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                    store[x, y] = false;
            }
            return store;
        }

        //Used to load Textures for all blocks in the board except the player blocks
        public Texture2D[,] loadBlocksTexture(ContentManager Content)
        {
            for (int z = 0; z < 10; z++)
            {
                for (int i = 0; i < 20; i++)
                    blocks[z, i] = Content.Load<Texture2D>(@"Textures\TetrusBlock1");
            }
            return blocks;
        }
    }
}