using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    //This is the main class file for Tetro
    //This runs the entire game
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Variables
        enum GameStates { CreditScreen, MainMenu, Playing, GameOver, HighScoreScreen, PauseGame, Debug, Settings, Tutroial };
        GameStates gameState = GameStates.CreditScreen;

        Texture2D 
            scoreBackground,
            gameBackground;
        SpriteFont 
            bigFont, 
            smallFont;
        Song 
            playBGM, 
            menuBGM;
        SoundEffect 
            menuSoundEffect, 
            blockGroundSoundEffect, 
            lineClearedSoundEffect;
        bool 
            escapeDidSomething = false,
            spaceDidSomething = false,
            muted;
        public static bool 
            consoleShown, 
            fullscreen;
        bool[,] store = new bool[10, 20];	                            //Block storing			
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
        IntPtr handle = GetConsoleWindow();
        Vector2[,] lines = new Vector2[10, 20]; 		                //Block placing board positions
        Texture2D[,] blocks = new Texture2D[10, 20];	                //Block store show
        SingleBlockHelper[] activeBlocks = new SingleBlockHelper[4];	//Blocks that the player can move
        XmlDocument settingsRecord;
        MainMenuClass mainMenuClass;
        ScoreClass scoreClass;
        ErrorHandler errorHandler = new ErrorHandler();
        SaveGameClass saveGameClass = new SaveGameClass();
        StringInputClass stringInputClass = new StringInputClass();
        PauseGameClass pauseGameClass;
        TutorialClass tutorialClass;
        SettingsClass settingsClass;
        BoardClass boardClass;
        BlockHelper blockHelper;
        CreditClass creditClass;
        GameOverClass gameOverClass;
        KeyboardState keyState;
        #endregion

        //Importing commands for show/hide console.
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 700;
            #region XML load settings
            try
            {
                XmlDocument settings = new XmlDocument();
                settings.Load("tetroSettings.xml");
                try //Nested try-catches to prevent total file rewrite to prevent settings reset.
                {
                    fullscreen = Convert.ToBoolean(settings.SelectSingleNode("/TetroSettings/Fullscreen").InnerText.ToString());
                    graphics.IsFullScreen = fullscreen;
                }
                catch
                {
                    Console.WriteLine("Fullscreen value is invalid; setting to false.");
                    settings.SelectSingleNode("/TetroSettings/Fullscreen").InnerText = "false";
                    graphics.IsFullScreen = false;
                }
                try
                {
                    consoleShown = Convert.ToBoolean(settings.SelectSingleNode("/TetroSettings/ConsoleShown").InnerText.ToString());
                    if (!consoleShown)
                        ShowWindow(handle, SW_HIDE);
                }
                catch
                {
                    Console.WriteLine("Show Console value is invalid; setting to true.");
                    settings.SelectSingleNode("/TetroSettings/ConsoleShown").InnerText = "true";
                }
                try
                {
                    muted = Convert.ToBoolean(settings.SelectSingleNode("/TetroSettings/Muted").InnerText.ToString());
                    if (muted)
                        MediaPlayer.IsMuted = muted;
                }
                catch
                {
                    Console.WriteLine("Muted value is invalid; setting to false.");
                    settings.SelectSingleNode("/TetroSettings/Muted").InnerText = "false";
                }
                settings.Save("tetroSettings.xml");
            }
            catch //Creates the file should it not exist.
            {
                Console.WriteLine("Could not load tetroSettings.xml; most likely the file doesn't exist.");
                Console.WriteLine("Generating tetroSettings.xml and setting all options to default.");
                XmlDocument settings = new XmlDocument();
                XmlNode rootNode = settings.CreateElement("TetroSettings");
                settings.AppendChild(rootNode);
                XmlNode userNode = settings.CreateElement("Fullscreen");
                userNode.InnerText = "false";
                rootNode.AppendChild(userNode);
                userNode = settings.CreateElement("ConsoleShown");
                userNode.InnerText = "true";
                rootNode.AppendChild(userNode);
                userNode = settings.CreateElement("Muted");
                userNode.InnerText = "false";
                rootNode.AppendChild(userNode);
                settings.Save("tetroSettings.xml");
                graphics.IsFullScreen = false;
            }
            #endregion
        }

        //This is the main class constructor
        protected override void Initialize()
        {
            errorHandler.recordError(1, 101, "Application Initialize", null);
            base.Initialize();
        }

        //This function loads all of Tetro's content like sprites and music files
        protected override void LoadContent()
        {
            try
            {
                #region LoadContent
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);

                //Fonts
                smallFont = Content.Load<SpriteFont>(@"Fonts\smallFont");
                bigFont = Content.Load<SpriteFont>(@"Fonts\bigFont");

                //Sounds
                menuSoundEffect = Content.Load<SoundEffect>(@"Audio\TetroEffect3");
                blockGroundSoundEffect = Content.Load<SoundEffect>(@"Audio\TetroEffect4");
                lineClearedSoundEffect = Content.Load<SoundEffect>(@"Audio\TetroEffect6");
                playBGM = Content.Load<Song>(@"Audio\TetroSong4");
                menuBGM = Content.Load<Song>(@"Audio\TetroSong3");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(menuBGM);

                //BoardClass
                boardClass = new BoardClass(blocks, store, lines);
                store = boardClass.resetStore();
                lines = boardClass.resetLinesGrid();
                blocks = boardClass.loadBlocksTexture(Content);

                scoreBackground = Content.Load<Texture2D>(@"Textures\TetroBorder");
                gameBackground = Content.Load<Texture2D>(@"Textures\Tetro Game Background");

                //Load the player controlled blocks
                blockHelper = new BlockHelper(activeBlocks, lines, store, blockGroundSoundEffect, lineClearedSoundEffect);
                blockHelper.setColors();
                activeBlocks = blockHelper.loadPlayerBlocks(Content);

                //MainMenuClass
                mainMenuClass = new MainMenuClass(smallFont, bigFont);
                mainMenuClass.LoadContent(Content);

                //CreditClass
                creditClass = new CreditClass(smallFont, bigFont);
                creditClass.LoadContent(Content);

                //ControlsClass
                settingsClass = new SettingsClass(smallFont, bigFont);
                settingsClass.Load(Content);

                //ScoreClass
                scoreClass = new ScoreClass(smallFont);
                scoreClass.Load(Content);

                //GameOver Class
                gameOverClass = new GameOverClass(bigFont, smallFont);
                gameOverClass.LoadContent(Content);

                //Tutorial Class
                tutorialClass = new TutorialClass(smallFont, menuBGM);
                tutorialClass.LoadContent(Content);

                //PauseGame Class
                pauseGameClass = new PauseGameClass(bigFont, smallFont);
                #endregion
            }
            catch (Exception d)
            {
                errorHandler.recordError(3, 102, "Load Content Failed", d.ToString());
            }
        }

        //This is Tetro's main update function
        //The game logic is in here
        protected override void Update(GameTime gameTime)
        {
            try
            {
                #region Update
                keyState = Keyboard.GetState();

                //Single Key press escape,  if escape is still down it still already did something
                escapeDidSomething = (keyState.IsKeyDown(Keys.Escape) && escapeDidSomething);

                //Single Key press Space,  if space is still down it still already did something
                spaceDidSomething = (keyState.IsKeyDown(Keys.Space) && spaceDidSomething);

                //For Mobile devices, this logic will close the Game when the Back button is pressed
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    Exit();

                if (gameState == GameStates.CreditScreen)
                {
                    #region Credit Screen
                    //Update BGM and GameState
                    if (creditClass.ChangeScreen(gameTime))
                        gameState = GameStates.MainMenu;
                    #endregion
                }

                //Switch case is for what option the player chooses
                if (gameState == GameStates.MainMenu)
                {
                    #region Main Menu Screen
                    mainMenuClass.Update(gameTime);
                    if (keyState.IsKeyDown(Keys.Space) && !spaceDidSomething)
                    {
                        menuSoundEffect.Play();
                        switch (mainMenuClass.detectGameState())
                        {
                            case 1:
                                #region Play
                                //Run constructors to play a new game
                                store = boardClass.resetStore();
                                for (int i = 0; i < activeBlocks.Length; i++)
                                {
                                    activeBlocks[i].setStore(store);
                                    activeBlocks[i].resetPlayerBlockPos();
                                }
                                blockHelper.setLevel(1);
                                blockHelper.setScore(0);
                                blockHelper.setTotalClearedLine(0);
                                gameState = GameStates.Playing;
                                MediaPlayer.Stop();
                                MediaPlayer.Play(playBGM);
                                break;
                                #endregion
                            case 2:
                                #region Tutorial
                                //Run constructors for tutorial
                                store = boardClass.resetStore();
                                for (int i = 0; i < activeBlocks.Length; i++)
                                {
                                    activeBlocks[i].setPattern(2);
                                    activeBlocks[i].resetBlocks();
                                    activeBlocks[i].setStore(store);
                                    activeBlocks[i].resetPlayerBlockPos();
                                }
                                store[0, 19] = true;
                                store[1, 19] = true;
                                store[2, 19] = true;
                                store[3, 19] = true;
                                store[4, 19] = false;
                                store[5, 19] = true;
                                store[6, 19] = true;
                                store[7, 19] = true;
                                store[8, 19] = true;
                                store[9, 19] = true;
                                blockHelper.setLevel(1);
                                blockHelper.setScore(0);
                                tutorialClass.setMessageNum(1);
                                tutorialClass.setGotoMenu(false);
                                gameState = GameStates.Tutroial;
                                MediaPlayer.Stop();
                                MediaPlayer.Play(playBGM);
                                break;
                                #endregion
                            case 3:
                                #region Settings
                                gameState = GameStates.Settings;
                                break;
                                #endregion
                            case 4:
                                #region Exit
                                Exit();
                                break;
                                #endregion
                            case 5:
                                #region High Scores
                                //Retrieve score twice to generate file if it does not exist
                                scoreClass.retrieveScores();
                                scoreClass.retrieveScores();
                                gameState = GameStates.HighScoreScreen;
                                break;
                                #endregion
                            case 6:
                                #region Load Game
                                store = saveGameClass.loadGameData();
                                for (int i = 0; i < activeBlocks.Length; i++)
                                {
                                    activeBlocks[i].setStore(store);
                                    activeBlocks[i].resetPlayerBlockPos();
                                }
                                blockHelper.setLevel(saveGameClass.getLoadedLevel());
                                blockHelper.setScore(saveGameClass.getLoadedScore());
                                blockHelper.setTotalClearedLine(saveGameClass.getLoadedTotalClearedLines());
                                MediaPlayer.Play(playBGM);
                                gameState = GameStates.Playing;
                                break;
                                #endregion
                            default:
                                break;
                        }
                        spaceDidSomething = true;
                    }
                    #endregion
                }

                if ((gameState == GameStates.Playing) ||
                    (gameState == GameStates.Debug) ||
                    (gameState == GameStates.Tutroial && !tutorialClass.getIsTutorialPaused()))
                {
                    #region Update Game Algorithm
                    blockHelper.setActiveBlocks(activeBlocks);
                    blockHelper.BlockHelperUpdate(gameTime);
                    activeBlocks = blockHelper.getActiveBlocks();
                    #endregion

                    #region Gameover Detection
                    //If a block hits the top of the board it is game over
                    for (int i = 0; i < 10; i++)
                    {
                        if (store[i, 0] == true)
                        {
                            gameState = GameStates.GameOver;
                            MediaPlayer.Stop();
                            MediaPlayer.Play(menuBGM);
                            MediaPlayer.IsRepeating = true;
                        }
                    }
                    #endregion
                }

                if (gameState == GameStates.Playing)
                {
                    #region Playing Game Screen
                    //Go to the Debug screen to see field values
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                        gameState = GameStates.Debug;

                    //Go to the pause game menu
                    if (keyState.IsKeyDown(Keys.Escape) && !escapeDidSomething)
                    {
                        menuSoundEffect.Play();
                        gameState = GameStates.PauseGame;
                        escapeDidSomething = true;
                    }
                    #endregion
                }

                if (gameState == GameStates.Tutroial)
                {
                    #region Tutorial Screen
                    tutorialClass.Update(gameTime);
                    if (tutorialClass.getGotoMenu() && !spaceDidSomething)
                    {
                        gameState = GameStates.MainMenu;
                        spaceDidSomething = true;
                    }
                    #endregion
                }

                if (gameState == GameStates.PauseGame)
                {
                    #region Pause Game Menu Screen
                    pauseGameClass.update(gameTime);
                    //Un_pause the game
                    if (keyState.IsKeyDown(Keys.Escape) && !escapeDidSomething)
                    {
                        menuSoundEffect.Play();
                        gameState = GameStates.Playing;
                        escapeDidSomething = true;
                    }

                    //Pause Menu option execution
                    if (keyState.IsKeyDown(Keys.Space) && !spaceDidSomething)
                    {
                        menuSoundEffect.Play();
                        switch (pauseGameClass.getMenuOption())
                        {
                            case 1:
                                //Resume
                                gameState = GameStates.Playing;
                                break;

                            case 2:
                                //Save
                                saveGameClass.recordGameData(store,
                                    blockHelper.getScore(),
                                    blockHelper.getLevel(),
                                    blockHelper.getTotalClearedLines());
                                MediaPlayer.Play(menuBGM);
                                gameState = GameStates.MainMenu;
                                break;

                            case 3:
                                //Exit
                                MediaPlayer.Play(menuBGM);
                                gameState = GameStates.MainMenu;
                                break;

                            default:

                                break;
                        }
                        spaceDidSomething = true;
                    }
                    #endregion
                }

                if (gameState == GameStates.GameOver)
                {
                    #region Game Over Screen
                    gameOverClass.Update(gameTime);
                    //Main Menu
                    if (keyState.IsKeyDown(Keys.Space) && !spaceDidSomething && gameOverClass.getCanSubmitName())
                    {
                        menuSoundEffect.Play();
                        try
                        {
                            scoreClass.recordScore(blockHelper.getScore(), gameOverClass.getName());
                        }
                        catch (Exception e)
                        {
                            errorHandler.recordError(2, 104, "Saving has failed.", e.ToString());
                        }
                        gameState = GameStates.MainMenu;
                        spaceDidSomething = true;
                    }
                    #endregion
                }

                if (gameState == GameStates.HighScoreScreen)
                {
                    #region High Score Screen
                    scoreClass.ScoreClassUpdate(gameTime);

                    //Main Menu
                    if (keyState.IsKeyDown(Keys.Space) && !spaceDidSomething)
                    {
                        menuSoundEffect.Play();
                        gameState = GameStates.MainMenu;
                        spaceDidSomething = true;
                    }
                    #endregion
                }

                if (gameState == GameStates.Settings)
                {
                    #region Settings Screen
                    settingsClass.Update(gameTime);
                    if (keyState.IsKeyDown(Keys.Space) && !spaceDidSomething)
                    {
                        menuSoundEffect.Play();
                        switch (settingsClass.changeSetting())
                        {
                            case 1:
                                graphics.ToggleFullScreen();
                                fullscreen = !fullscreen;
                                break;

                            case 2:
                                if (consoleShown)
                                {
                                    //Hide the window
                                    ShowWindow(handle, SW_HIDE); // 0 = SW_HIDE                                        
                                }
                                if (!consoleShown)
                                {
                                    //Show window again
                                    ShowWindow(handle, SW_SHOW); //5 = SW_SHOWNORMA
                                }
                                consoleShown = !consoleShown;
                                break;

                            case 3:
                                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
                                muted = !muted;
                                break;

                            case 4:
                                settingsRecord = new XmlDocument();
                                settingsRecord.Load("tetroSettings.xml");
                                settingsRecord.SelectSingleNode("/TetroSettings/Fullscreen").InnerText = fullscreen.ToString();
                                settingsRecord.SelectSingleNode("/TetroSettings/ConsoleShown").InnerText = consoleShown.ToString();
                                settingsRecord.SelectSingleNode("/TetroSettings/Muted").InnerText = muted.ToString();
                                settingsRecord.Save("tetroSettings.xml");
                                gameState = GameStates.MainMenu;
                                break;

                            default:

                                break;
                        }
                        spaceDidSomething = true;
                    }	
                    base.Update(gameTime);
                    #endregion
                }
                #endregion
            }
            catch (Exception d)
            {
                errorHandler.recordError(3, 103, "Updating has failed", d.ToString());
            }
        }
        
        //This class draws to the screen so the player can see content like blocks
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                #region Draw
                graphics.GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin();

                if (gameState == GameStates.CreditScreen)
                    creditClass.Draw(spriteBatch);

                if (gameState == GameStates.MainMenu)
                    mainMenuClass.Draw(spriteBatch);

                if ((gameState == GameStates.Playing) ||
                    (gameState == GameStates.PauseGame))
                {
                    spriteBatch.Draw(gameBackground, Vector2.Zero, Color.Blue);
                    spriteBatch.Draw(scoreBackground, new Vector2(316, 0), Color.White);
                    spriteBatch.Draw(scoreBackground, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(bigFont, "Score:" + blockHelper.getScore().ToString(), new Vector2(350, 300), Color.White);
                    spriteBatch.DrawString(bigFont, "Lines: " + blockHelper.getTotalClearedLines().ToString(), new Vector2(350, 350), Color.White);
                    spriteBatch.DrawString(bigFont, "Level: " + blockHelper.getLevel().ToString(), new Vector2(350, 400), Color.White);
                    spriteBatch.DrawString(bigFont, "Next Pattern", new Vector2(340, 10), Color.White);
                }

                if ((gameState == GameStates.Playing) ||
                    (gameState == GameStates.Debug) ||
                    (gameState == GameStates.Tutroial))
                {
                    if (gameState == GameStates.Tutroial)
                    {
                        spriteBatch.Draw(gameBackground, Vector2.Zero, Color.Blue);
                        spriteBatch.DrawString(bigFont, "Next Pattern", new Vector2(340, 10), Color.White);
                    }
                    else 
                    {
                        spriteBatch.DrawString(smallFont, "Press escape to pause", new Vector2(400,570), Color.White);
                    }
                    spriteBatch.Draw(scoreBackground, new Vector2(316, 0), Color.White);
                    spriteBatch.Draw(scoreBackground, Vector2.Zero, Color.White);

                    //Draws the player controlled blocks
                    for (int i = 0; i < activeBlocks.Length; i++)
                        activeBlocks[i].Draw(spriteBatch);

                    blockHelper.Draw(spriteBatch, blocks);
                }

                if (gameState == GameStates.Debug)
                {
                    #region Debug Screen
                    for (int i = 0; i < activeBlocks.Length; i++)
                        spriteBatch.DrawString(smallFont, "stopFlags: " + activeBlocks[i].getBlockCollideBottomFlag().ToString(), new Vector2(350, 300 + (i * 25)), Color.White);
                    spriteBatch.DrawString(smallFont, "STORE: " + store[9, 19].ToString(), new Vector2(350, 400), Color.White);
                    spriteBatch.DrawString(smallFont, "Can go down: " + activeBlocks[0].getCanGoDownFlag().ToString(), new Vector2(350, 425), Color.White);
                    spriteBatch.DrawString(smallFont, "Next Pattern: " + activeBlocks[0].getNextPattern().ToString(), new Vector2(350, 450), Color.White);
                    spriteBatch.DrawString(smallFont, "Pattern 0: " + activeBlocks[0].getPattern().ToString(), new Vector2(350, 475), Color.White);
                    spriteBatch.DrawString(smallFont, "Pattern 1: " + activeBlocks[1].getPattern().ToString(), new Vector2(350, 500), Color.White);
                    spriteBatch.DrawString(smallFont, "Pattern 2: " + activeBlocks[2].getPattern().ToString(), new Vector2(350, 525), Color.White);
                    spriteBatch.DrawString(smallFont, "Pattern 3: " + activeBlocks[3].getPattern().ToString(), new Vector2(350, 550), Color.White);
                    spriteBatch.DrawString(smallFont, "Block Speed: " + activeBlocks[0].getMinTimer().ToString(), new Vector2(350, 275), Color.White);
                    spriteBatch.DrawString(smallFont, "Score: " + blockHelper.getScore().ToString(), new Vector2(350, 250), Color.White);
                    spriteBatch.DrawString(smallFont, "Lines: " + blockHelper.getTotalClearedLines().ToString(), new Vector2(350, 225), Color.White);
                    spriteBatch.DrawString(smallFont, "Level: " + blockHelper.getLevel().ToString(), new Vector2(350, 200), Color.White);
                    #endregion
                }

                if (gameState == GameStates.PauseGame)
                {
                    spriteBatch.DrawString(bigFont, "PAUSE", new Vector2(60, 50), Color.White);
                    spriteBatch.DrawString(smallFont, "Press space to select", new Vector2(400, 540), Color.White);
                    spriteBatch.DrawString(smallFont, "Press arrows to navigate", new Vector2(400, 570), Color.White);
                    pauseGameClass.draw(spriteBatch);
                }

                if (gameState == GameStates.GameOver)
                {
                    gameOverClass.Draw(spriteBatch);
                    spriteBatch.DrawString(bigFont, "Your score: " + blockHelper.getScore().ToString(), new Vector2(125, 150), Color.White);
                }

                if (gameState == GameStates.Settings)
                    settingsClass.Draw(spriteBatch);

                if (gameState == GameStates.HighScoreScreen)
                    scoreClass.Draw(spriteBatch, bigFont);

                if (gameState == GameStates.Tutroial)
                    tutorialClass.Draw(spriteBatch);

                spriteBatch.End();

                base.Draw(gameTime);
                #endregion
            }
            catch (Exception d)
            {
                errorHandler.recordError(3, 104, "Drawing has failed", d.ToString());
            }
        }
    }
}
