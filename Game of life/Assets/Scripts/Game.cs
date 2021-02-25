using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_W = 120;
    private static int SCREEN_H = 60;

    public float speed = 0.1f;
    private float time = 0;

    Cell[,] grid = new Cell[SCREEN_W, SCREEN_H];
    // Start is called before the first frame update
    void Start()
    {
        Console.Write("start fnc");
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
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
        for (int y=0; y<SCREEN_H; y++)
        {
            for(int x=0; x<SCREEN_W;x++)
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
        int numNeighbors;
        for (int y=0; y<SCREEN_H; y++)
        {
            for(int x=0; x<SCREEN_W;x++)
            {
                numNeighbors = 0;
                if (grid[(x-1+SCREEN_W)%SCREEN_W,(y+1)%SCREEN_H].isAlive)
                    numNeighbors++;
                if (grid[(x-1+SCREEN_W)%SCREEN_W,(y)%SCREEN_H].isAlive)
                    numNeighbors++;
                if (grid[(x-1+SCREEN_W)%SCREEN_W,(y-1+SCREEN_H)%SCREEN_H].isAlive)
                    numNeighbors++;

                if (grid[x,(y+1)%SCREEN_H].isAlive)
                    numNeighbors++;
                if (grid[x,(y-1+SCREEN_H)%SCREEN_H].isAlive)
                    numNeighbors++;

                if (grid[(x+1)%SCREEN_W,(y+1)%SCREEN_H].isAlive)
                    numNeighbors++;
                if (grid[(x+1)%SCREEN_W,(y)%SCREEN_H].isAlive)
                    numNeighbors++;
                if (grid[(x+1)%SCREEN_W,(y-1+SCREEN_H)%SCREEN_H].isAlive)
                    numNeighbors++;

                grid[x,y].numNeighbors = numNeighbors;
            }
        }
    }

    void Populate()
    {
        for (int y=0; y<SCREEN_H; y++)
        {
            for(int x=0; x<SCREEN_W;x++)
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
