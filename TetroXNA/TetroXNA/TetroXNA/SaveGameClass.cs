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
    //This class saves all of the boolean values of the stored blocks into a flat file
    //This class also loads the file so the player can resume their game
    public class SaveGameClass
    {
        private bool[,] so = new bool[10, 20];
        private int loadedScore, loadedLevel, loadedTotalClearedLines;
        private static string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private string path = baseFolder + @"Save Games\savedGameData.txt";
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

        public void recordGameData(bool[,] st, int sc, int lv, int lns)
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
                        saveWrite.WriteLine(StringCipher.Encrypt(st[x, y].ToString(), "w1i7BI&B3J.H2m6SX{jOQ&$NR*33P0"));
                    }
                }
                saveWrite.WriteLine(StringCipher.Encrypt(sc.ToString(), "'!Mqi3C0tZENCuw~9aF64/I[j50?e$"));
                saveWrite.WriteLine(StringCipher.Encrypt(lv.ToString(), "w1i7BI&B3J.H2m6SX{jOQ&$NR*33P0"));
                saveWrite.WriteLine(StringCipher.Encrypt(lns.ToString(), "w1i7BI&B3J.H2m6SX{jOQ&$NR*33P0"));

                saveWrite.Close();
                theFileWrite.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Saving has failed.");
                ErrorHandler.recordError(2, 200, "Saving the game has failed.", e.ToString());
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
                        so[x, y] = Convert.ToBoolean(StringCipher.Decrypt(saveRead.ReadLine(), "w1i7BI&B3J.H2m6SX{jOQ&$NR*33P0"));
                }
                loadedScore = Convert.ToInt32(StringCipher.Decrypt(saveRead.ReadLine(), "'!Mqi3C0tZENCuw~9aF64/I[j50?e$"));
                loadedLevel = Convert.ToInt32(StringCipher.Decrypt(saveRead.ReadLine(), "w1i7BI&B3J.H2m6SX{jOQ&$NR*33P0"));
                loadedTotalClearedLines = Convert.ToInt32(StringCipher.Decrypt(saveRead.ReadLine(), "w1i7BI&B3J.H2m6SX{jOQ&$NR*33P0"));

                saveRead.Close();
                theFileRead.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Load has failed.");
                ErrorHandler.recordError(2, 201, "Loading the game has failed.", e.ToString());
            }

            //Erase the file so the player can't retry to get a high score from a score that is not 0 far in the game.
            try
            {
                theFileWrite = new FileStream(path,
                                              FileMode.Create,
                                              FileAccess.Write);
                saveWrite = new StreamWriter(theFileWrite);
                saveWrite.Close();
                theFileWrite.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erasing the save game file has failed");
                ErrorHandler.recordError(2, 201, "Erasing the save game file has failed", e.ToString());
            }
            return so;
        }
    }
}


