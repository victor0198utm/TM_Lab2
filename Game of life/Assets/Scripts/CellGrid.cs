using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    private Material lineM;
    public bool showMain = true;
    public bool showSub = true;
    public int gridSizeX = 320;
    public int gridSizeY = 160;
    public float startX = -1;
    public float startY = -1;
    public float startZ = 0;
    public float smallStep = 2;
    public float largeStep = 20;

    public Color mainColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    public Color subColor = new Color(0.9f, 0.9f, 0.9f, 1f);

    void CreateLineM (){
        if ( !lineM ){
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineM = new Material(shader);
            lineM.hideFlags = HideFlags.HideAndDontSave;

            lineM.SetInt("_SrcBlen", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineM.SetInt("_DstBlen", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            lineM.SetInt("_ZWrite", 0);
            lineM.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

        }
    }

    private void OnDisable(){
        DestroyImmediate(lineM);
    }

    private void OnPostRender(){
        CreateLineM();
        lineM.SetPass(0);
        GL.Begin(GL.LINES);

        if(showSub){
            GL.Color(subColor);

            for(float y = 0; y <= gridSizeY; y += smallStep){
                GL.Vertex3(startX, startY + y, startZ);
                GL.Vertex3(startX + gridSizeX, startY + y, startZ);
            }

            for(float x = 0; x <= gridSizeX; x += smallStep){
                GL.Vertex3(startX + x, startY, startZ);
                GL.Vertex3(startX + x, startY + gridSizeY, startZ);
            }
        }

        if(showMain){
            GL.Color(mainColor);

            for(float y = 0; y <= gridSizeY; y += largeStep){
                GL.Vertex3(startX, startY + y, startZ);
                GL.Vertex3(startX + gridSizeX, startY + y, startZ);
            }

            for(float x = 0; x <= gridSizeX; x += largeStep){
                GL.Vertex3(startX + x, startY, startZ);
                GL.Vertex3(startX + x, startY + gridSizeY, startZ);
            }
        }

        GL.End();
    }
}
