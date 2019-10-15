using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class TetrisGrid
{
    Texture2D emptyCell;
    
    Vector2 position;
    
    public int Width { get { return 10; } }
   
    public int Height { get { return 20; } }
    
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
        Clear();
    }
    
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }
    
    public void Clear()
    {
    }
}

