using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D paddleRed;
        Texture2D paddleBlue;
        Texture2D ball;

        Vector2 player1Location = new Vector2(50, 200);
        Vector2 player2Location = new Vector2(735, 200);
        Vector2 ballLocation = new Vector2(350, 225);

        Vector2 ballVelocity = new Vector2(2, 0);
        Vector2 ballAccelleration = new Vector2(0.35f, 0);

        KeyboardState input = Keyboard.GetState();


        float[] yAccelarationArrayPos = new float[] { 0.5f, 1.0f, 1.5f, 2.0f };
        float[] yAccelarationArrayNeg = new float[]  { 0.5f, -1.0f, -1.5f, -2.0f };
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            paddleRed = Content.Load<Texture2D>("rodeSpeler");
            paddleBlue = Content.Load<Texture2D>("blauweSpeler");
            ball = Content.Load<Texture2D>("bal");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected bool ballPaddleCollision()
        {
            if ((ballLocation.X >= 720 && ballLocation.X <= 730) && (ballLocation.Y <= (player2Location.Y + 100) && ballLocation.Y >= player2Location.Y))
            {
                return true;
            }
            else if ((ballLocation.X <= 60 && ballLocation.X >= 50) && (ballLocation.Y <= (player1Location.Y + 100) && ballLocation.Y >= player1Location.Y))
            {
                return true;
            }

            return false;
        }

        protected void gameOver()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //check input from keyboard
            input = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || input.IsKeyDown(Keys.Escape))
                Exit();

            //Player1 movement
            if (input.IsKeyDown(Keys.W) && player1Location.Y > 0)
            {
                player1Location.Y -= 6;
            }
            else if (input.IsKeyDown(Keys.S) && player1Location.Y < (GraphicsDevice.Viewport.Bounds.Height - 100))
            {
                player1Location.Y += 6;
            }

            //Player2 movement
            if (input.IsKeyDown(Keys.Up) && player2Location.Y > 0)
            {
                player2Location.Y -= 6;
            }
            else if (input.IsKeyDown(Keys.Down) && player2Location.Y < (GraphicsDevice.Viewport.Bounds.Height - 100))
            {
                player2Location.Y += 6;
            }

            //ball movement

            //check game over
            if (ballLocation.X <= 0 || ballLocation.X >= GraphicsDevice.Viewport.Bounds.Width)
            {
                ballLocation.X = GraphicsDevice.Viewport.Bounds.Width / 2;
                ballLocation.Y = GraphicsDevice.Viewport.Bounds.Height / 2;

                ballVelocity = new Vector2(2, 0);
            }

            //check Y wall collisions
            if (ballLocation.Y <= 0 || ballLocation.Y >= GraphicsDevice.Viewport.Bounds.Height)
            {
                ballVelocity.Y *= -1.0f;
            }

            //check playerColliisions and ball x direction
            if (ballPaddleCollision())
            {
                ballVelocity.X *= -1.0f;

                System.Random rand = new System.Random();
                if (rand.Next(0, 2) == 1)
                {
                    ballVelocity.Y += yAccelarationArrayNeg[rand.Next(0, 4)];
                }
                else
                {
                    ballVelocity.Y += yAccelarationArrayPos[rand.Next(0, 4)];
                }
            }


            //update ballvelocity
            if(ballPaddleCollision() && ballVelocity.X < 3.3f)
            {
                if(ballVelocity.X > 0)
                {
                    ballVelocity += ballAccelleration;
                }
                else
                {
                    ballVelocity -= ballAccelleration;
                }
            }

            //update location
            ballLocation += ballVelocity;
            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);
            spriteBatch.Begin();
            spriteBatch.Draw(paddleRed, player1Location, Color.White);
            spriteBatch.Draw(paddleBlue, player2Location, Color.White);
            spriteBatch.Draw(ball, ballLocation, Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
