using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime = 6;
    LineRenderer lineRenderer;
    int connectedDots = 2;
    public float timer = 0;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        lineRenderer.material = whiteDiffuseMat;
        lineRenderer.positionCount = connectedDots;
        lineRenderer.widthCurve = AnimationCurve.Linear(0, .05f, 1, .05f);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = connectedDots;
        timer += Time.deltaTime;
        if (timer > drawingTime) 
        {
            if (connectedDots < starTransforms.Count) 
            {
                connectedDots += 1;
            }
            timer = 0;
        }
        DrawLine();
    }

    public void DrawLine()
    {
        for (int i = 0; i < starTransforms.Count; i++)
        {
            lineRenderer.SetPosition(i , starTransforms[i].position);
        }
    }
}
