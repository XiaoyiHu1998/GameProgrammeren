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


    public TetrisBlock(Vector2 location, bool[,] blockArray, Color blockColor)
    {
        Location = location;
        color = blockColor;
        blocks = blockArray;
    }

    public void rotateRight()
    {
        bool[,] newArray = new bool[4, 4];

        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                newArray[i, j] = blocks[3 - j, i];
            }
        }

        blocks = newArray;
    }
    
    public bool[,] Read()
    {
        return blocks;
    }

    public Vector2 getLocation()
    {
        return Location;
    }

    public void setLocation(Vector2 displacement)
    {
        Location = displacement;
    }

    public void updateLocation(Vector2 displacement)
    {
        Location += displacement;
    }

    public Color getColor()
    {
        return color;
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

                Color.Pink
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

                Color.Blue
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

                Color.Purple
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

                Color.Orange
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

                Color.Yellow
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

                Color.Green
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

                Color.Red
            )
    { }
}

class BlockSelector
{
    static Vector2 startLocation = new Vector2(4,0);
    TetrisBlock[] blockArray = { new block_I(startLocation), new block_square(startLocation),
                                 new block_L(startLocation), new block_L_inverse(startLocation),
                                 new block_z(startLocation), new block_z_inverse(startLocation),
                                 new block_T(startLocation)
                                };

    public TetrisBlock SelectNextBlock(int selector)
    {
        blockArray = new TetrisBlock[] {
                                            new block_I(startLocation), new block_square(startLocation),
                                            new block_L(startLocation), new block_L_inverse(startLocation),
                                            new block_z(startLocation), new block_z_inverse(startLocation),
                                            new block_T(startLocation)
                                        };

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
    public bool gameover;
    List<int> fullRowIndexBuffer = new List<int> {-1};

    public float timerLength = 20.0f;
    float timer = 20.0f;
    int input;
    
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
        pointbuffer = new List<int> {0};
        gameover = false;
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

        //draw background grid
        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                spriteBatch.Draw(emptyCell, currentDrawPosition, grid[y, x]);
                currentDrawPosition.X += 30;
            }
            currentDrawPosition.X = 0;
            currentDrawPosition.Y += 30;
        }
        
        //draw active block
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x])
                    spriteBatch.Draw(emptyCell, new Vector2((blockPosition.X + (float)x) * 30, (blockPosition.Y + (float)y) * 30), blockColor);
            }
        }

        //preview of nextBlock
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (nextBlockArray[y, x])
                {
                    spriteBatch.Draw(emptyCell, new Vector2(nextBlockPreviewLocation.X + (float)x * 30, nextBlockPreviewLocation.Y + (float)y * 30), nextBlockColor);
                }
                else
                {
                    spriteBatch.Draw(emptyCell, new Vector2(nextBlockPreviewLocation.X + (float)x * 30, nextBlockPreviewLocation.Y + (float)y * 30), Color.White);
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

    public void moveTetrisBlock(int input,ref TetrisBlock activeBlock, Vector2 position)
    {
        switch (input)
        {
            case 0:
                //move left
                if(!checkLeftBoundCollision())
                    activeBlock.updateLocation(new Vector2(-1, 0));

                if(checkInternalCollision())
                    activeBlock.updateLocation(new Vector2(1, 0));

                break;
            case 1:
                //move right
                if (!checkRightBoundCollision())
                    activeBlock.updateLocation(new Vector2(1, 0));

                if(checkInternalCollision())
                    activeBlock.updateLocation(new Vector2(-1, 0));

                break;
            case 2:
                //move down
                while (true)
                {
                    activeBlock.updateLocation(new Vector2(0, 1));

                    if (checkFallCollision())
                    {
                        activeBlock.updateLocation(new Vector2(0,-1));
                        break;
                    }

                }
                break;
            case 3:
                //rotate right
                activeBlock.rotateRight();

                if(checkInGrid())
                    activeBlock.rotateRight();
                    activeBlock.rotateRight();
                    activeBlock.rotateRight();

                if (checkInternalCollision())
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

                if(checkInGrid())
                    activeBlock.rotateRight();
                
                if (checkInternalCollision())
                {
                    activeBlock.rotateRight();
                }

                break;
            case 1000:

                break;
            default:
                break;
        }

        if (!checkBlockOnBlockCollision())
            activeBlock.updateLocation(new Vector2(0, 1));
    }

    public bool checkBlockOnBlockCollision()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();

        if(activeBlockPosition.Y < Height - 4)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (blockArray[y, x] && grid[y + (int)activeBlockPosition.Y + 1, x + (int)activeBlockPosition.X] != Color.White)
                        return true;
                }
            }
        }
        
        return false;
    }

    public bool checkInternalCollision()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x] && grid[y + (int)activeBlockPosition.Y, x + (int)activeBlockPosition.X] != Color.White)
                    return true;
            }
        }

        return false;
    }

    public bool checkLeftBoundCollision()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x] && x + (int)activeBlockPosition.X <= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool checkRightBoundCollision()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x] && x + (int)activeBlockPosition.X >= 9)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool checkVerticalBoundCollision()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x] && y + (int)activeBlockPosition.Y == 19 || y + (int)activeBlockPosition.Y < 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool checkInGrid()
    {
        bool[,] blockArray = activeBlock.Read();
        Vector2 activeBlockPosition = activeBlock.getLocation();

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (blockArray[y, x] && y + (int)activeBlockPosition.Y > 19 || y + (int)activeBlockPosition.Y < 0 || x + (int)activeBlockPosition.X < 0 || x + (int)activeBlockPosition.X > 9)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool checkFallCollision()
    {
        if (checkVerticalBoundCollision())
            return true;
        if (checkBlockOnBlockCollision())
            return true;

        return false;
    }

    public void indexFullRows()
    {
        for(int y = 0; y < Height; y++)
        {
            bool rowFull = true;
            for(int x = 0; x < Width; x++)
            {
                if (grid[y, x] == Color.White && rowFull)
                    rowFull = false;
            }

            if (rowFull)
            {
                pointbuffer.Add(100);
                fullRowIndexBuffer.Add(y);
            }
        }
    }

    public void cascadeRows()
    {
        if(fullRowIndexBuffer.Count > 1)
        {
            for (int i = 1; i < fullRowIndexBuffer.Count; i++)
            {
                for (int y = fullRowIndexBuffer[i]; y > 0; y--)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        grid[y, x] = grid[y - 1, x];
                    }
                }
            }

            for (int x = 0; x < Width; x++)
            {
                grid[0, x] = Color.White;
            }

            fullRowIndexBuffer = new List<int> { -1 };
        }
    }

    public void UpdateGrid()
    {
        if(timer > 0)
        {
            timer -= 0.5f;
        }
        else
        {
            Color color = activeBlock.getColor();
            Vector2 activeBlockPosition = activeBlock.getLocation();
            bool[,] blockArray = activeBlock.Read();
            bool blockHit = checkBlockOnBlockCollision() || checkVerticalBoundCollision();

            if (blockHit)
            {
                //create new active activeBlock
                //add 10 points to pointbuffer
                activeBlock = nextBlock;
                nextBlock = blockSelector.SelectNextBlock(random.Next(0, 7));
                activeBlock.setLocation(new Vector2(4, 0));
                pointbuffer.Add(10);

                //check if the game is over
                if (activeBlockPosition.Y == 0 && checkBlockOnBlockCollision())
                {
                    gameover = true;
                }

                //put the old activeblock into the grid
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (blockArray[i, j])
                        {
                            grid[i + (int)activeBlockPosition.Y, j + (int)activeBlockPosition.X] = color;
                        }
                    }
                }

                //index full rows
                indexFullRows();

                //move rows down if a row has been cleared
                cascadeRows();

            }
            else
            {
                moveTetrisBlock(input, ref activeBlock, activeBlockPosition);
            }

            timer = timerLength;
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

    public Vector2 getBlockPosition()
    {
        return activeBlock.getLocation();
    }
}

