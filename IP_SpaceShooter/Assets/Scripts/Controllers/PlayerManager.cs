using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerCount = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerCount);
        if (playerCount == 2)
        {
            playerCount = 0;
        }
    }

    public void PlayerJoined()
    {
        playerCount ++;
    }
}
