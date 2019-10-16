using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


class TetrisGame : Game
{
    SpriteBatch spriteBatch;
    InputHelper inputHelper;
    GameWorld gameWorld;

    /// <summary>
    /// A static reference to the ContentManager object, used for loading assets.
    /// </summary>
    public static ContentManager ContentManager { get; private set; }
    

    /// <summary>
    /// A static reference to the width and height of the screen.
    /// </summary>
    public static Point ScreenSize { get; private set; }

    [STAThread]
    static void Main(string[] args)
    {
        TetrisGame game = new TetrisGame();
        game.Run();
    }

    public TetrisGame()
    {        
        // initialize the graphics device
        GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);

        // store a static reference to the content manager, so other objects can use it
        ContentManager = Content;
        
        // set the directory where game assets are located
        Content.RootDirectory = "Content";

        // set the desired window size
        ScreenSize = new Point(800, 600);
        graphics.PreferredBackBufferWidth = ScreenSize.X;
        graphics.PreferredBackBufferHeight = ScreenSize.Y;

        // create the input helper object
        inputHelper = new InputHelper();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // create and reset the game world
        gameWorld = new GameWorld();
        gameWorld.Reset();

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(Content.Load<Song>("snd_music"));
       // SoundEffect blocksound = Content.Load<SoundEffect>("snd_click");
    }

    protected override void Update(GameTime gameTime)
    {
        inputHelper.Update(gameTime);
        gameWorld.HandleInput(gameTime, inputHelper);
        gameWorld.Update(gameTime);

        if(inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
        {
            gameWorld.Input(0);
        }
        else if(inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
        {
            gameWorld.Input(1);
        }
        else if(inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.S))
        {
            gameWorld.Input(2);
        }
        else if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.E))
        {
            gameWorld.Input(3);
        }
        else if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
        {
            gameWorld.Input(4);
        }
        else{
            gameWorld.Input(1000);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        gameWorld.Draw(gameTime, spriteBatch);
    }
}

