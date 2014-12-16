using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetroXNA
{
    public class SaveGameClass
    {
        private bool[,] so = new bool[10, 20];
        private int loadedScore, loadedLevel, loadedTotalClearedLines;
        private static string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private string path = baseFolder + @"Save Games\savedGameData.txt";
        FileStream theFileRead;
        FileStream theFileWrite;
        StreamReader saveRead;
        StreamWriter saveWrite;

        public int getLoadedScore() { return loadedScore; }
        public int getLoadedLevel() { return loadedLevel; }
        public int getLoadedTotalClearedLines() { return loadedTotalClearedLines; }

        public SaveGameClass()
        {
            System.IO.Directory.CreateDirectory("Save Games");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void recordGameData(bool[,] st, int sc, int lv, int lns/*test*/)
        {
            try
            {
                theFileWrite = new FileStream(path,
                                              FileMode.Create,
                                              FileAccess.Write);
                saveWrite = new StreamWriter(theFileWrite);

                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        saveWrite.WriteLine(st[x, y].ToString());
                    }
                }
                saveWrite.WriteLine(sc.ToString());
                saveWrite.WriteLine(lv.ToString());
                saveWrite.WriteLine(lns.ToString());

                saveWrite.Close();
                theFileWrite.Close();
            }
            catch
            {
                
            }
        }

        public bool[,] loadGameData()
        {
            try
            {
                theFileRead = new FileStream(path,
                                             FileMode.Open,
                                             FileAccess.Read);
                saveRead = new StreamReader(theFileRead);

                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        so[x, y] = Convert.ToBoolean(saveRead.ReadLine());
                    }
                }
                loadedScore = Convert.ToInt32(saveRead.ReadLine());
                loadedLevel = Convert.ToInt32(saveRead.ReadLine());
                loadedTotalClearedLines = Convert.ToInt32(saveRead.ReadLine());

                saveRead.Close();
                theFileRead.Close();
            }
            catch
            {

            }
            return so;
        }
    }
}


