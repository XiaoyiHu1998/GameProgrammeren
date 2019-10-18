using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
        bool[,] rotatedright = RotateMatrix(blocks, 4);

        bool[,] RotateMatrix(bool[,] matrix, int n)
        {
            bool[,] newGrid = new bool[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    newGrid[i, j] = matrix[n - j - 1, i];
                }
            }

            blocks = newGrid;
            return newGrid;
        }
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
    public block_T(Vector2 location)
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

class BlockSelector
{
    static Vector2 startLocation = new Vector2(5,0);
    TetrisBlock[] blockArray = { new block_I(startLocation), new block_square(startLocation),
                                 new block_L(startLocation), new block_L_inverse(startLocation),
                                 new block_z(startLocation), new block_z_inverse(startLocation),
                                 new block_T(startLocation)
                                };

    public TetrisBlock SelectNextBlock(int selector)
    {
        return blockArray[selector];
    }
};


class TetrisGrid
{
    Texture2D emptyCell;
    Vector2 position;
    
    public int Width { get { return 10; } }
    public int Height { get { return 20; } }

    static BlockSelector blockSelector = new BlockSelector();
    static Random random = new Random();
    List<int> pointbuffer;
    public Color[,] grid = new Color[20, 10];
    Vector2 currentDrawPosition = Vector2.Zero;
    TetrisBlock activeBlock = new block_L(new Vector2(3,0));
    TetrisBlock nextBlock = blockSelector.SelectNextBlock(random.Next(0,6));

    float timer = 30.0f;
    int input;
    
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
        pointbuffer = new List<int> {0};
        Clear();
    }
    
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        currentDrawPosition = position;
        Vector2 blockPosition = activeBlock.getLocation();
        Color blockColor = activeBlock.getColor();
        bool[,] blockArray = activeBlock.Read();

        Vector2 nextBlockPreviewLocation = new Vector2(400, 140);
        Color nextBlockColor = nextBlock.getColor();
        bool[,] nextBlockArray = nextBlock.Read();
        UpdateGrid();

        for(int i = 0; i < Height; i++)
        {
            for(int j = 0; j < Width; j++)
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

        //preview of nextBlock
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (nextBlockArray[i, j])
                {
                    spriteBatch.Draw(emptyCell, new Vector2((nextBlockPreviewLocation.X + (new Vector2(i, j).X) * 30), (nextBlockPreviewLocation.Y + (new Vector2(i, j).Y) * 30)), nextBlockColor);
                }
                else
                {
                    spriteBatch.Draw(emptyCell, new Vector2((nextBlockPreviewLocation.X + (new Vector2(i, j).X) * 30), (nextBlockPreviewLocation.Y + (new Vector2(i, j).Y) * 30)), Color.Gray);
                }

            }
        }

        //debug view of currentBlock
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (blockArray[i, j])
                {
                    spriteBatch.Draw(emptyCell, new Vector2((400 + (new Vector2(i, j).X) * 30), (400 + (new Vector2(i, j).Y) * 30)), Color.Green);
                }
                else
                {
                    spriteBatch.Draw(emptyCell, new Vector2((400 + (new Vector2(i, j).X) * 30), (400 + (new Vector2(i, j).Y) * 30)), Color.Red);
                }

            }
        }

    }
    
    public void Clear()
    {
        for(int i = 0; i < Height; i ++)
        {
            for(int j = 0; j < Width; j++)
            {
                grid[i, j] = Color.White;
            }
        }
    }

    public void setInput(int i)
    {
        input = i;
    }

    public void moveTetrisBlock(int input,ref TetrisBlock activeBlock, bool canGoDownTwice, Vector2 position)
    {
        int xDisplacement = 0;
        int yDisplacement = 1;
        bool[,] blockArray = activeBlock.Read();

        switch (input)
        {
            case 0:
                //move left
                if(!checkCollisionSides())
                {
                    activeBlock.updateLocation(new Vector2(-1, 0));
                }

                break;
            case 1:
                //move right
                if (!checkCollisionSides())
                {
                    activeBlock.updateLocation(new Vector2(1, 0));
                }

                break;
            case 2:
                //move down
                for(int i = 0; i < 2; i++)
                {
                    if (!checkBlockOnBlockCollision())
                    {
                        activeBlock.updateLocation(new Vector2(0,0));
                    }
                }
                break;
            case 3:
                //rotate right
                activeBlock.rotateRight();
                if(checkBlockOnBlockCollision())
                {
                    activeBlock.rotateRight();
                    activeBlock.rotateRight();
                    activeBlock.rotateRight();
                }
                break;
            case 4:
                //rotate left
                activeBlock.rotateRight();
                activeBlock.rotateRight();
                activeBlock.rotateRight();

                if (checkBlockOnBlockCollision())
                {
                    activeBlock.rotateRight();
                }

                break;
            case 1000:

                break;
            default:
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
                activeBlock.shiftDown();
            }
        }

        activeBlock.updateLocation(new Vector2(xDisplacement, yDisplacement));
    }

    public bool checkBlockOnBlockCollision()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();
        bool hasCollided = false;

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x] && grid[y + (int)activeBlockPosition.Y, x + (int)activeBlockPosition.X] != Color.White)
                {
                    if ((int)activeBlockPosition.Y + y < 19)
                    {
                        hasCollided = grid[y + (int)activeBlockPosition.Y + 1, x + (int)activeBlockPosition.X] != Color.White;
                    }
                    else if ((int)activeBlockPosition.Y + y == 19)
                    {
                        hasCollided = true;
                    }

                }
            }
        }

        return hasCollided;
    }

    public bool checkCollisionSides()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();
        bool hasCollided = false;

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x] && x + (int)activeBlockPosition.X  == 6 || x + (int)activeBlockPosition.X <= 0)
                {
                    hasCollided = true;
                }
            }
        }

        return hasCollided;
    }


    public void UpdateGrid()
    {
        if(timer > 0)
        {
            timer -= 0.8f;
        }
        else
        {
            Color color = activeBlock.getColor();
            Vector2 position = activeBlock.getLocation();
            bool[,] blockArray = activeBlock.Read();
            bool blockHit = false;
            bool canGoDownTwice = true;
            bool rowCleared = false;

            if (checkBlockOnBlockCollision())
            {
                blockHit = true;
            }

            if (blockHit)
            {
                if (!rowCleared)
                {
                    //create new active activeBlock
                    //add 10 points to pointbuffer
                    activeBlock = nextBlock;
                    nextBlock = blockSelector.SelectNextBlock(random.Next(0, 6));
                    pointbuffer.Add(10);
                }

                //put the old activeblock into the grid
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
                moveTetrisBlock(input, ref activeBlock, canGoDownTwice, position);
            }

            timer = 30.0f;
        }
        
    }

    public int returnPointBuffer()
    {
        int totalPoints = 0;

        for(int i = 0; i < this.pointbuffer.Count; i++)
        {
            totalPoints += pointbuffer[i];
        }

        pointbuffer = new List<int> {0};
        return totalPoints;
    }
}

