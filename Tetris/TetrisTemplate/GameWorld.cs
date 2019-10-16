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
    enum GameState
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
    GameState gameState;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid;

    /// <summary>
    /// The score of the player.
    /// </summary>
    int score;

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;
        score = 0;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
    }

    public void Update(GameTime gameTime)
    {
        grid.UpdateGrid();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        {
            Vector2 textLocationStart = new Vector2(400, 200);
            Vector2 textLocationScore = new Vector2(400, 200);
            Vector2 textLocationVolgendBlock = new Vector2(400, 250);

            switch (gameState)
            {
                case 0:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Press space to start!", textLocationStart, Color.Black);
                    spriteBatch.End();

                    break;
                case 1:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Score van levens: " + score, textLocationScore, Color.Black);
                    spriteBatch.DrawString(font, "Volgend blok:" + volgendBlok, textLocationVolgendBlock, Color.Black);
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
    }

    public void Input(int integer)
    {
        grid.setInput(integer);
    }

}
