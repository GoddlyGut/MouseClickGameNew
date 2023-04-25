using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MouseClickGameNew
{

    enum CurrentLevel
    {
        Level_0 = 40,
        Level_1 = 60,
        Level_2 = 50,
        Level_3 = 80,
        Level_4 = 90,
    }

    public class Game1 : Game
    {
        //GRAPHICS RELATED
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;

        //GAME BOARD
        List<int> posToChoose = new List<int>();
        Dictionary<int, int> gridItemsPositions = new Dictionary<int, int>();

        //GAME STATES
        bool isGameRunning = true;
        bool isTimeUp = false;

        //GRID HANDLING
        int numberOfGridSpaces = 20;
        bool gridCreated = false;
        int gridWidth = 20;
        int gridHeight = 20;
        int cellSize = 30;
        int gridSpacing = 0;

        //VALUES
        int numberOfItemsInLevel = 0;
        int numberOfItemsInLevelMax; //LEAVE NULL
        CurrentLevel currentLevelSelected = CurrentLevel.Level_0;
        int score = 0;
        float clock = 60 * 30;

        //STATES
        MouseState mouse, oldMouse;

        //MOUSE RELATED
        Point cursorPoint;


        //---------------------------------------------------\\
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 600; //
            _graphics.PreferredBackBufferHeight = 600; //
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            numberOfItemsInLevelMax = (int)currentLevelSelected;

            CreatePositionsToRandomlyPick();



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouse = Mouse.GetState();
            cursorPoint = new Point(mouse.X, mouse.Y);

            if (!gridCreated)
            {
                CreateItemsOnGrid();
            }

            // TODO: Add your update logic here

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                
                foreach (KeyValuePair<int, int> pair in gridItemsPositions)
                {


                    if (MathF.Abs(((pair.Key % gridWidth) * (cellSize + gridSpacing)) - mouse.X) < 20 && MathF.Abs(((pair.Key / gridWidth) * (cellSize + gridSpacing)) - mouse.Y) < 20)
                    {
                        gridItemsPositions.Remove(pair.Key);
                        score++;
                        return;
                    }
                }
            }

            CheckIfAllItemsClicked();

            oldMouse = mouse;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            DrawGridItemsOnScreen();
            DrawTextItemsOnScreen();
            UpdateClock();

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void UpdateClock()
        {
            if (clock > 0)
            {
                clock -= 1;
            }
            else
            {
                ResetEntireGame(CurrentLevel.Level_0, true);
            }
            
        }

        private void CreatePositionsToRandomlyPick()
        {
            for (int i = 0; i < 400; i++)
            {
                posToChoose.Add(i);
            }
        }

        private void CreateItemsOnGrid()
        {
            while (numberOfItemsInLevel < numberOfItemsInLevelMax)
            {
                Random random = new Random();

                bool posFound = false;

                while (!posFound)
                {
                    int XpositionToCheck = random.Next(posToChoose.Count);
                    int YpositionToCheck = random.Next(posToChoose.Count);
                    if (!gridItemsPositions.ContainsKey(XpositionToCheck) && !gridItemsPositions.ContainsValue(YpositionToCheck))
                    {
                        posFound = true;
                        Debug.WriteLine(posToChoose[XpositionToCheck] + ", " + posToChoose[YpositionToCheck]);
                        gridItemsPositions.Add(posToChoose[XpositionToCheck], posToChoose[YpositionToCheck]);
                        numberOfItemsInLevel++;
                    }

                }
            }
        }

        private void ResetEntireGame(CurrentLevel levelToEnable, bool resetScore)
        {
            gridItemsPositions.Clear();
            currentLevelSelected = levelToEnable;
            clock = 30 * 60;
            if (resetScore)
            {
                score = 0;
            }
            numberOfItemsInLevel= 0;
            numberOfItemsInLevelMax = (int)levelToEnable;
        }

        private void CheckIfAllItemsClicked()
        {
            if (gridItemsPositions.Count == 0)
            {
                switch(currentLevelSelected)
                {
                    case CurrentLevel.Level_0:
                        ResetEntireGame(CurrentLevel.Level_1, false);
                        break;
                    case CurrentLevel.Level_1:
                        ResetEntireGame(CurrentLevel.Level_2, false);
                        break;
                    case CurrentLevel.Level_2:
                        ResetEntireGame(CurrentLevel.Level_3, false);
                        break;
                    case CurrentLevel.Level_3:
                        ResetEntireGame(CurrentLevel.Level_4, false);
                        break;
                    default:
                        ResetEntireGame(CurrentLevel.Level_0, true);
                        break;
                }
            }
        }

        private void DrawTextItemsOnScreen()
        {
            _spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(font, "Level: " + currentLevelSelected, new Vector2(250, 0), Color.White);
            _spriteBatch.DrawString(font, "Time Remaining: " + MathF.Ceiling((clock / 60)).ToString(), new Vector2(400, 0), Color.White);
        }

        private void DrawGridItemsOnScreen()
        {

            foreach (KeyValuePair<int, int> pair in gridItemsPositions)
            {
                Texture2D texture = Content.Load<Texture2D>("PowerStar");

                int x = (pair.Key % gridWidth) * (cellSize + gridSpacing);
                int y = (pair.Key / gridWidth) * (cellSize + gridSpacing);
                Rectangle rect = new Rectangle(x, y, cellSize, cellSize);


                _spriteBatch.Draw(texture, rect, Color.White);
            }


            
        }
    }
}