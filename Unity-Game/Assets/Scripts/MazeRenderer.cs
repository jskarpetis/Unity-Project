using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Range(1,50)]
    private int width = 10;

    [SerializeField]
    [Range(1,50)]
    private int height = 10;
    [SerializeField]
    private float size = 10f;
    [SerializeField]
    private Transform wallPrefab = null;
    [SerializeField]
    private Transform MainFloorPrefab = null;
    [SerializeField]
    private Transform CenterFloor = null;
    [SerializeField]
    private Transform TreasureFloor = null;
    [SerializeField]
    private Transform PlayerPrefab = null;
    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
        Spawn(PlayerPrefab);
    }
    private void Spawn(Transform PlayerPrefab)
    {
        var xrandom = new System.Random();
        var zrandom = new System.Random();
        var Player = Instantiate(PlayerPrefab, transform) as Transform;
        Player.position = new Vector3(xrandom.Next(-width/2, width/2), -size / 3, zrandom.Next(-height/2, height/2));

    }
    private void Draw(WallState[,] maze)
    {
        var Room1Center = new Position { X = width / 2, Y = height / 2 };
        var Room2Center = new Position { X = 3 * width / 2, Y = height / 2 };
        var Room3Center = new Position { X = 3 * width / 2, Y = 3 * height / 2 };
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

        int checkFlag = 0;
        for (int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);
                for (int k = 0; k < 9; ++k)
                {
                    if (i == WinnerRoom[k, 0] && j == WinnerRoom[k, 1])
                    {
                        checkFlag = 1;
                    }
                    if((i == TreasureRoom1[k, 0] && j == TreasureRoom1[k, 1]) || (i == TreasureRoom2[k, 0] && j == TreasureRoom2[k, 1]) || (i == TreasureRoom3[k, 0] && j == TreasureRoom3[k, 1]))
                    {
                        checkFlag = 2;
                    }
                }
                var MainFloor2 = Instantiate(MainFloorPrefab, transform) as Transform;
                if (checkFlag == 0)
                {
                    var MainFloor = Instantiate(MainFloorPrefab, transform) as Transform;
                    MainFloor.position = position + new Vector3(0, -size/2, 0);
                    MainFloor2.position = position + new Vector3(0, size / 2, 0);
                }
                else if(checkFlag == 2)
                {
                    var treasurefloor = Instantiate(TreasureFloor, transform) as Transform;
                    treasurefloor.position = position + new Vector3(0, -size / 2, 0);
                    MainFloor2.position = position + new Vector3(0, size / 2, 0);
                    checkFlag = 0;
                }
                else
                {
                    var WinnerFloor = Instantiate(CenterFloor, transform) as Transform;
                    WinnerFloor.position = position + new Vector3(0, -size / 2, 0);
                    MainFloor2.position = position + new Vector3(0, size / 2, 0);
                    checkFlag = 0;
                }
                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size/2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);

                }
                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }
                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }
                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
