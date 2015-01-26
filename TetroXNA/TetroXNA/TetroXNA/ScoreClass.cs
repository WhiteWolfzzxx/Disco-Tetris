using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TetroXNA
{
    public class ScoreClass
    {
        Int32 dummy; //Used for the write once.
        String[] textHighScores1 = new string[10];
        String[] textHighScores2 = new string[10];
        //FileStream theFileRead;
        //FileStream theFileWrite;
        //StreamReader theScoreRead;
        //StreamWriter theScoreWrite;
        Boolean boolWorkingFileIO = true;
        XmlDocument scoresWrite;
        XmlDocument scoresRead;

        public ScoreClass ()
		{

		}

		//High Score Screen Update
		public void ScoreClassUpdate(GameTime gameTime)
		{
			retriveScores ();
		}

		private void retriveScores()
		{
			boolWorkingFileIO = true;

			/*try
			{
				theFileRead = new FileStream("tetroHighScores.txt", 
				                             FileMode.OpenOrCreate,
				                             FileAccess.Read);
				theScoreRead = new StreamReader(theFileRead);

				for (int i = 0; i < 10; i++)
				{
					textHighScores1[i] = theScoreRead.ReadLine();

					if (textHighScores1[i] == null)
					{
						textHighScores1[i] = "0";
					}
				}
				theScoreRead.Close();
				theFileRead.Close();
			}*/
            try
            {
                scoresRead = new XmlDocument();
                scoresRead.Load("tetroHighScores.xml");

                for (int i = 0; i < 10; i++)
                {
                    textHighScores1[i] = scoresRead.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Score"].Value;

                    if (textHighScores1[i] == "")
                    {
                        textHighScores1[i] = "0";
                    }
                }
                scoresRead.Save("tetroHighScores.xml");
            }
			catch 
            {
				boolWorkingFileIO = false;
                XmlDocument create = new XmlDocument();
                XmlNode rootNode = create.CreateElement("TetroScores");
                create.AppendChild(rootNode);

                for (int i = 1; i < 11; i++)
                {
                    XmlNode userNode = create.CreateElement("Score" + i);
                    XmlAttribute attribute = create.CreateAttribute("Score");
                    attribute.Value = "";
                    userNode.Attributes.Append(attribute);
                    rootNode.AppendChild(userNode);
                }
                create.Save("tetroHighScores.xml");
			}
		}

		//Records Scores in a flat file on hard drive
		public void recordScore(int sc)
		{
			boolWorkingFileIO = true;

			retriveScores ();

			//if there ar no file problems continue
			if (boolWorkingFileIO) 
            {
				int j = 0;

				for (int i = 0; i < 10; i++) 
                {
					if (sc > Convert.ToInt32 (textHighScores1 [i]) && i == j) 
                    {
						textHighScores2 [i] = sc.ToString();
						i++;
						if (i < 10) 
                        {
							textHighScores2 [i] = textHighScores1 [j];
						}
					}
                    else 
                    {
						textHighScores2 [i] = textHighScores1 [j];
					}
					j++;
				}
			}

			//Write the new scores to the file
			try{
                dummy = 1;
                /*while (dummy == 1)
                {
                    theFileWrite = new FileStream("tetroHighScores.txt",
                                                      FileMode.Create,
                                                      FileAccess.Write);
                    theScoreWrite = new StreamWriter(theFileWrite);

                    for (int i = 0; i < 10; i++)
                    {
                        theScoreWrite.WriteLine(textHighScores2[i]);
                    }

                    theScoreWrite.Close();
                    theFileWrite.Close();

                    dummy = 0;
                }*/
			}
			catch
            {
				boolWorkingFileIO = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			for (int i = 0; i < 10; i++)
            {
                if (i < 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "     " + textHighScores1[i]),
                                                    new Vector2(200, (200 + (i * 25))), Color.White); 
                }
                else if(i == 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "     " + textHighScores1[i]),
                                                    new Vector2(188, (200 + (i * 25))), Color.White);
                }
			}
		}
	}
}

