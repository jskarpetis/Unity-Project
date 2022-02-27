using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Flags]
public enum WallState
{
    //0000 : No Walls
    //1111 : left,right,up,down
    LEFT = 1, //0001
    RIGHT = 2, //0010
    UP = 4, //0100
    DOWN = 8, //1000

    VISITED = 128, //1000 0000
}
public struct Position
{
    public int X;
    public int Y;
}

public struct Neighbour
{
    public Position Position;
    public WallState SharedWall;
}
public static class MazeGenerator
{
    // Start is called before the first frame update
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }
    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        ///-----------------------Recursive Back Tracker Algorithm-----------------------------------
        ///Input : 
        ///       -WallState[,] maze : An initialized maze with all wall L R U D
        ///       -width : The width of the maze which we choose in the inspector of unity
        ///       -height : The height of the maze which we choose in the inspector of unity
        ///Output :
        ///     A random maze.
        var rng = new System.Random();                                                                                                           //Get to a random position and start walking around
        var position = new Position { X = rng.Next(0, width), Y = rng.Next(0, height) };
        var positionStack = new Stack<Position>();

        maze[position.X, position.Y] |= WallState.VISITED;                                                                                       //Mark the first random position as visited
        positionStack.Push(position);

        while (positionStack.Count > 0)                                                                                                          //Until Stack is empty start walking around with GetUnvisitedNeighbours method
        {
            var current = positionStack.Pop();                                                                                                   //save the current position 
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);                                                               //return a list of unvisited neighbours

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbour.SharedWall);

                maze[nPosition.X, nPosition.Y] |= WallState.VISITED;
                positionStack.Push(nPosition);
            }
        }
        return maze;
    }
    private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbour>();

        if (p.X > 0) //Left
        {
            if (!maze[p.X - 1, p.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X - 1,
                        Y = p.Y
                    },
                    SharedWall = WallState.LEFT
                });
            }
        }
        if (p.Y > 0) //BOTTOM
        {
            if (!maze[p.X, p.Y - 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y - 1
                    },
                    SharedWall = WallState.DOWN
                });
            }
        }
        if (p.Y < height - 1) //UP
        {
            if (!maze[p.X, p.Y + 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y + 1
                    },
                    SharedWall = WallState.UP
                });
            }
        }
        if (p.X < width - 1) //right
        {
            if (!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X + 1,
                        Y = p.Y
                    },
                    SharedWall = WallState.RIGHT
                });
            }
        }
        return list;
    }
    public static WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.DOWN | WallState.UP;
        //var rng1 = new System.Random();
        //var rng2 = new System.Random();
        //var rng3 = new System.Random();
        var Room1Center = new Position { X = width/2, Y = height/2 };
        var Room2Center = new Position { X = 3*width/2, Y = height / 2 };
        var Room3Center = new Position { X = 3 * width / 2 , Y = 3*height / 2 };

        int[,] WinnerRoom = new int[9, 2] //center room x,y
        {
            { width / 2 - 1 , height / 2 - 1},
            { width / 2 - 1 , height / 2    },
            { width / 2 - 1 , height / 2 + 1},
            { width / 2     , height / 2 - 1},
            { width / 2     , height / 2    },
            { width / 2     , height / 2 + 1},
            { width / 2 + 1 , height / 2 - 1},
            { width / 2 + 1 , height / 2    },
            { width / 2 + 1 , height / 2 + 1},
        };
        int[,] TreasureRoom1 = new int[9, 2] //random room x,y
        {
            { Room1Center.X / 2 - 1 , Room1Center.Y / 2 - 1},
            { Room1Center.X / 2 - 1 , Room1Center.Y / 2    },
            { Room1Center.X / 2 - 1 , Room1Center.Y / 2 + 1},
            { Room1Center.X / 2     , Room1Center.Y / 2 - 1},
            { Room1Center.X / 2     , Room1Center.Y / 2    },
            { Room1Center.X / 2     , Room1Center.Y / 2 + 1},
            { Room1Center.X / 2 + 1 , Room1Center.Y / 2 - 1},
            { Room1Center.X / 2 + 1 , Room1Center.Y / 2    },
            { Room1Center.X / 2 + 1 , Room1Center.Y / 2 + 1},
        };
        int[,] TreasureRoom2 = new int[9, 2] //random room x,y
        {
            { Room2Center.X / 2 - 1 , Room2Center.Y  / 2 - 1},
            { Room2Center.X  / 2 - 1 , Room2Center.Y  / 2    },
            { Room2Center.X  / 2 - 1 , Room2Center.Y  / 2 + 1},
            { Room2Center.X  / 2     , Room2Center.Y  / 2 - 1},
            { Room2Center.X  / 2     , Room2Center.Y  / 2    },
            { Room2Center.X  / 2     , Room2Center.Y  / 2 + 1},
            { Room2Center.X  / 2 + 1 , Room2Center.Y  / 2 - 1},
            { Room2Center.X  / 2 + 1 , Room2Center.Y  / 2    },
            { Room2Center.X  / 2 + 1 , Room2Center.Y  / 2 + 1},
        };
        int[,] TreasureRoom3 = new int[9, 2] //random room x,y
        {
            { Room3Center.X  / 2 - 1 , Room3Center.Y / 2 - 1},
            { Room3Center.X / 2 - 1 , Room3Center.Y / 2    },
            { Room3Center.X / 2 - 1 , Room3Center.Y / 2 + 1},
            { Room3Center.X / 2     , Room3Center.Y / 2 - 1},
            { Room3Center.X / 2     , Room3Center.Y / 2    },
            { Room3Center.X / 2     , Room3Center.Y / 2 + 1},
            { Room3Center.X / 2 + 1 , Room3Center.Y / 2 - 1},
            { Room3Center.X / 2 + 1 , Room3Center.Y / 2    },
            { Room3Center.X / 2 + 1 , Room3Center.Y / 2 + 1},
        };

        int checkFlag = 0;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                for(int k = 0; k < 9; ++k)
                {
                    if((i==WinnerRoom[k,0] && j == WinnerRoom[k,1]) || (i == TreasureRoom1[k, 0] && j == TreasureRoom1[k, 1]) || (i == TreasureRoom2[k, 0] && j == TreasureRoom2[k, 1]) || (i == TreasureRoom3[k, 0] && j == TreasureRoom3[k, 1]))
                    {
                        checkFlag =1;
                    }
                }
                if (checkFlag == 0)
                {
                    maze[i, j] = initial;
                }
                else
                {
                    checkFlag = 0;
                }
            }
        }
        return ApplyRecursiveBacktracker(maze, width, height);
    }
}
