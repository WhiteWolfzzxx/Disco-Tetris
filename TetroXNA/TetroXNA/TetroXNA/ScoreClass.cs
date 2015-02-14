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
        int redIntensity, blueIntensity, greenIntensity;
        Int32 dummy; //Used for the write once.
        String[] textHighScores1 = new string[10];
        String[] textHighScores2 = new string[10];
        String[] encryptedHighScores1 = new string[10];
        String[] encryptedHighScores2 = new string[10];
        String[] textNames1 = new string[10];
        String[] textNames2 = new string[10];
        Boolean boolWorkingFileIO = true;
        MenuProperties menuProperties = new MenuProperties();
        Texture2D scoreTitle, background;
        XmlDocument scoresWrite, scoresRead;

        public ScoreClass ()
		{

		}

		//High Score Screen Update
		public void ScoreClassUpdate(GameTime gameTime)
		{
            redIntensity = menuProperties.getRed();
            blueIntensity = menuProperties.getBlue();
            greenIntensity = menuProperties.getGreen();
            menuProperties.colorChanger();
		}

		public void retriveScores()
		{
			boolWorkingFileIO = true;

            try
            {
                scoresRead = new XmlDocument();
                scoresRead.Load("tetroHighScores.xml");

                for (int i = 0; i < 10; i++)
                {
                    encryptedHighScores1[i] = scoresRead.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Score"].Value;
                    textNames1[i] = scoresRead.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Name"].Value;

                    if (encryptedHighScores1[i] != "")
                    {
                        textHighScores1[i] = StringCipher.Decrypt(encryptedHighScores1[i], "hello"); 
                    }
                    else
                    {
                        textHighScores1[i] = "0";
                    }
                    
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
                    nameAttribute.Value = "AAA";
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

			//if there are no file problems continue
			if (boolWorkingFileIO) 
            {
				for (int i = 0, j = 0; i < 10; i++, j++) 
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
				}
			}

			//Write the new scores to the file
			try
            {
                dummy = 1;
                
                while (dummy == 1)
                {
                    scoresWrite = new XmlDocument();

                    scoresWrite.Load("tetroHighScores.xml");
                    
                    for (int i = 0; i < 10; i++)
                    {
                        encryptedHighScores2[i] = StringCipher.Encrypt(textHighScores2[i], "hello");
                        scoresWrite.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Score"].Value = encryptedHighScores2[i];
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

        public void Load(ContentManager Content)
        {
            scoreTitle = Content.Load<Texture2D>(@"Textures\ScoresTitle");
            background = Content.Load<Texture2D>(@"Textures\Tetro Scores Background");
        }

		public void Draw(SpriteBatch spriteBatch, SpriteFont font)
		{
            spriteBatch.Draw(background, Vector2.Zero, Color.Blue);

            spriteBatch.Draw(scoreTitle, new Vector2(5, 5), new Color(redIntensity, greenIntensity, blueIntensity));

			for (int i = 0; i < 10; i++)
            {
                if (i < 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "     " + textNames1[i] + "     " + textHighScores1[i]),
                                                    new Vector2(150, (175 + (i * 40))), Color.White); 
                }
                else if(i == 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "    " + textNames1[i] + "     " + textHighScores1[i]),
                                                    new Vector2(150-16, (175 + (i * 40))), Color.White);
                }
			}
		}
	}
}

