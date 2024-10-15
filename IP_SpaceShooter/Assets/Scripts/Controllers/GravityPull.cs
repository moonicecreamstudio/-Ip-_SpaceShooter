using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPull : MonoBehaviour
{
    public Rigidbody2D planetGravityRadius;

    void Start()
    {

    }
    void Update()
    {
        planetGravityRadius = GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Missile"))
        {
            other.SendMessage("InsideGravity", SendMessageOptions.DontRequireReceiver);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.SendMessage("InsideGravity", SendMessageOptions.DontRequireReceiver);
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Missile"))
        {
            other.SendMessage("OutsideGravity", SendMessageOptions.DontRequireReceiver);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.SendMessage("OutsideGravity", SendMessageOptions.DontRequireReceiver);
        }
    }
}
