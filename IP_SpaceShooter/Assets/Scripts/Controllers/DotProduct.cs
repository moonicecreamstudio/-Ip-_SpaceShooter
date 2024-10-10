using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DotProduct : MonoBehaviour
{
    public float redAngle;
    public float blueAngle;

    Vector3 redVector;
    Vector3 blueVector;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        redVector.x = Mathf.Cos(redAngle * Mathf.Deg2Rad);
        redVector.y = Mathf.Sin(redAngle * Mathf.Deg2Rad);

        Debug.DrawLine(Vector3.zero, Vector3.Normalize(redVector), Color.red);

        blueVector.x = Mathf.Cos(blueAngle * Mathf.Deg2Rad);
        blueVector.y = Mathf.Sin(blueAngle * Mathf.Deg2Rad);

        Debug.DrawLine(Vector3.zero, Vector3.Normalize(blueVector), Color.blue);

        float redAndBlue = redVector.x * blueVector.x + redVector.y * blueVector.y;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("redAndBlue = " + redAndBlue);
        }
    }
}
