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
        //This class is designed to handle the high score menu and how to record new high scores
        //This class also encrypts high scores so the file cannot be tampered with
        int redIntensity, blueIntensity, greenIntensity;
        Int32 dummy;
        String[] textHighScores1 = new string[10];
        String[] textHighScores2 = new string[10];
        String[] encryptedHighScores1 = new string[10];
        String[] encryptedHighScores2 = new string[10];
        String[] textNames1 = new string[10];
        String[] textNames2 = new string[10];
        String[] encryptedNames1 = new string[10];
        String[] encryptedNames2 = new string[10];
        Boolean boolWorkingFileIO = true;
        SpecialEffects specialEffects = new SpecialEffects();
        Texture2D scoreTitle, background;
        SpriteFont smallFont;
        XmlDocument scoresWrite, scoresRead;

        public ScoreClass (SpriteFont sf)
		{
            smallFont = sf;
		}

		//High Score Screen Update
		public void ScoreClassUpdate(GameTime gameTime)
		{
            redIntensity = specialEffects.getRed();
            blueIntensity = specialEffects.getBlue();
            greenIntensity = specialEffects.getGreen();
            specialEffects.colorChanger();
		}

        public void retrieveScores()
		{
			boolWorkingFileIO = true;

            try
            {
                scoresRead = new XmlDocument();
                scoresRead.Load("tetroHighScores.xml");

                for (int i = 0; i < 10; i++)
                {
                    encryptedHighScores1[i] = scoresRead.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Score"].Value;
                    encryptedNames1[i] = scoresRead.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Name"].Value;

                    if (encryptedHighScores1[i] != "")
                    {
                        textHighScores1[i] = StringCipher.Decrypt(encryptedHighScores1[i], "hello"); 
                    }
                    else
                    {
                        textHighScores1[i] = "0";
                    }

                    if (encryptedNames1[i] != "")
                    {
                        textNames1[i] = StringCipher.Decrypt(encryptedNames1[i], "yes");
                    }
                    else
                    {
                        textNames1[i] = "FDR";
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
                Console.WriteLine("No high score file was found");
                Console.WriteLine("Generating tetroHighScores.xml");
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
                    nameAttribute.Value = "";
                    userNode.Attributes.Append(nameAttribute);
                    rootNode.AppendChild(userNode);
                }
                create.Save("tetroHighScores.xml");
			}
		}

		//Records Scores to an XML file on hard drive
		public void recordScore(int sc, string n)
		{
			boolWorkingFileIO = true;

            retrieveScores();

			//If there are no file problems continue
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
                        encryptedNames2[i] = StringCipher.Encrypt(textNames2[i], "yes");
                        scoresWrite.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Score"].Value = encryptedHighScores2[i];
                        scoresWrite.SelectSingleNode("/TetroScores/Score" + (i + 1)).Attributes["Name"].Value = encryptedNames2[i];
                    }
                    scoresWrite.Save("tetroHighScores.xml");
                    dummy = 0;
                }
			}
			catch
            {
                Console.WriteLine("No high score file was found");
                Console.WriteLine("Generating tetroHighScores.xml");
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
                    nameAttribute.Value = "";
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
            spriteBatch.DrawString(smallFont, "Press space", new Vector2(10, 540), Color.White);
            spriteBatch.DrawString(smallFont, "to go back", new Vector2(10, 570), Color.White);

			for (int i = 0; i < 10; i++)
            {
                if (i < 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "     " + textNames1[i] + "    " + textHighScores1[i]),
                                                    new Vector2(150, (175 + (i * 40))), Color.White); 
                }
                else if(i == 9)
                {
                    spriteBatch.DrawString(font, ((i + 1).ToString() + "    " + textNames1[i] + "    " + textHighScores1[i]),
                                                    new Vector2(150-16, (175 + (i * 40))), Color.White);
                }
			}
		}
	}
}

