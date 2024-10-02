using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;
    List<float> angles = new List<float>() { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f, 360f };
    public float radius;
    public Vector3 offset;
    private int currentAngleIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Cos(0 * Mathf.Deg2Rad), Mathf.Sin(0 * Mathf.Deg2Rad)) * radius + planetTransform.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(radius, 5f, planetTransform);
    }

    public void OrbitalMotion(float radius, float speed, Transform target)
    {
        float currentAngle = angles[currentAngleIndex];
        float endPointX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float endPointY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 endingPoint = new Vector3(endPointX, endPointY) * radius + target.transform.position; ;
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endingPoint, step);

        if (transform.position == endingPoint)
        {
            currentAngleIndex++;
            if (currentAngleIndex >= angles.Count)
            {
                currentAngleIndex = 0;
            }
        }
    }
}
