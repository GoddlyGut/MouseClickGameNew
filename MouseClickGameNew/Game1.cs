using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MouseClickGameNew
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        //GAME BOARD
        List<int> posToChoose = new List<int>();
        List<int> XposUsed = new List<int>();
        List<int> YposUsed = new List<int>();
        Dictionary<int, int> gridItemsPositions = new Dictionary<int, int>();


        //GAME STATES
        bool isGameRunning = true;
        int currentLevel = 0;
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
        int numberOfItemsInLevelMax = 60;

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

            CreatePositionsToRandomlyPick();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!gridCreated)
            {
                CreateItemsOnGrid();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            DrawGridItemsOnScreen();

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
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