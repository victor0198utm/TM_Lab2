using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_W = 4;
    private static int SCREEN_H = 2;

    public int speed = 6;
    private float time = 0;

    public Camera cam;
    public Transform camPosition;
    public int size = 20;

    Cell[,] grid = new Cell[160, 80];

    public bool simulate = true;
    private int nowX = -10;
    private int nowY = -10;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1200, 600, false);
        Console.Write("start fnc");
        if (size>40) size=40;
        if (speed>10) speed=10;
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (size>40) size=40;
        if (size<1) size=1;
        if (speed>10) speed=10;
        if (speed<0) speed=0;

        cam.orthographicSize = size*2;
        camPosition.position = new Vector3(size*4-1, size*2-1, -10);

        if(simulate == true){
            if (time >= (10-speed)/50.0)
            {
                time = 0;
                CountNeighbors();
                Populate();
            }else {
                time += Time.deltaTime;
            }
        }
        
        UserInput();
    }

    void UserInput(){
        if(Input.GetMouseButton(0)){
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x/2);
            int y = Mathf.RoundToInt(mousePoint.y/2);
            if((x!=nowX || y!=nowY) && x>=0 && y>=0 && x<SCREEN_W*size && y<SCREEN_H*size){
                grid[x, y].SetAlive(!grid[x, y].isAlive);
                nowX = x;
                nowY = y;
            }
        }

        if(Input.GetKeyDown(KeyCode.P)){   
            simulate = !simulate;
            AudioListener.pause = !AudioListener.pause;
        }

        if(Input.GetKeyDown(KeyCode.C)){
            for (int y=0; y<SCREEN_H*size; y++)
                for(int x=0; x<SCREEN_W*size;x++)
                    grid[x,y].SetAlive(false);
        }

        if(Input.GetKeyDown(KeyCode.I)){   
            size++;
        }

        if(Input.GetKeyDown(KeyCode.D)){   
            size--;
        }
    }

    void PlaceCells()
    {
        Console.Write("start");
        for (int y=0; y<80; y++)
        {
            for(int x=0; x<160;x++)
            {
                
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x*2, y*2), Quaternion.identity) as Cell;
                if(x<SCREEN_W*size && y<SCREEN_H*size)
                    cell.SetAlive(RandomAliveCell());
                else cell.SetAlive(false);
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
        for (int y=0; y<SCREEN_H*40; y++)
        {
            for(int x=0; x<SCREEN_W*40;x++)
            {
                numNeighbors = 0;

                if(x-1>=0){
                    if(y+1<SH)
                        if (grid[x-1,y+1].isAlive)
                            numNeighbors++;
                    if (grid[x-1,y].isAlive)
                        numNeighbors++;
                    if(y-1>=0)
                        if (grid[x-1,y-1].isAlive)
                            numNeighbors++;
                }

                if(y+1<SH)
                    if (grid[x,y+1].isAlive)
                        numNeighbors++;
                if(y-1>=0)
                    if (grid[x,y-1].isAlive)
                        numNeighbors++;

                if(x+1<SW){
                    if(y+1<SH)
                        if (grid[x+1,y+1].isAlive)
                            numNeighbors++;
                    if (grid[x+1,y].isAlive)
                        numNeighbors++;
                    if(y-1>=0)
                        if (grid[x+1,y-1].isAlive)
                            numNeighbors++;
                }
                grid[x,y].numNeighbors = numNeighbors;

                if(x>SW || y>SH)
                    grid[x,y].numNeighbors = 0;
            }
        }
    }

    void Populate()
    {
        for (int y=0; y<SCREEN_H*40; y++)
        {
            for(int x=0; x<SCREEN_W*40;x++)
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
