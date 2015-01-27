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
        String[] textNames1 = new string[10];
        String[] textNames2 = new string[10];
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

            try
            {
                scoresRead = new XmlDocument();
                scoresRead.Load("tetroHighScores.xml");

                for (int i = 0; i < 10; i++)
                {
                    textHighScores1[i] = scoresRead.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Score"].Value;
                    textNames1[i] = scoresRead.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Name"].Value;

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
                    XmlAttribute nameAttribute = create.CreateAttribute("Name");
                    nameAttribute.Value = "John Doe";
                    userNode.Attributes.Append(nameAttribute);
                    rootNode.AppendChild(userNode);
                }
                create.Save("tetroHighScores.xml");
			}
		}

		//Records Scores in a flat file on hard drive
		public void recordScore(int sc, string n)
		{
			boolWorkingFileIO = true;

			retriveScores ();

			//if there ar no file problems continue
			if (boolWorkingFileIO) 
            {
				int j = 0;

				for (int i = 0; i < 10; i++) 
                {
					if (sc > Convert.ToInt32(textHighScores1[i]) && i == j) 
                    {
						textHighScores2 [i] = sc.ToString();
                        textNames2[i] = n.ToString();
						i++;
						if (i < 10) 
                        {
							textHighScores2 [i] = textHighScores1 [j];
                            textNames2[i] = textNames1[j];
						}
					}
                    else 
                    {
						textHighScores2 [i] = textHighScores1 [j];
                        textNames2[i] = textNames1[j];
					}
					j++;
				}
			}

			//Write the new scores to the file
			try{
                dummy = 1;
                
                while (dummy == 1)
                {
                    scoresWrite = new XmlDocument();

                    scoresWrite.Load("tetroHighScores.xml");
                    
                    for (int i = 0; i < 10; i++)
                    {                        
                        scoresWrite.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Score"].Value = textHighScores2[i];
                        scoresWrite.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Name"].Value = textNames2[i];
                    }
                    scoresWrite.Save("tetroHighScores.xml");
                    dummy = 0;
                }
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
                    XmlAttribute nameAttribute = create.CreateAttribute("Name");
                    nameAttribute.Value = "John Doe";
                    userNode.Attributes.Append(nameAttribute);
                    rootNode.AppendChild(userNode);
                }
                create.Save("tetroHighScores.xml");
			}
		}

		public void Draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			for (int i = 0; i < 10; i++)
            {
                if (i < 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "     " + textHighScores1[i] + "     " + textNames1[i]),
                                                    new Vector2(200, (200 + (i * 25))), Color.White); 
                }
                else if(i == 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "     " + textHighScores1[i] + "     " + textNames1[i]),
                                                    new Vector2(188, (200 + (i * 25))), Color.White);
                }
			}
		}
	}
}

