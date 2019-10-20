using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    public enum GameState
    {
        StartScreen,
        Playing,
        GameOver
    }

    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get { return random; } }
    static Random random;

    /// <summary>
    /// The main font of the game.
    /// </summary>
    SpriteFont font;

    /// <summary>
    /// The current game state.
    /// </summary>
    public GameState gameState;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid;

    /// <summary>
    /// The score of the player.
    /// </summary>
    int score;

    /// <summary>
    /// The level of the player based on points.
    /// </summary>
    int level;

    /// <summary>
    /// The level of the player based on points.
    /// </summary>
    float levelThreshold;

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;
        score = 0;
        level = 1;
        levelThreshold = 75;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Start()
    {
        gameState = GameState.Playing;
    }

    public void Update(GameTime gameTime)
    {
        grid.UpdateGrid();
        score += grid.returnPointBuffer() * (1 + level / 10);
        if(score >= levelThreshold)
        {
            level++;
            levelThreshold *= 1.5f;
            grid.timerLength = grid.timerLength * (float)0.95;
        }
        if (grid.gameover)
        {
            gameState = GameState.GameOver;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        {
            Vector2 textLocationStart = new Vector2(225, 260);
            Vector2 textLocationControls = new Vector2(225, 300);
            Vector2 textLocationEnd = new Vector2(400, 50);
            Vector2 textLocationScore = new Vector2(400, 50);
            Vector2 textLocationLevel = new Vector2(400, 80);
            Vector2 textLocationVolgendBlock = new Vector2(400, 110);

            switch ((int)gameState)
            {
                case 0:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Press space to start!", textLocationStart, Color.Black);
                    spriteBatch.DrawString(font, "Use A and D for left right, Q and E for rotation", textLocationControls, Color.Black);
                    spriteBatch.End();

                    break;
                case 1:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Score: " + score, textLocationScore, Color.Black);
                    spriteBatch.DrawString(font, "Level: " + level, textLocationLevel, Color.Black);
                    spriteBatch.DrawString(font, "Volgend blok:", textLocationVolgendBlock, Color.Black);
                    spriteBatch.DrawString(font, "y position: " + grid.getBlockPosition().Y, new Vector2(400, 350), Color.Black);
                    grid.Draw(gameTime, spriteBatch);
                    spriteBatch.End();
                    break;
                case 2:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Game Over: press Enter to return to start!", textLocationEnd, Color.Black);
                    spriteBatch.End();
                    break;
                default:
                    break;
            }
        }
    }

    public void Reset()
    {
        gameState = GameState.StartScreen;
        score = 0;
        level = 1;
        levelThreshold = 50;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
    }

    public void Input(int integer)
    {
        grid.setInput(integer);
    }

    protected void incrementScoreBlock()
    {
        score += 10;
    }

    protected void incrementScoreRow()
    {
        score += 100;
    }

}
