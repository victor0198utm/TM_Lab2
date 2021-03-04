using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_W = 4;
    private static int SCREEN_H = 2;

    public int speed = 10;
    private float time = 0;

    public Camera cam;
    public Transform camPosition;
    public int size = 40;

    Cell[,] grid = new Cell[160, 80];
    // Start is called before the first frame update
    void Start()
    {
        Console.Write("start fnc");
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize = size*2;
        camPosition.position = new Vector3(size*4-1, size*2-1, -10);
        
        if (size>40) size=40;
        if (speed>10) speed=10;
        if (speed<0) speed=0;

        if (time >= (10-speed)/50.0)
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
                if (grid[(x+1)%SW,(y-1+SH)%SH].isAlive)
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
