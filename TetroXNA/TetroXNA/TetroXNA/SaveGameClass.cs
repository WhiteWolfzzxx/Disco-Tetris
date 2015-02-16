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
    //This class saves all of the bool values of the stored blocks into a flat file
    //This class also loads the file so the player can resume their game
    public class SaveGameClass
    {
        private bool[,] so = new bool[10, 20];
        private int loadedScore, loadedLevel, loadedTotalClearedLines;
        private static string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private string path = baseFolder + @"Save Games\savedGameData.txt";
        private ErrorHandler errorHandler = new ErrorHandler();
        private FileStream theFileRead;
        private FileStream theFileWrite;
        private StreamReader saveRead;
        private StreamWriter saveWrite;

        public int getLoadedScore() { return loadedScore; }
        public int getLoadedLevel() { return loadedLevel; }
        public int getLoadedTotalClearedLines() { return loadedTotalClearedLines; }

        public SaveGameClass()
        {
            System.IO.Directory.CreateDirectory("Save Games");
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
                errorHandler.recordError(2, 200, "Save game has failed.", "Save game class has failed to save game");
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
                        so[x, y] = Convert.ToBoolean(saveRead.ReadLine());
                }
                loadedScore = Convert.ToInt32(saveRead.ReadLine());
                loadedLevel = Convert.ToInt32(saveRead.ReadLine());
                loadedTotalClearedLines = Convert.ToInt32(saveRead.ReadLine());

                saveRead.Close();
                theFileRead.Close();
            }
            catch
            {
                errorHandler.recordError(2, 201, "Load game has failed.", "Save game class has failed to load game");
            }
            return so;
        }
    }
}


