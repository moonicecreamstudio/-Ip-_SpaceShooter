using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityKill : MonoBehaviour
{
    public Rigidbody2D center;

    void Start()
    {

    }
    void Update()
    {
        center = GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Missile"))
        {
            other.SendMessage("CenterSucked", SendMessageOptions.DontRequireReceiver);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.SendMessage("CenterSucked", SendMessageOptions.DontRequireReceiver);
        }
    }
}
