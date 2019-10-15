using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;


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

        for(int j = 0; j < 4; j++)
        {
            blocks[0, j] = false;
        }
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

    public Color[,] grid = new Color[20, 10];
    Vector2 currentDrawPosition = Vector2.Zero;
    TetrisBlock tetrisblock = new block_L(Vector2.Zero);
    float timer = 30.0f;
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
        Vector2 blockPosition = tetrisblock.getLocation();
        Color blockColor = tetrisblock.getColor();
        bool[,] blockArray = tetrisblock.Read();
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
        
        if(blockPosition.Y < 16)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (blockArray[i, j])
                        spriteBatch.Draw(emptyCell, new Vector2((blockPosition.X + new Vector2(i, j).X) * 30, (blockPosition.Y + new Vector2(i, j).Y) * 30), blockColor);
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (blockArray[i, j])
                {
                    spriteBatch.Draw(emptyCell, new Vector2((400 + (new Vector2(i, j).X) * 30), (200 + (new Vector2(i, j).Y) * 30)), Color.Green);
                }
                else
                {
                    spriteBatch.Draw(emptyCell, new Vector2((400 + (new Vector2(i, j).X) * 30), (200 + (new Vector2(i, j).Y) * 30)), Color.Red);
                }

            }
        }

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

    public void moveTetrisBlock(int input,ref TetrisBlock tetrisBlock, bool canGoDownTwice, Vector2 position)
    {
        int xDisplacement = 0;
        int yDisplacement = 1;
        bool[,] blockArray = tetrisBlock.Read();

        switch (input)
        {
            case 0:
                bool leftSideEmpty = true;

                if (position.X > 0)
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
                    {
                        tetrisblock.shiftLeft();
                    }
                }

                break;
            case 1:
                bool rightSideEmpty = true;

                if (position.X < 6)
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
                    {
                        tetrisblock.shiftRight();
                    }
                }
                break;
            case 2:
                if (canGoDownTwice)
                    yDisplacement = 2;
                break;
        }

        if(position.Y == 15)
        {
            bool bottomSideEmpty = true;
            yDisplacement = 0;

            for(int i = 0; i < 4; i++)
            {
                if (blockArray[i, 3])
                    bottomSideEmpty = false;
            }

            if (bottomSideEmpty)
            {
                tetrisblock.shiftDown();
            }
        }

        tetrisblock.updateLocation(new Vector2(xDisplacement, yDisplacement));
    }

    public void UpdateGrid()
    {
        if(timer > 0)
        {
            timer -= 0.8f;
        }
        else
        {
            Color color = tetrisblock.getColor();
            Vector2 position = tetrisblock.getLocation();
            bool[,] blockArray = tetrisblock.Read();
            bool blockHit = false;
            bool canGoDownTwice = true;

            if (position.Y < 15)
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
            else if (position.Y < 16)
            {
                for (int i = 3; i > 1; i--)
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
            else if (position.Y == 16)
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
                        if (blockArray[i, j])
                        {
                            grid[i + (int)position.Y, j + (int)position.X] = color;
                        }
                    }
                }
            }
            else
            {
                moveTetrisBlock(input, ref tetrisblock, canGoDownTwice, position);
            }

            timer = 30.0f;
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

