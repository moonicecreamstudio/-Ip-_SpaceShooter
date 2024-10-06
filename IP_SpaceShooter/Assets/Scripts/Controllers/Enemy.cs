using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    float movementSpeed = 1f;
    float maxEnemyDistance = 5f;
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        EnemyMovement();
        
    }

    public void EnemyMovement()
    {
        var step = movementSpeed * Time.deltaTime;
        float enemyToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (enemyToPlayer > maxEnemyDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }
}
