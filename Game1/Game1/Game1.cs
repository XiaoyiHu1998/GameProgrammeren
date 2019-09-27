using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    enum gameState : int
    {
        start,
        game,
        gameOver
    };

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D paddleRed;
        Texture2D paddleBlue;
        Texture2D ball;
        
        int gamestate = (int)gameState.start;
        int player1HP = 3;
        int player2HP = 3;

        KeyboardState input = Keyboard.GetState();
        System.Random rand = new System.Random();

        Vector2 ballLocation = new Vector2(350, 225);
        Vector2 ballVelocity;
        Vector2 ballAccelleration = new Vector2(0.35f, 0);


        Vector2 player1Location = new Vector2(50, 200);
        Vector2 player2Location = new Vector2(735, 200);
        

        float[] yAccelarationArrayPos = new float[] { 0.5f, 1.0f, 1.5f, 2.0f };
        float[] yAccelarationArrayNeg = new float[]  { 0.5f, -1.0f, -1.5f, -2.0f };
        
        public Game1()
        {
            System.Random randInit = new System.Random();
            Vector2 ballVelocity = new Vector2(2,rand.Next(3, 4));
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
            font = Content.Load<SpriteFont>("font");
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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (gamestate)
            {
                case 0:
                    //check input from keyboard
                    input = Keyboard.GetState();
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || input.IsKeyDown(Keys.Escape))
                        Exit();

                    if (input.IsKeyDown(Keys.Space))
                    {
                        gamestate = (int)gameState.game;
                        player1HP = 3;
                        player2HP = 3;
                        player1Location = new Vector2(50, 200);
                        player2Location = new Vector2(GraphicsDevice.Viewport.Width - 75, 200);
                    }
                    break;

                case 1:
                    //check input from keyboard
                    input = Keyboard.GetState();

                    if (ballVelocity.X == 0)
                    {
                        ballVelocity.X = 2;
                    }

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

                        if(ballLocation.X <= 0)
                        {
                            player1HP--;
                        }
                        else
                        {
                            player2HP--;
                        }


                        ballLocation.X = GraphicsDevice.Viewport.Bounds.Width / 2;
                        ballLocation.Y = GraphicsDevice.Viewport.Bounds.Height / 2;
                        int xDirection = rand.Next(0, 2);
                        
                        if(xDirection == 0)
                        {

                            ballVelocity = new Vector2(-2, rand.Next(-4, 4));
                        }
                        else
                        {
                            ballVelocity = new Vector2(2, rand.Next(-4, 4));
                        }


                        if(player1HP <= 0 || player2HP <= 0)
                        {
                            gamestate = (int)gameState.gameOver;
                        }
                    }

                    //check Y wall collisions
                    if (ballLocation.Y <= 0 || ballLocation.Y >= GraphicsDevice.Viewport.Bounds.Height - 15)
                    {
                        ballVelocity.Y *= -1.0f;
                    }

                    //check playerColliisions and ball x direction
                    if (ballPaddleCollision())
                    {
                        ballVelocity.X *= -1.0f;

                        //if (rand.Next(0, 2) == 1)
                        //{
                        //    ballVelocity.Y += yAccelarationArrayNeg[rand.Next(0, 4)];
                        //}
                        //else
                        //{
                        //    ballVelocity.Y += yAccelarationArrayPos[rand.Next(0, 4)];
                        //}

                        if(ballLocation.X > 500)
                        {
                            if(ballLocation.Y <= player2Location.Y + 50)
                            {
                                ballVelocity.Y = rand.Next(0, 2) * (ballLocation.Y / player2Location.Y) + 1;
                            }
                            else
                            {
                                ballVelocity.Y = rand.Next(-2, 0) * (ballLocation.Y / player2Location.Y) - 1;
                            }
                        }
                        else
                        {
                            if (ballLocation.Y <= player1Location.Y + 50)
                            {
                                ballVelocity.Y = rand.Next(0, 2) * (ballLocation.Y / player1Location.Y) + 1;
                            }
                            else
                            {
                                ballVelocity.Y = rand.Next(-2, 0) * (ballLocation.Y / player1Location.Y) - 1;
                            }
                        }

                    }


                    //update ballvelocity
                    if (ballPaddleCollision() && ballVelocity.X < 3.3f)
                    {
                        if (ballVelocity.X > 0)
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
            
                    break;

                case 2:
                    //check input from keyboard
                    input = Keyboard.GetState();
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || input.IsKeyDown(Keys.Escape))
                        Exit();

                    if (input.IsKeyDown(Keys.Enter))
                    {
                        gamestate = (int)gameState.start;
                    }

                    break;
                default:
                    break;

            }
        }
            

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 textLocationStart = new Vector2(GraphicsDevice.Viewport.Width / 2 - 85, GraphicsDevice.Viewport.Height / 2 - 45);
            Vector2 textLocationHP1 = new Vector2(5, 10);
            Vector2 textLocationHP2 = new Vector2(GraphicsDevice.Viewport.Width - 40, 10);
            Vector2 textLocationEnd = new Vector2(GraphicsDevice.Viewport.Width / 2 - 125, GraphicsDevice.Viewport.Height / 2 - 45);

            switch (gamestate)
            {
                case 0:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Press space to start!", textLocationStart, Color.Black);
                    spriteBatch.End();

                    break;
                case 1:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.Draw(paddleRed, player1Location, Color.White);
                    spriteBatch.Draw(paddleBlue, player2Location, Color.White);
                    spriteBatch.Draw(ball, ballLocation, Color.White);
                    spriteBatch.DrawString(font, "HP: " + player1HP, textLocationHP1, Color.Black);
                    spriteBatch.DrawString(font, "HP:" + player2HP, textLocationHP2, Color.Black);
                    spriteBatch.End();
                    break;
                case 2:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Game Over: press Enter to return to start!", textLocationEnd, Color.Black);
                    spriteBatch.End();
                    break;
                default:
                    break;
            }
            

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}


