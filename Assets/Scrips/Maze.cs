using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[SerializeField]
public class MapLocation
{
    public int x;
    public int z;


}


public class Maze : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>();
    public int width = 30;
    public int depth = 30;
    public byte[,] map;
    public int scale = 6;

    System.Random rng = new System.Random();

    void Start()
    {
        SetMapLocation();
        initalisMap();
        Generate();
        GenerateList(5, 5);
        DrawMap();
    }


    void SetMapLocation()
    {
        MapLocation location = new MapLocation();
        location.x = 1;
        location.z = 0;
        directions.Add(location);

        MapLocation location1 = new MapLocation();
        location1.x = 0;
        location1.z = 1;
        directions.Add(location1);

        MapLocation location2 = new MapLocation();
        location2.x = -1;
        location2.z = 0;
        directions.Add(location2);

        MapLocation location3 = new MapLocation();
        location3.x = 0;
        location3.z = -1;
        directions.Add(location3);


    }
    void initalisMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;
            }
        }
    }

    public virtual void Generate()
    {
        //for (int z = 0; z < depth; z++)
        //  fir (int x = 0; x <width, x++)
        //{
        //   //if(Random.Range(0,100) < 50)
        //map [x,z] = 0;    ..1 = wall 0 = coridor
        //}
    }

    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
        }
    }


    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    public void Shuffle(List<MapLocation> mapLocations)
    {
        int n = mapLocations.Count;

        while (n > 1)
        {
            n--;

            int k = rng.Next(n + 1);
            MapLocation value = mapLocations[k];
            mapLocations[k] = mapLocations[n];
            mapLocations[n] = value;
        }

    }

    void Generate(int x, int z)
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        int startX = x;
        int startZ = z;
        map[startX, startZ] = 0;
        stack.Push(new Vector2Int(startX, startZ));

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();
            Shuffle(directions);
            bool moved = false;
            foreach (MapLocation dir in directions)
            {
                int changX = current.x + dir.x;
                int changZ = current.y + dir.z;

                if (!(CountSquareNeighbours(changX, changZ) >= 2 || map[changX, changZ] == 0))
                {
                    map[changX, changZ] = 0;
                    stack.Push(new Vector2Int(changX, changZ));
                    moved = true;
                    break;
                }
            }
        }
    }


    void GenerateList(int x, int z)
    {
        List<MapLocation> mapDatas = new List<MapLocation>();
        map[x, z] = 0;
        MapLocation mapData = new MapLocation();
        mapData.x = x;
        mapData.z = z;
        mapDatas.Add(mapData);

        while (mapDatas.Count > 0)
        {
            MapLocation current = mapDatas[0];
            Shuffle(directions);
            bool moved = false;

            foreach (MapLocation dir in directions)
            {
                int changeX = current.x + dir.x;
                int changeZ = current.z + dir.z;

                if (!(CountSquareNeighbours(changeX, changeZ) >= 2 || map[changeX, changeZ] == 0))
                {
                    map[changeX, changeZ] = 0;
                    MapLocation tempData = new MapLocation();
                    tempData.x = changeX;
                    tempData.z = changeZ;
                    mapDatas.Insert(0, tempData);
                    moved = true;
                    break;
                }

            }
            if (!moved)
            {
                mapDatas.RemoveAt(0);
            }
        }
    }
}
