using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseTrig : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // We start with 45
        // We apply cos to it
        // We apply acos to it (which is the inverse)
        // We should get 45 back
        // Debug.Log("Cos(45): " + Mathf.Cos(45 * Mathf.Deg2Rad));
        // Debug.Log("Cos(-45): " + Mathf.Cos(-45 * Mathf.Deg2Rad));

        // We start with 45
        // We apply cos to it
        // We apply acos to it (which is the inverse)
        // We should get 45 back
        // Debug.Log("ACos(Cos(45)): " + Mathf.Acos(Mathf.Cos(45 * Mathf.Deg2Rad)) * Mathf.Rad2Deg);

        // We start with -45
        // We apply cos to it
        // We apply acos to it (which is the inverse)
        // We should get -45 back
        // Debug.Log("ACos(Cos(-45)): " + Mathf.Acos(Mathf.Cos(-45 * Mathf.Deg2Rad)) * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
