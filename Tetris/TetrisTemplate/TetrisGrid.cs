using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TetrisBlock
{
    protected bool[,] blocks = new bool[4, 4];
    Vector2 Location;
    Color color;
    int[] dimensions;

    public TetrisBlock(Vector2 location, bool[,] blockArray, Color blockColor, int[] blockDimensions)
    {
        Location = location;
        color = blockColor;
        dimensions = blockDimensions;
        blocks = blockArray;
    }

    public void rotateRight()
    {

    }

    public void rotateLeft()
    {

    }

    public bool[,] Read()
    {
        return blocks;
    }

    public Vector2 getLocation()
    {
        return Location;
    }

    public void updateLocation(Vector2 displacement)
    {
        Location += displacement;
    }

    public Color getColor()
    {
        return color;
    }

    public int[] getDimensions()
    {
        return dimensions;
    }

    public void shiftLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                blocks[i, j] = blocks[i, j++];
            }

            blocks[i, 3] = false;
        }
    }

    public void shiftRight()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 3; j > 0; j--)
            {
                blocks[i, j] = blocks[i, j--];
            }

            blocks[i, 0] = false;
        }
    }

    public void shiftDown()
    {
        for (int i = 3; i > 0; i--)
        {
            for (int j = 0; j < 4; j++)
            {
                blocks[i, j] = blocks[i--, j];
            }
        }

        for (int j = 0; j < 4; j++)
        {
            blocks[0, j] = false;
        }
    }
}

class block_square : TetrisBlock
{
    public block_square(Vector2 location)
        : base(
                location,

                new bool[,] {
                    { false,false,false,false },
                    { false,true,true,false },
                    { false,true,true,false },
                    { false,false,false,false }
                },

                Color.Pink,
                new int[] { 2, 2 }
            )
    { }
}

class block_T : TetrisBlock
{
    public block_T(Vector2 location, int[] blockDimensions)
        : base(
                location,

                new bool[,] {
                    { false,false,false,false },
                    { true,true,true,false },
                    { false,true,false,false },
                    { false,false,false,false }
                },

                Color.Blue,
                new int[] { 3, 2 }
            )
    { }
}

class block_L : TetrisBlock
{
    public block_L(Vector2 location)
        : base(
                location,

                new bool[,] {
                    { false,true,false,false },
                    { false,true,false,false },
                    { false,true,true,false },
                    { false,false,false,false }
                },

                Color.Purple,
                new int[] { 2, 3 }
            )
    { }
}


class block_L_inverse : TetrisBlock
{
    public block_L_inverse(Vector2 location)
        : base(
                location,

                new bool[,] {
                    { false,false,true,false },
                    { false,false,true,false },
                    { false,true,true,false },
                    { false,false,false,false }
                },

                Color.Orange,
                new int[] { 2, 3 }
            )
    { }
}


class block_z : TetrisBlock
{
    public block_z(Vector2 location)
        : base(
                location,

                new bool[,] {
                    { false,false,false,false },
                    { true,true,false,false },
                    { false,true,true,false },
                    { false,false,false,false }
                },

                Color.Yellow,
                new int[] { 3, 2 }
            )
    { }
}


class block_z_inverse : TetrisBlock
{
    public block_z_inverse(Vector2 location)
        : base(
                location,

                new bool[,] {
                    { false,false,false,false },
                    { false,false,true,true },
                    { false,true,true,false },
                    { false,false,false,false }
                },

                Color.Green,
                new int[] { 3, 2 }
            )
    { }
}

class block_I : TetrisBlock
{
    public block_I(Vector2 location)
        : base(
                location,

                new bool[,] {
                    { false,true,false,false },
                    { false,true,false,false },
                    { false,true,false,false },
                    { false,true,false,false }
                },

                Color.Red,
                new int[] { 1, 4 }
            )
    { }
}

class ColorGrid
{
    private Color[,] grid = new Color[20, 10];

    ColorGrid()
    {
        
    }
    
}

class TetrisGrid
{
    Texture2D emptyCell;

    Vector2 position;

    public int Width { get { return 10; } }

    public int Height { get { return 20; } }

    //use grid[y,x] for access.
    Color[,] grid = new Color[10, 20];

    TetrisBlock ActiveBlock = new block_L(new Vector2(0, 5));

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
        Vector2 currentLocation = Vector2.Zero;
        
        //draw the Color
        for(int y = 0; y < 20; y++)
        {
            for(int x = 0; x < 10; x++)
            {
                spriteBatch.Draw(emptyCell, new Vector2(x * 30, y * 30), grid[y,x]);
            }
        }


    }

    
    public void Clear()
    {
    }
}

