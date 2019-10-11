using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
}

class block_square : TetrisBlock
{ 
    public block_square(Vector2 location)
        :base(
                location,

                new bool[,] {
                    { false,false,false,false },
                    { false,true,true,false },
                    { false,true,true,false },
                    { false,false,false,false }
                },

                Color.Pink, 
                new int[] { 2, 2}
            )
    {}
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


class TetrisGrid
{
    Texture2D emptyCell;
    Vector2 position;
    
    public int Width { get { return 10; } }
    public int Height { get { return 20; } }

    Color[,] grid = new Color[20, 10];
    Vector2 currentDrawPosition = Vector2.Zero;
    TetrisBlock tetrisblock = new block_square(Vector2.Zero);
    int input;
    
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
        Clear();
    }
    
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        currentDrawPosition = position;
        UpdateGrid();

        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                spriteBatch.Draw(emptyCell, currentDrawPosition, grid[i, j]);
                currentDrawPosition.X += 30;
            }
            currentDrawPosition.X = 0;
            currentDrawPosition.Y += 30;
        }

        currentDrawPosition = Vector2.Zero;
        
    }
    
    public void Clear()
    {
        for(int i = 0; i < 20; i ++)
        {
            for(int j = 0; j < 10; j++)
            {
                grid[i, j] = Color.White;
            }
        }
    }

    public void addTetrisBlock()
    {
        //tetrisblocks.Add(new TetrisBlock());
    }

    public void setInput(int i)
    {
        input = i;
    }

    public void UpdateGrid()
    {
        Clear();
        Color color = tetrisblock.getColor();
        Vector2 position = tetrisblock.getLocation();
        bool[,] blockArray = tetrisblock.Read();
        bool blockHit = false;
        bool canGoDownTwice = true;

        if(position.X < 18)
        {
            for (int i = 3; i > 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {

                    if (grid[i + (int)position.Y + 1, j + (int)position.X] != Color.White)
                    {
                        blockHit = true;
                    }

                    if (grid[i + (int)position.Y + 2, j + (int)position.X] != Color.White)
                    {
                        canGoDownTwice = false;
                    }
                }
            }
        }
        else if (position.X < 19)
        {
            for (int i = 3; i > 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {

                    if (grid[i + (int)position.Y + 1, j + (int)position.X] != Color.White)
                    {
                        blockHit = true;
                    }

                    canGoDownTwice = false;
                }
            }
        }
        else if(position.Y == 20)
        {
            blockHit = true;
            canGoDownTwice = false;
        }

        if (blockHit)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (blockArray[i,j])
                    {
                        grid[i + (int)position.Y, j + (int)position.X] = color;
                    }
                }
            }
        }
        else
        {
            int xDisplacement = 0;
            int yDisplacement = 1;
            switch(input)
            {
                case 0:
                    bool leftSideEmpty = true;

                    if(position.Y > 0)
                    {
                        xDisplacement = -1;
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (blockArray[i, 0])
                                leftSideEmpty = false;
                        }


                        if (leftSideEmpty)
                            xDisplacement = -1;
                    }

                    break;
                case 1:
                    bool rightSideEmpty = true;

                    if (position.Y < 10)
                    {
                        xDisplacement = 1;
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (blockArray[i, 3])
                                rightSideEmpty = false;
                        }


                        if (rightSideEmpty)
                            xDisplacement = 1;
                    }
                    break;
                case 2:
                    if (canGoDownTwice)
                        yDisplacement = 2;
                    break;
            }

            tetrisblock.updateLocation(new Vector2(yDisplacement, xDisplacement));
        }

        //for (int i = 0; i < tetrisblocks.Count; i++)
        //{
        //    color = tetrisblocks[i].getColor();
        //    position = tetrisblocks[i].getLocation();
        //    blockArray = tetrisblocks[i].Read();
            
        //    for(int j = 0; j < 4; j++)
        //    {
        //        for (int k = 0; k < 4; k++)
        //        {
        //            if(blockArray[j,k])
        //            {
        //                grid[j + (int)position.X, k + (int)position.Y] = color;
        //            }
        //        }
        //    }

        //}
        
    }
}

