using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_W = 20;
    private static int SCREEN_H = 10;

    public float speed = 0.1f;
    private float time = 0;

    public Camera cam;
    public int size = 60;

    Cell[,] grid = new Cell[1200, 600];
    // Start is called before the first frame update
    void Start()
    {
        Console.Write("start fnc");
        PlaceCells();
        cam.orthographicSize=size;
    }

    // Update is called once per frame
    void Update()
    {
        if (size>60) size=60;
        if (time >= speed)
        {
            time = 0;
            CountNeighbors();
            Populate();
        }else {
            time += Time.deltaTime;
        }
    }

    void PlaceCells()
    {
        Console.Write("start");
        for (int y=0; y<SCREEN_H*size; y++)
        {
            for(int x=0; x<SCREEN_W*size;x++)
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x*2, y*2), Quaternion.identity) as Cell;
                cell.SetAlive(RandomAliveCell());
                grid[x,y] = cell;

            }
        }
    }

    bool RandomAliveCell(){
        int rand = UnityEngine.Random.Range(0,100);
        if(rand>70)
            return true;
        return false;
    }

    void CountNeighbors(){
        int SH = SCREEN_H * size;
        int SW = SCREEN_W * size;
        int numNeighbors;
        for (int y=0; y<SH; y++)
        {
            for(int x=0; x<SW;x++)
            {
                numNeighbors = 0;
                if (grid[(x-1+SW)%SW,(y+1)%SH].isAlive)
                    numNeighbors++;
                if (grid[(x-1+SW)%SW,(y)%SH].isAlive)
                    numNeighbors++;
                if (grid[(x-1+SW)%SW,(y-1+SH)%SH].isAlive)
                    numNeighbors++;

                if (grid[x,(y+1)%SH].isAlive)
                    numNeighbors++;
                if (grid[x,(y-1+SH)%SH].isAlive)
                    numNeighbors++;

                if (grid[(x+1)%SW,(y+1)%SH].isAlive)
                    numNeighbors++;
                if (grid[(x+1)%SW,(y)%SH].isAlive)
                    numNeighbors++;
                if (grid[(x+1)%SW,(y-1+SH)%SCREEN_H].isAlive)
                    numNeighbors++;

                grid[x,y].numNeighbors = numNeighbors;
            }
        }
    }

    void Populate()
    {
        for (int y=0; y<SCREEN_H*size; y++)
        {
            for(int x=0; x<SCREEN_W*size;x++)
            {
                 if (grid[x,y].isAlive)
                {
                    if (grid[x,y].numNeighbors != 2 && grid[x,y].numNeighbors != 3)
                        grid[x,y].SetAlive(false);
                }else{
                    if (grid[x,y].numNeighbors == 3)
                        grid[x,y].SetAlive(true);
                }
            }   
        }
    }
}
